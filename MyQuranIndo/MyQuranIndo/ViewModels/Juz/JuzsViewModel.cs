using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Juz;
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
            //SurahFilter = new SurahFilter();
            Juzs = new ObservableCollection<Models.Qurans.JuzHeader>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ItemTapped = new Command<Models.Qurans.JuzHeader>(OnJuzSelected);
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
        private void ScrollToItem(Models.Qurans.JuzHeader juzHeader, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(juzHeader, -1, position: ScrollToPosition.Start, isAnimated);
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
                    int juzID = 0;
                    int.TryParse(text, out juzID);
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (juzID > 0)
                    {
                        if (juzID > 0 && juzID <= Juzs.Count)
                        {
                            ScrollToItem(juzID);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var s = Juzs.FirstOrDefault(q => q.SurahNameStart.ToLower().Contains(text)
                            || q.SurahNameEnd.ToLower().Contains(text));
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

        private async void OnJuzSelected(Models.Qurans.JuzHeader juz)
        {
            if (juz == null)
            {
                return;
            }
            if (juz.ID == 0)
            {
                return;
            }
            var oldColor = juz.RowColor;
            juz.RowColor = (Color)Application.Current.Resources["SelectedItem"];
            await Shell.Current.GoToAsync($"{nameof(JuzDetailPage)}?{nameof(JuzDetailViewModel.JuzID)}={juz.ID.ToString()}");
            juz.RowColor = oldColor;
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                Juzs.Clear();
                var juzs = await JuzDataService.GetJuzHeaderAsync(true);
                for (int i = 0; i < juzs.Count; i++)
                {
                    Juzs.Add(juzs[i]);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_JUZ
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
            SelectedJuz = null;
        }
    }
}
