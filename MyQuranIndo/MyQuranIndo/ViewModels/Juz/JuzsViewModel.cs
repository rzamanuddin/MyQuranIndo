using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Juz;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Juz
{
    public class JuzsViewModel : BaseViewModel, IHasCollectionViewModel
    {
        private Models.Qurans.JuzHeader _selectedJuz;
        private ICommand _searchCommand;

        public ObservableCollection<Models.Qurans.JuzHeader> Juzs { get; }
        public Command LoadCommand { get; }
        public ICommand ItemTapped { get; }

        public Models.Qurans.JuzHeader SelectedJuz
        {
            get => _selectedJuz;
            set
            {
                SetProperty(ref _selectedJuz, value);
                OnJuzSelected(value);
            }
        }

        public IHasCollectionView View { get; set; }
        public Task Initialization { get; private set; }

        public JuzsViewModel()
        {
            Title = "Daftar Juz";
            Juzs = new ObservableCollection<Models.Qurans.JuzHeader>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ItemTapped = new Command<Models.Qurans.JuzHeader>(OnJuzSelected, (x) => CanNavigate);
            Initialization = InitStart();
        }

        private async Task InitStart()
        {
            await ExecuteLoadCommand();
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

        private void ScrollToItem(Models.Qurans.JuzHeader juzHeader, bool isAnimated = false)
        {
            if (View?.CollectionView != null && juzHeader != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    View.CollectionView.ScrollTo(juzHeader, -1, position: ScrollToPosition.Start, isAnimated);
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
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (int.TryParse(keyword, out int juzID) && juzID > 0)
                    {
                        if (juzID <= Juzs.Count)
                        {
                            ScrollToItem(juzID);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var foundJuz = Juzs.FirstOrDefault(q =>
                            (q.SurahNameStart != null && q.SurahNameStart.ToLower().Contains(keyword)) ||
                            (q.SurahNameEnd != null && q.SurahNameEnd.ToLower().Contains(keyword)));

                        if (foundJuz != null)
                        {
                            ScrollToItem(foundJuz);
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

        private async void OnJuzSelected(Models.Qurans.JuzHeader juz)
        {
            if (juz == null || juz.ID == 0) return;

            CanNavigate = false;
            var oldColor = juz.RowColor;
            juz.RowColor = (Color)Application.Current.Resources["SelectedItem"];

            await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}?{nameof(TabbedPageJuzDetailViewModel.JuzID)}={juz.ID}");

            juz.RowColor = oldColor;
            CanNavigate = true;
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                var fetchedJuzs = await JuzDataService.GetJuzHeaderAsync(true);
                if (fetchedJuzs == null) return;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Juzs.Clear();
                    foreach (var item in fetchedJuzs)
                    {
                        Juzs.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_JUZ + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            SelectedJuz = null;
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