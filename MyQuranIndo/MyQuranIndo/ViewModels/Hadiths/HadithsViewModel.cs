using MyQuranIndo.Messages;
using MyQuranIndo.Models.Hadiths;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Hadiths;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Hadiths
{
    public class HadithsViewModel : BaseViewModel, IHasCollectionViewModel
    {
        private Narrator _selectedNarrator;
        private ICommand _searchCommand;

        public ObservableCollection<Narrator> Narrators { get; }
        public Command LoadCommand { get; }
        public ICommand ItemTapped { get; }

        public Narrator SelectedNarattor
        {
            get => _selectedNarrator;
            set
            {
                SetProperty(ref _selectedNarrator, value);
                OnNarratorSelected(value);
            }
        }

        public IHasCollectionView View { get; set; }
        public Task Initialization { get; private set; }

        public HadithsViewModel()
        {
            Title = "Daftar Perawi Hadis";
            Narrators = new ObservableCollection<Narrator>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ItemTapped = new Command<Narrator>(OnNarratorSelected, (x) => CanNavigate);
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

        private void ScrollToItem(Narrator narrator, bool isAnimated = false)
        {
            if (View?.CollectionView != null && narrator != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    View.CollectionView.ScrollTo(narrator, -1, position: ScrollToPosition.Start, isAnimated);
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
                    int narratorId = 0;
                    int.TryParse(keyword, out narratorId);
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (narratorId > 0)
                    {
                        if (narratorId <= Narrators.Count)
                        {
                            ScrollToItem(narratorId);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var s = Narrators.FirstOrDefault(q => q.Name != null && q.Name.ToLower().Contains(keyword));
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

        private async void OnNarratorSelected(Narrator narrator)
        {
            if (narrator == null)
            {
                return;
            }

            CanNavigate = false;
            var oldColor = narrator.RowColor;
            narrator.RowColor = (Color)Application.Current.Resources["SelectedItem"];

            await Shell.Current.GoToAsync($"{nameof(HadithDetailPage)}?{nameof(HadithDetailViewModel.Slug)}={narrator.Slug}");

            narrator.RowColor = oldColor;
            CanNavigate = true;
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                var narratorList = await HadithDataService.GetNarratorsAsync(true);
                if (narratorList == null) return;

                var tempList = new List<Narrator>();
                for (int i = 0; i < narratorList.Count; i++)
                {
                    narratorList[i].ID = i + 1;
                    tempList.Add(narratorList[i]);
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Narrators.Clear();
                    foreach (var item in tempList)
                    {
                        Narrators.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_HADITH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            SelectedNarattor = null;
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