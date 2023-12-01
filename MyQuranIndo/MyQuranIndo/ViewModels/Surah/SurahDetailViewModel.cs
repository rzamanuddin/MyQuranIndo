using MyQuranIndo.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using MyQuranIndo.References;
using Newtonsoft.Json;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Models.Bookmarks;
using MyQuranIndo.Configuration;
using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Fonts;

namespace MyQuranIndo.ViewModels.Surah
{
    [QueryProperty(nameof(SurahID), nameof(SurahID))]
    [QueryProperty(nameof(AyahID), nameof(AyahID))]
    public class SurahDetailViewModel : BaseViewModel, IHasListViewViewModel //TODO:
    {
        private string result = "", errorMessage = "";

        private string surahID;
        private MyQuranIndo.Models.Qurans.Surah surah;
        private string subTitle;
        private bool isBismillahShow;
        private string ayahID;
        private ICommand _searchCommand;

        //private ICommand _swipeCommand;

        public string Bismillah
        {
            get => "بِسْمِ اللّٰهِ الرَّحْمٰنِ الرَّحِيْمِ";
        }

        public string BismillahTranslate
        {
            get => "Dengan nama Allah Yang Maha Pengasih, Maha Penyayang.";
        }

        public bool IsBismillahShow
        {
            get => isBismillahShow;
            set => SetProperty(ref isBismillahShow, value);
        }

        public MyQuranIndo.Models.Qurans.Surah Surah
        {
            get => surah;
            set => SetProperty(ref surah, value);
        }

        public string SubTitle
        {
            get => subTitle;
            set => SetProperty(ref subTitle, value);
        }

        public ObservableCollection<MyQuranIndo.Models.Qurans.Ayah> Ayahs { get; set;  }

        public string ID { get; set; }
        public string SurahID
        {
            get
            {
                return surahID;
            }
            set
            {
                surahID = value;
                LoadSurahID(value);
            }
        }

        public string AyahID
        {
            get
            {
                return ayahID;
            }
            set
            {
                ayahID = value;
                LoadSurahID(SurahID, value);
            }
        }

        private bool visibleSearchBar = false;
        public bool VisibleSearchBar
        {
            get => visibleSearchBar;
            set => SetProperty(ref visibleSearchBar, value);
        }

        public IHasListView View
        {
            get; set;
        }        

        public Command LoadCommand { get; private set; }
        public ICommand AyahOneTapped { get; }
        public ICommand AyahTwoTapped { get; }
        public Command SettingTapped { get; }
        public ICommand GoBackCommand { get; }

        public SurahDetailViewModel()
        {
            Title = "Surah";
            Ayahs = new ObservableCollection<Ayah>();            
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            AyahOneTapped = new Command<Ayah>(OnAyahOneTapped);
            AyahTwoTapped = new Command<Ayah>(OnAyahTwoTapped);
            SettingTapped = new Command(OnSettingTapped);
            GoBackCommand = new Command(async () => await OnBackTapped());
        }

