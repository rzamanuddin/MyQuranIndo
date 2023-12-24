using Java.Util;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Hadiths;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.ViewModels.Juz;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Hadiths;
using MyQuranIndo.Views.Juz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            View.CollectionView.ScrollTo(index - 1, -1, position: ScrollToPosition.Start, isAnimated);
        }
        private void ScrollToItem(Narrator narrator, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(narrator, -1, position: ScrollToPosition.Start, isAnimated);
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
        {
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                {
                    ScrollToItem(1);
                }
                else
                {
                    int narratorId = 0;
                    int.TryParse(text, out narratorId);
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (narratorId > 0)
                    {
                        if (narratorId > 0 && narratorId <= Narrators.Count)
                        {
                            ScrollToItem(narratorId);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var s = Narrators.FirstOrDefault(q => q.Name.ToLower().Contains(text));
                        if (s != null)
                        {
                            ScrollToItem(s);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
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
            await Shell.Current.GoToAsync($"{nameof(HadithDetailPage)}?{nameof(HadithDetailViewModel.Slug)}={narrator.Slug.ToString()}");
            narrator.RowColor = oldColor;
            CanNavigate = true;
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                Narrators.Clear();
                var narrator = await HadithDataService.GetNarratorsAsync(true);
                for (int i = 0; i < narrator.Count; i++)
                {
                    narrator[i].ID = i + 1;
                    Narrators.Add(narrator[i]);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_HADITH
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            //IsBusy = true;
            SelectedNarattor = null;
        }
    }
}
