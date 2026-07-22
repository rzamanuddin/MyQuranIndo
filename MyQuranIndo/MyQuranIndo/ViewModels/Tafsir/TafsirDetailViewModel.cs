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
        private Models.Qurans.Surah surah;
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
            get => surahID;
            set
            {
                surahID = value;
                LoadSurahID(value);
            }
        }

        public string TafsirID
        {
            get => tafsirID;
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

        public IHasListView View { get; set; }

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
            if (View?.ListView != null && tafsir != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    View.ListView.ScrollTo(tafsir, position: ScrollToPosition.Start, isAnimated);
                });
            }
        }

        public async void LoadSurahID(string targetSurahID, string targetTafsirID = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(targetSurahID) || targetSurahID == "0")
                {
                    return;
                }

                var fetchedSurah = await SurahDataService.GetSurahAsync(Convert.ToInt32(targetSurahID));
                if (fetchedSurah != null)
                {
                    Surah = fetchedSurah;
                    ID = fetchedSurah.ID.ToString();
                    SubTitle = fetchedSurah.DisplayNameLatin;
                    Title = $"{fetchedSurah.ID}. {fetchedSurah.NameLatin}";
                }

                await ExecuteLoadCommand();

                if (!string.IsNullOrEmpty(targetTafsirID) && targetTafsirID != "0" && targetTafsirID != "1")
                {
                    if (int.TryParse(targetTafsirID, out int parsedTafsirID))
                    {
                        var targetItem = Tafsirs.FirstOrDefault(q => q.ID == parsedTafsirID);
                        ScrollToItem(targetItem);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_LOAD_SURAH + Environment.NewLine + ex.Message, Message.MSG_OK);
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
                if (!int.TryParse(SurahID, out int parsedSurahID) || parsedSurahID == 0)
                {
                    return;
                }

                var tafsirsList = await TafsirDataService.GetAsync(parsedSurahID);
                if (tafsirsList == null) return;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Tafsirs.Clear();
                    foreach (var item in tafsirsList)
                    {
                        Tafsirs.Add(item);
                    }
                });

                if (!string.IsNullOrEmpty(TafsirID) && TafsirID != "0" && TafsirID != "1")
                {
                    if (int.TryParse(TafsirID, out int parsedTafsirID))
                    {
                        var targetItem = Tafsirs.FirstOrDefault(q => q.ID == parsedTafsirID);
                        ScrollToItem(targetItem);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_SURAH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool IsValidTapped(Models.Qurans.Tafsir tafsir)
        {
            if (tafsir == null || tafsir.ID == 0)
            {
                return false;
            }
            return true;
        }

        private async void SearchData(string text)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    if (Tafsirs.Count > 0)
                        ScrollToItem(Tafsirs[0]);
                }
                else
                {
                    MP3Service.StopPlayer();
                    string keyword = text.Trim();
                    string errorMessageText = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (int.TryParse(keyword, out int tafsirID) && tafsirID > 0)
                    {
                        bool isValidRange = false;
                        if ((SurahID == "1" || SurahID == "9") && tafsirID <= Tafsirs.Count)
                        {
                            isValidRange = true;
                        }
                        else if (SurahID != "1" && SurahID != "9" && tafsirID <= Tafsirs.Count)
                        {
                            isValidRange = true;
                        }

                        if (isValidRange)
                        {
                            var targetItem = Tafsirs.FirstOrDefault(q => q.ID == tafsirID);
                            ScrollToItem(targetItem);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, errorMessageText, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var matchingItem = Tafsirs.FirstOrDefault(q => q.TafsirText != null && q.TafsirText.ToLower().Contains(keyword.ToLower()));

                        if (matchingItem != null)
                        {
                            ScrollToItem(matchingItem);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, errorMessageText, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_FIND_AYAH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(SearchData));

        private async void OnTafsirOneTapped(Models.Qurans.Tafsir tafsir)
        {
            if (!IsValidTapped(tafsir)) return;

            var oldColor = tafsir.RowColor;
            tafsir.RowColor = (Color)Application.Current.Resources["SelectedItem"];

            result = "";
            errorMessage = "";
            try
            {
                var targetSurah = await SurahDataService.GetSurahAsync(tafsir.SurahID);
                string surahName = targetSurah?.NameLatin ?? string.Empty;

                string action = await ActionHelper.DisplayActionTafsirAsync(tafsir.SurahID, tafsir.TafsirID, surahName);
                string tafsirCopied = ActionHelper.GetTafsirToShare(tafsir, Surah?.NameLatin ?? string.Empty);

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

                if (!string.IsNullOrWhiteSpace(result))
                {
                    ToastService.Show(result);
                }
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await DisplayAlertSafe(Message.MSG_TITLE_ERROR, errorMessage, Message.MSG_OK);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                tafsir.RowColor = oldColor;
            }
        }

        public void OnAppearing()
        {
            // Optional logic on appearing
        }

        private async Task DisplayAlertSafe(string title, string message, string cancel)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (App.Current?.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert(title, message, cancel);
                }
            });
        }
    }
}