        private async void OnSettingTapped()
        {
            //MP3Helper.StopPlayer();
            MP3Service.StopPlayer();
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        public async Task OnBackTapped()
        {
            //MP3Service.StopPlayer();
            MP3Service.StopPlayer();
            await Shell.Current.GoToAsync("..");
        }
        public void ScrollToItem(Ayah ayah, bool isAnimated = false)
        {
            View.ListView.ScrollTo(ayah, position: ScrollToPosition.Start, isAnimated);
        }

        public async void LoadSurahID(string surahID, string ayahID = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(surahID) || surahID == "0")
                {
                    return;
                }

                var surah = await SurahDataService.GetSurahAsync(Convert.ToInt32(surahID));

                Surah = surah;
                ID = surah.ID.ToString();
                SubTitle = surah.DisplayNameLatin; // String.Format("{0}. {1} (2)", Surah.ID, surah.NameLatin, surah.DisplayNameLatin);
                Title = String.Format("{0}. {1}", surah.ID, surah.NameLatin);

                if (!string.IsNullOrEmpty(ayahID) && ayahID != "0" && ayahID != "1")
                {
                    ScrollToItem(Ayahs.FirstOrDefault(q => q.ID == Convert.ToInt32(ayahID)));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_LOAD_SURAH 
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadCommand()
        {
            try
            {
                //MP3Helper.StopPlayer();
                MP3Service.StopPlayer();
                Ayahs.Clear();

                int surahID = 0;
                int.TryParse(SurahID, out surahID);
                var ayahs = await AyahDataService.GetAsync(surahID);

                Ayah a;

                // Add bismillah except surah Al-Fatihah and At-Taubah
                if (surahID != 1 && surahID != 9)
                {
                    a = new Ayah();
                    a.SurahID = surahID;
                    a.ID = 0;
                    a.ReadText = Bismillah;
                    a.TextIndo = "bismillāhir-raḥmānir-raḥīm";
                    a.TranslateIndo = BismillahTranslate;
                    //a.ReadTajwidText = GetFormattedHtml(a.ReadText);
                    Ayahs.Add(a);
                }

                //int i = 1;
                for (int i = 0; i < ayahs.Count; i++)
                {
                    Ayahs.Add(ayahs[i]);
                }

                if (!string.IsNullOrEmpty(this.AyahID) && this.AyahID != "0" && this.AyahID != "1")
                {
                    ScrollToItem(Ayahs.FirstOrDefault(q => q.ID == Convert.ToInt32(this.AyahID)));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR
                    , Message.MSG_FAIL_GET_SURAH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string GetFormattedHtml(string ayahText)
        {
            string body = "";
            string header = "<html><head></head><body>";
            string footer = "</body></html>";

            //// Gunnah (Nun tasydid, Mim tasydid)
            //string[] gunnah = new string[] { "نَّ", "مَّ" };
            //string[] idghamBigunnah = new string[] { "نْ", "\u064B", "\u064C", "\u064D" };
            ////string ya = "\u0649";
            ////String s = "a\u0304\u0308bc\u0327\u0645";

            //TextElementEnumerator charEnum = StringInfo.GetTextElementEnumerator(ayahText);
            //while (charEnum.MoveNext())
            //{
            //    string text = charEnum.GetTextElement();
            //    var isGunnah = gunnah.Any(q => String.Equals(q, text, StringComparison.InvariantCultureIgnoreCase));
            //    //var isIdghamBigunnah = idghamBigunnah.Any(q => String.Equals(q, text, StringComparison.InvariantCultureIgnoreCase));
            //    if (isGunnah)
            //    {
            //        body += string.Format("<span style='color: green'>{0}</span>", text);
            //    }
            //    else
            //    {
            //        body += string.Format("<span>{0}</span>", text);
            //    }
            //}

            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamBigunnahs, TajwidHelper.COLOR_IDGHAM_BIGUNNAH);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamBilagunnahs, TajwidHelper.COLOR_IDGHAM_BILAGUNNAH);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Ikhfas, TajwidHelper.COLOR_IKHFA);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Iqlabs, TajwidHelper.COLOR_IQLAB);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamMimis, TajwidHelper.COLOR_IDGHAM_MIMI);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IkhfaSyafawis, TajwidHelper.COLOR_IKHFA_SYAFAWI);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Gunnahs, TajwidHelper.COLOR_GUNNAH);

            // TODO: set qalqalah
            // body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Qalqalahs, TajwidHelper.COLOR_QALQALAH);

