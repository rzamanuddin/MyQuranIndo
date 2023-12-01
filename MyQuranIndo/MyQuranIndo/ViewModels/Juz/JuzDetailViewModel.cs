using MyQuranIndo.Configuration;
using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Bookmarks;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.References;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Juz
{
    [QueryProperty(nameof(JuzID), nameof(JuzID))]
    [QueryProperty(nameof(SurahID), nameof(SurahID))]
    [QueryProperty(nameof(AyahID), nameof(AyahID))]
    public class JuzDetailViewModel : BaseViewModel, IHasListViewViewModel
    {
        private string result = "", errorMessage = "";

        private string juzID;
        private MyQuranIndo.Models.Qurans.JuzHeader juzHeader;
        private string subTitle;
        private bool isBismillahShow;
        private string ayahID;
        private ICommand _searchCommand;

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

        public MyQuranIndo.Models.Qurans.JuzHeader JuzHeader
        {
            get => juzHeader;
            set => SetProperty(ref juzHeader, value);
        }

        public string SubTitle
        {
            get => subTitle;
            set => SetProperty(ref subTitle, value);
        }

        public ObservableCollection<JuzDetail> JuzDetails { get; set; }

        public ObservableCollection<SurahGroup> SurahGroups { get; set; }

        public string ID { get; set; }
        public string JuzID
        {
            get
            {
                return juzID;
            }
            set
            {
                juzID = value;
                LoadJuzID(value);
            }
        }

        private string surahID;
        public string SurahID
        {
            get
            {
                return surahID;
            }
            set
            {
                surahID = value;
                //LoadJuzID(value);
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
                LoadJuzID(JuzID, SurahID, value);
            }
        }

        public IHasListView View
        {
            get; set;
        }

        private bool visibleSearchBar = false;
        public bool VisibleSearchBar
        {
            get => visibleSearchBar;
            set => SetProperty(ref visibleSearchBar, value);
        }

        public Command LoadCommand { get; private set; }
        public ICommand AyahOneTapped { get; }
        public ICommand AyahTwoTapped { get; }
        public ICommand SettingTapped { get; }
        public ICommand GoBackCommand { get; }

        public JuzDetailViewModel()
        {
            Title = "Juz";
            JuzDetails = new ObservableCollection<JuzDetail>();
            SurahGroups = new ObservableCollection<SurahGroup>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            //errorMessages = new Dictionary<string, ErrorMessage>();
            AyahOneTapped = new Command<JuzDetail>(OnAyahOneTapped);
            AyahTwoTapped = new Command<JuzDetail>(OnAyahTwoTapped);
            SettingTapped = new Command(OnSettingTapped);
            GoBackCommand = new Command(async() => await OnBackTapped());
        }

        private async void OnSettingTapped()
        {
            MP3Service.StopPlayer();
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        public void ScrollToItem(JuzDetail jusDetail, bool isAnimated = false)
        {
            View.ListView.ScrollTo(jusDetail, position: ScrollToPosition.Start, isAnimated); // don't forget check null
            //View.CollectionView.ScrollTo(ayah, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }

        public async void LoadJuzID(string juzID, string surahID = null, string ayahID = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(juzID) || juzID == "0")
                {
                    return;
                }

                var juzHeader = await JuzDataService.GetJuzHeaderAsync(Convert.ToInt32(juzID));
                JuzHeader = juzHeader;
                ID = juzHeader.ID.ToString();
                SubTitle = juzHeader.Description;
                Title = $"Juz {ID} ({juzHeader.TotalAyah} Ayat)";
                //Title = $"Juz {ID}";

                if (!string.IsNullOrEmpty(surahID) && surahID != "0" && !string.IsNullOrEmpty(ayahID) && ayahID != "0")
                {
                    ScrollToItem(JuzDetails.FirstOrDefault(q => q.SurahID == Convert.ToInt32(surahID) 
                        && q.AyahID == Convert.ToInt32(ayahID)));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_LOAD_JUZ
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
                MP3Service.StopPlayer();
                JuzDetails.Clear();
                SurahGroups.Clear();

                int juzID = 0;
                int.TryParse(JuzID, out juzID);

                if (juzID == 0)
                {
                    return;
                }

                //var juzHeader = await JuzDataService.GetJuzHeaderAsync(Convert.ToInt32(juzID));
                //JuzHeader = juzHeader;
                //ID = juzHeader.ID.ToString();
                //SubTitle = juzHeader.Description;
                //Title = $"Juz {ID} ({juzHeader.TotalAyah} Ayat)";
                ////Title = $"Juz {ID}";

                var juzDetails = await JuzDataService.GetJuzDetailAsync(juzID);

                JuzDetail jd;

                for (int i = 0; i < juzDetails.Count; i++)
                {
                    // Add bismillah except surah Al-Fatihah and At-Taubah
                    if (juzDetails[i].SurahID != 1 && juzDetails[i].SurahID != 9
                        && juzDetails[i].AyahID == 1)
                    {
                        jd = new JuzDetail();
                        jd.JuzID = juzID;
                        jd.SurahID = juzDetails[i].SurahID;
                        jd.SurahNameLatin = juzDetails[i].SurahNameLatin;
                        jd.SurahNumberOfAyah = juzDetails[i].SurahNumberOfAyah;
                        jd.SurahTranslateIndo = juzDetails[i].SurahTranslateIndo;
                        jd.AyahID = 0;
                        jd.ReadText = Bismillah;
                        jd.TextIndo = "bismillāhir-raḥmānir-raḥīm";
                        jd.TranslateIndo = BismillahTranslate;
                        
                        jd.HtmlRead = GetFormattedHtml(jd.ReadText);
                        JuzDetails.Add(jd);
                    }

                    JuzDetails.Add(juzDetails[i]);
                }

                //var juzList = new ObservableCollection<JuzDetail>();

                var surahGroups = juzDetails.ToLookup(q => q.SurahID);

                foreach (var sg in surahGroups)
                {
                    var juzList = new ObservableCollection<JuzDetail>();
                    var jds = JuzDetails.Where(q => q.SurahID == sg.Key).ToList();

                    for (int i = 0; i < jds.Count; i++)
                    {
                        juzList.Add(jds[i]);
                    }
                    SurahGroups.Add(new SurahGroup(sg.Key, jds.FirstOrDefault().SurahNameLatin, juzList.Where(q => q.AyahID > 0 && q.SurahID == sg.Key).Count(), juzList));
                }

                if (!string.IsNullOrEmpty(this.SurahID) && this.SurahID != "0" && !string.IsNullOrEmpty(this.AyahID) && this.AyahID != "0")
                {
                    ScrollToItem(JuzDetails.FirstOrDefault(q => q.SurahID == Convert.ToInt32(surahID)
                        && q.AyahID == Convert.ToInt32(ayahID)));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR
                    , Message.MSG_FAIL_GET_JUZ + Environment.NewLine + ex.Message, Message.MSG_OK);
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

        private bool IsValidTapped(JuzDetail juzDetail)
        {
            if (juzDetail == null)
            {
                return false;
            }

            // Dont show pop up menu at bismillah
            if (juzDetail.AyahID == 0)
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
                    ScrollToItem(JuzDetails[0]);
                }
                else
                {
                    MP3Service.StopPlayer();
                    int ayahID = 0;
                    int.TryParse(text, out ayahID);
                    string errorMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (ayahID > 0)
                    {
                        // TODO: set this
                        //if (ayahID > 0 && ((JuzID == "1" || JuzID == "9") && ayahID <= JuzDetails.Count)
                        //    || ((JuzID != "1" || JuzID != "9") && ayahID < JuzDetails.Count))
                        //{
                        //    ScrollToItem(JuzDetails.FirstOrDefault(q => q.AyahID == ayahID));
                        //}
                        //else
                        //{
                        //    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
                        //}
                    }
                    else
                    {
                        var a = JuzDetails.FirstOrDefault(q => q.TranslateIndo.ToLower().Contains(text)
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

        private async void OnAyahTwoTapped(JuzDetail juzDetail)
        {
            if (IsValidTapped(juzDetail))
            {
                MP3Service.StopPlayer();
                var oldColor = juzDetail.RowColor;
                // Show Tafsir
                try
                {
                    juzDetail.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                    var tafsir = await TafsirDataService.GetAsync(juzDetail.SurahID, juzDetail.AyahID);
                    //string tafsir = //getSurahID()Tafsir.TafsirID.TafsirKemenag.Text[ayah.Number];
                    string title = string.Format($"Tafsir {TafsirTypeHelper.GetTafsirTypeName()} Q.S {0}:{1}"
                        , juzDetail.SurahID, juzDetail.AyahID);

                    await ActionHelper.ShareTafsirAsync(title, tafsir.TafsirText);
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_TAFSIR + Environment.NewLine
                        + ex.Message, Message.MSG_OK);
                }
                finally
                {
                    juzDetail.RowColor = oldColor;
                }
            }
        }

        public async Task OnBackTapped()
        {
            MP3Service.StopPlayer();
            await Shell.Current.GoToAsync("..");
        }

        private async void OnAyahOneTapped(JuzDetail juzDetail)
        {
            if (IsValidTapped(juzDetail))
            {
                MP3Service.StopPlayer();
                var oldColor = juzDetail.RowColor;
                juzDetail.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                result = "";
                errorMessage = "";

                string action = await ActionHelper.DisplayActionAyahAsync(juzDetail.SurahID, juzDetail.AyahID, juzDetail.SurahNameLatin);
                string ayahCopied = ActionHelper.GetAyahToShare(juzDetail);
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
                            ActionHelper.BookmarkAyah(juzDetail.ToAyah(), juzDetail.SurahNameLatin, out result, out errorMessage);
                            break;
                        case ActionHelper.AYAH_LAST_READ:
                            var lastRead = await ActionHelper.SetLastReadAyahAsync(juzDetail.ToAyah(), juzDetail.SurahNameLatin, SurahDataService);
                            result = lastRead.Item1;
                            errorMessage = lastRead.Item2;
                            break;
                        case ActionHelper.PLAY_MP3:
                            await MP3Service.PlayMurottal(juzDetail.SurahID, juzDetail.AyahID, ToastService);
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
                    juzDetail.RowColor = oldColor;
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            //SelectedSurah = null;

            //ScrollToItem(11);
        }
    }
}
