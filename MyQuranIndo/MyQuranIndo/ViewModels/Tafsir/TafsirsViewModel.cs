using MyQuranIndo.Messages;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Tafsir;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Tafsir
{
    public class TafsirsViewModel : BaseViewModel, IHasCollectionViewModel
    {
        private Models.Qurans.Surah _selectedSurah;
        private ICommand _searchCommand;

        public ObservableCollection<Models.Qurans.Surah> Surahs { get; }
        public Command LoadCommand { get; }
        public Command<Models.Qurans.Surah> ItemTapped { get; }

        public Models.Qurans.Surah SelectedSurah
        {
            get => _selectedSurah;
            set
            {
                SetProperty(ref _selectedSurah, value);
                OnSurahSelected(value);
            }
        }

        public IHasCollectionView View { get; set; }
        public Task Initialization { get; private set; }

        public TafsirsViewModel()
        {
            Title = "Daftar Tafsir";
            Surahs = new ObservableCollection<Models.Qurans.Surah>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ItemTapped = new Command<Models.Qurans.Surah>(OnSurahSelected, (x) => CanNavigate);
            Initialization = InitStart();
        }

        private void ScrollToItem(int index, bool isAnimated = false)
        {
            if (View?.CollectionView != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    View.CollectionView.ScrollTo(index - 1, -1, position: ScrollToPosition.Start, isAnimated);
                });
            }
        }

        private void ScrollToItem(Models.Qurans.Surah surah, bool isAnimated = false)
        {
            if (View?.CollectionView != null && surah != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    View.CollectionView.ScrollTo(surah, -1, position: ScrollToPosition.Start, isAnimated);
                });
            }
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
        {
            try
            {
                string keyword = text?.Trim()?.ToLower() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    ScrollToItem(1);
                }
                else
                {
                    int surahID = 0;
                    int.TryParse(keyword, out surahID);
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (surahID > 0)
                    {
                        if (surahID <= Surahs.Count)
                        {
                            ScrollToItem(surahID);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var s = Surahs.FirstOrDefault(q => q.NameLatin != null && q.NameLatin.ToLower().Contains(keyword));
                        if (s != null)
                        {
                            ScrollToItem(s);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }));

        private async void OnSurahSelected(Models.Qurans.Surah surah)
        {
            if (surah == null || surah.ID == 0)
            {
                return;
            }

            CanNavigate = false;
            var oldColor = surah.RowColor;
            surah.RowColor = (Color)Application.Current.Resources["SelectedItem"];

            await Shell.Current.GoToAsync($"{nameof(TafsirDetailPage)}?{nameof(TafsirDetailViewModel.SurahID)}={surah.ID}");

            surah.RowColor = oldColor;
            CanNavigate = true;
        }

        private async Task InitStart()
        {
            await ExecuteLoadCommand();
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                var surahList = await SurahDataService.GetSurahNewAsync(true);
                if (surahList == null) return;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Surahs.Clear();
                    foreach (var item in surahList)
                    {
                        Surahs.Add(item);
                    }
                });
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

        public void OnAppearing()
        {
            SelectedSurah = null;
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