            return ""; // header + body + footer;
        }

        private bool IsValidTapped(Ayah ayah)
        {
            if (ayah == null)
            {
                return false;
            }

            // Dont show pop up menu at bismillah
            if (ayah.ID == 0)
            {
                return false;
            }

            return true;
        }


        private async void SearchData(string text)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                {
                    ScrollToItem(Ayahs[0]);
                }
                else
                {
                    //MP3Service.StopPlayer();
                    MP3Service.StopPlayer();
                    int ayahID = 0;
                    int.TryParse(text, out ayahID);
                    string errorMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (ayahID > 0)
                    {
                        if (ayahID > 0 && ((SurahID == "1" || SurahID == "9") && ayahID <= Ayahs.Count) 
                            || ((SurahID != "1" || SurahID != "9") && ayahID < Ayahs.Count))
                        {
                            ScrollToItem(Ayahs.FirstOrDefault(q => q.ID == ayahID));
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var a = Ayahs.FirstOrDefault(q => q.TranslateIndo.ToLower().Contains(text)
                            || q.TextIndo.ToLower().Contains(text));

                        if (a != null)
                        {
                            ScrollToItem(a);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_FIND_AYAH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
        {
            SearchData(text);
        }));

        private async void OnAyahTwoTapped(Ayah ayah)
        {
            if (IsValidTapped(ayah))
            {
                //MP3Helper.StopPlayer();
                MP3Service.StopPlayer();
                var oldColor = ayah.RowColor;
                // Show Tafsir
                try
                {
                    ayah.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                    var tafsir = await TafsirDataService.GetAsync(getSurahID(), ayah.ID);
                    //string tafsir = //getSurahID()Tafsir.TafsirID.TafsirKemenag.Text[ayah.Number];
                    string title = $"Tafsir {TafsirTypeHelper.GetTafsirTypeName()} Q.S {ayah.SurahID}:{ayah.AyahID}";
                    await ActionHelper.ShareTafsirAsync(title, tafsir.TafsirText);
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_TAFSIR + Environment.NewLine
                        + ex.Message, Message.MSG_OK);
                }
                finally
                {
                    ayah.RowColor = oldColor;
                }
            }
        }

        private int getSurahID()
        {
            int surahID = 0;
            int.TryParse(SurahID, out surahID);
            return surahID;
        }
        private async void OnAyahOneTapped(Ayah ayah)
        {
            if (IsValidTapped(ayah))
            {
                //MP3Helper.StopPlayer();
                MP3Service.StopPlayer();
                var oldColor = ayah.RowColor;
                ayah.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                result = "";
                errorMessage = "";
                var surah = await SurahDataService.GetSurahAsync(ayah.SurahID);                
                string action = await ActionHelper.DisplayActionAyahAsync(ayah.SurahID, ayah.AyahID, surah.NameLatin);
                string ayahCopied = ActionHelper.GetAyahToShare(ayah, Surah.NameLatin);
                try
                {
                    switch (action)
                    {
                        case ActionHelper.AYAH_SHARE:
                            await ActionHelper.ShareAyahAsync(ayahCopied);
                            break;
                        case ActionHelper.AYAH_COPY:
                            await Clipboard.SetTextAsync(ayahCopied);
                            result = ActionHelper.AYAH_COPY + " Berhasil.";
                            break;
                        case ActionHelper.AYAH_BOOKMARK:
                            //SetBookmarkAyah(ayah);
                            ActionHelper.BookmarkAyah(ayah, Surah.NameLatin, out result, out errorMessage);
                            break;
                        case ActionHelper.AYAH_LAST_READ:
                            //await SetLastReadAyah(ayah);
                            var lastRead = await ActionHelper.SetLastReadAyahAsync(ayah, Surah.NameLatin, SurahDataService);
                            result = lastRead.Item1;
                            errorMessage = lastRead.Item2;
                            break;
                        case ActionHelper.PLAY_MP3:
                            //await MP3Helper.PlayMurottal(ayah.SurahID, ayah.AyahID, ToastService);
                            await MP3Service.PlayMurottal(ayah.SurahID, ayah.AyahID, ToastService);
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(this.result))
                    {
                        //await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, this.result, Message.MSG_OK);
                        ToastService.Show(this.result);
                    }
                    if (!string.IsNullOrWhiteSpace(this.errorMessage))
                    {
                        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, this.errorMessage, Message.MSG_OK);
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine
                        + ex.Message, Message.MSG_OK);
                }
                finally
                {
                    ayah.RowColor = oldColor;
                }
            }
        }

        //private async Task SetLastReadAyah(Ayah ayah)
        //{

        //    try
        //    {
        //        bool isConfirm = true;
        //        if (Preferences.ContainsKey(MenuKey.LAST_SURAH))
        //        {
        //            //string test = Preferences.Get(Key.LAST_SURAH, "");
        //            int lastSurah = Preferences.Get(MenuKey.LAST_SURAH, 0);
        //            int lastAyah = Preferences.Get(MenuKey.LAST_AYAH, 0);
        //            var lastSurahRead = await SurahDataService.GetSurahAsync(lastSurah);

        //            string lastRead = string.Format("Ayat terakhir dibaca adalah Q.S. {0}. {1} Ayat {2}, akan diganti menjadi Q.S. {3}. {4} Ayat {5}"
        //                , lastSurah
        //                , lastSurahRead.NameLatin
        //                , lastAyah
        //                , ayah.SurahID
        //                , Surah.NameLatin
        //                , ayah.ID);

        //            isConfirm = await App.Current.MainPage.DisplayAlert(Message.CONFIRM, lastRead, Message.MSG_YES, Message.MSG_NO);
        //        }

        //        if (isConfirm)
        //        {
        //            Preferences.Set(MenuKey.LAST_AYAH, ayah.ID);
        //            Preferences.Set(MenuKey.LAST_SURAH, ayah.SurahID);

        //            //await SecureStorage.SetAsync(Key.LAST_SURAH, Surah.ID.ToString());
        //            //await SecureStorage.SetAsync(Key.LAST_AYAH, ayah.Number.ToString());
        //            result = string.Format("Ayat terakhir dibaca berhasil diubah menjadi Q.S. {0}. {1} Ayat {2}."
        //                , getSurahID(), Surah.NameLatin, ayah.ID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = "Error" + Environment.NewLine + ex.Message;
        //    }
        //}

        //private void SetBookmarkAyah(Ayah ayah)
        //{
        //    result = "";
        //    errorMessage = "";
        //    try
        //    {
        //        List<Bookmark> bookmarks = new List<Bookmark>();
        //        if (Preferences.ContainsKey(MenuKey.BOOKMARK))
        //        {
        //            List<Bookmark> getBookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(Preferences.Get(MenuKey.BOOKMARK, null));
        //            bookmarks.AddRange(getBookmarks);
        //        }
        //        for (int i = 0; i < bookmarks.Count; i++)
        //        {
        //            if (bookmarks[i].SurahID == ayah.SurahID && bookmarks[i].AyahID == ayah.ID)
        //            {
        //                bookmarks.RemoveAt(i);
        //            }
        //        }
        //        Bookmark b = new Bookmark();
        //        b.AyahID = ayah.ID;
        //        b.CreatedDate = DateTime.Now;
        //        b.SurahID = ayah.SurahID;
        //        b.SurahNameLatin = Surah.NameLatin;
        //        b.Row = bookmarks.Count + 1;

        //        bookmarks.Add(b);
        //        Preferences.Set(MenuKey.BOOKMARK, JsonConvert.SerializeObject(bookmarks));

        //        result = string.Format("Q.S {0}:{1} berhasil di bookmark."
        //            , getSurahID(), ayah.ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = "Error" + Environment.NewLine + ex.Message;
        //    }
        //}

        public void OnAppearing()
        {
            IsBusy = true;
            //SelectedSurah = null;
        }
    }
}
