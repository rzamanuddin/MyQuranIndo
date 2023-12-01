using Java.Util;
using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.ViewModels.Juz;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Juz;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Views.Surah;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Tafsir
{
    [QueryProperty(nameof(SurahID), nameof(SurahID))]
    [QueryProperty(nameof(TafsirID), nameof(TafsirID))]
    public class TafsirDetailViewModel : BaseViewModel, IHasListViewViewModel
    {
        private string result = "", errorMessage = "";
        private string surahID;
        private string subTitle;
        private MyQuranIndo.Models.Qurans.Surah surah;
        private bool isBismillahShow;
        private string tafsirID;
        private ICommand _searchCommand;

        public bool IsBismillahShow
        {
            get => isBismillahShow;
            set => SetProperty(ref isBismillahShow, value);
        }

        public Models.Qurans.Surah Surah
        {
            get => surah;
            set => SetProperty(ref surah, value);
        }

        public string SubTitle
        {
            get => subTitle;
            set => SetProperty(ref subTitle, value);
        }
        public ObservableCollection<Models.Qurans.Tafsir> Tafsirs { get; set; }

        public string ID { get; set; }

        public string SurahID
        {
            get { return surahID; }
            set
            {
                surahID = value;
                LoadSurahID(value);
            }
        }

        public string TafsirID
        {
            get
            {
                return tafsirID;
            }
            set
            {
                tafsirID = value;
                LoadSurahID(surahID, value);
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

        public ICommand LoadCommand { get; private set; }
        public ICommand TafsirOneTapped { get; }
        public ICommand SettingTapped { get; }
        public ICommand GoBackCommand { get; }

        public TafsirDetailViewModel()
        {
            Title = "Surat";
            Tafsirs = new ObservableCollection<Models.Qurans.Tafsir>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            TafsirOneTapped = new Command<Models.Qurans.Tafsir>(OnTafsirOneTapped);
            GoBackCommand = new Command(async () => await OnBackTapped());
            SettingTapped = new Command(OnSettingTapped);
        }
        private async void OnSettingTapped()
        {
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        public async Task OnBackTapped()
        {
            await Shell.Current.GoToAsync("..");
        }

        public void ScrollToItem(Models.Qurans.Tafsir tafsir, bool isAnimated = false)
        {
            View.ListView.ScrollTo(tafsir, position: ScrollToPosition.Start, isAnimated);
        }

        public async void LoadSurahID(string surahID, string tafsirID = null)
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
                SubTitle = surah.DisplayNameLatin;
                Title = String.Format("{0}. {1}", surah.ID, surah.NameLatin);

                if (!string.IsNullOrEmpty(tafsirID) && tafsirID != "0" && tafsirID != "1")
                {
                    ScrollToItem(Tafsirs.FirstOrDefault(q => q.ID == Convert.ToInt32(tafsirID)));
                }
                await ExecuteLoadCommand();
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
                Tafsirs.Clear();

                int surahID = 0;
                int.TryParse(SurahID, out surahID);
                var tafsirs = await TafsirDataService.GetAsync(surahID);

                Models.Qurans.Tafsir t;

                //int i = 1;
                for (int i = 0; i < tafsirs.Count; i++)
                {
                    Tafsirs.Add(tafsirs[i]);
                }

                if (!string.IsNullOrEmpty(this.TafsirID) && this.TafsirID != "0" && this.TafsirID != "1")
                {
                    ScrollToItem(Tafsirs.FirstOrDefault(q => q.ID == Convert.ToInt32(this.Tafsirs)));
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

        private bool IsValidTapped(Models.Qurans.Tafsir tafsir)
        {
            if (tafsir == null)
            {
                return false;
            }

            // Dont show pop up menu at bismillah
            if (tafsir.ID == 0)
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
                    ScrollToItem(Tafsirs[0]);
                }
                else
                {
                    //MP3Service.StopPlayer();
                    MP3Service.StopPlayer();
                    int tafsirID = 0;
                    int.TryParse(text, out tafsirID);
                    string errorMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (tafsirID > 0)
                    {
                        if (tafsirID > 0 && ((SurahID == "1" || SurahID == "9") && tafsirID <= Tafsirs.Count)
                            || ((SurahID != "1" || SurahID != "9") && tafsirID < Tafsirs.Count))
                        {
                            ScrollToItem(Tafsirs.FirstOrDefault(q => q.ID == tafsirID));
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var t = Tafsirs.FirstOrDefault(q => q.TafsirText.ToLower().Contains(text));

                        if (t != null)
                        {
                            ScrollToItem(t);
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

        private async void OnTafsirOneTapped(Models.Qurans.Tafsir tafsir)
        {
            if (IsValidTapped(tafsir))
            {
                var oldColor = tafsir.RowColor;
                tafsir.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                result = "";
                errorMessage = "";
                var surah = await SurahDataService.GetSurahAsync(tafsir.SurahID);
                string action = await ActionHelper.DisplayActionTafsirAsync(tafsir.SurahID, tafsir.TafsirID, surah.NameLatin);
                string tafsirCopied = ActionHelper.GetTafsirToShare(tafsir, Surah.NameLatin);
                try
                {
                    switch (action)
                    {
                        case ActionHelper.TAFSIR_SHARE:
                            await ActionHelper.ShareAyahAsync(tafsirCopied);
                            break;
                        case ActionHelper.TAFSIR_COPY:
                            await Clipboard.SetTextAsync(tafsirCopied);
                            result = ActionHelper.TAFSIR_COPY + " Berhasil.";
                            break;
                        case ActionHelper.ACTION_LOOK_AS_SURAH:
                            await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}?{nameof(TabbedPageSurahDetailViewModel.SurahID)}={tafsir.SurahID}&{nameof(TabbedPageSurahDetailViewModel.AyahID)}={tafsir.TafsirID}");
                            break;
                        case ActionHelper.ACTION_LOOK_AS_JUZ:
                            var juzID = await JuzDataService.GetJuzIDAsync(tafsir.SurahID, tafsir.TafsirID);
                            await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}?{nameof(TabbedPageJuzDetailViewModel.JuzID)}={juzID}&{nameof(TabbedPageJuzDetailViewModel.SurahID)}={tafsir.SurahID}&{nameof(TabbedPageJuzDetailViewModel.AyahID)}={tafsir.TafsirID}");
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
                    tafsir.RowColor = oldColor;
                }
            }
        }

        public void OnAppearing()
        {
            //IsBusy = true;
        }
    }
}
