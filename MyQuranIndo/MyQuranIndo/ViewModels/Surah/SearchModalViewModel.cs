using MyQuranIndo.Messages;
using MyQuranIndo.Views.Surah;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Surah
{
    public class SearchModalViewModel : BaseViewModel
    {
        private ObservableCollection<Models.Qurans.Surah> surahs;
        private ObservableCollection<Models.Qurans.Ayah> ayahs;
        private Models.Qurans.Surah selectedSurah;
        private Models.Qurans.Ayah selectedAyah;

        public ICommand LoadCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand OkCommand { get; }

        public ObservableCollection<Models.Qurans.Surah> Surahs
        {
            get => surahs;
            set => SetProperty(ref surahs, value);
        }
        public ObservableCollection<Models.Qurans.Ayah> Ayahs
        {
            get => ayahs;
            set => SetProperty(ref ayahs, value);
        }
        public Models.Qurans.Surah SelectedSurah
        {
            get => selectedSurah;
            set
            {
                SetProperty(ref selectedSurah, value);
                LoadAyahs(value.ID);
            }
        }
        public Models.Qurans.Ayah SelectedAyah
        {
            get => selectedAyah;
            set => SetProperty(ref selectedAyah, value);
        }

        public SearchModalViewModel()
        {
            Surahs = new ObservableCollection<Models.Qurans.Surah>();
            Ayahs = new ObservableCollection<Models.Qurans.Ayah>();
            CancelCommand = new Command(async () => await OnCancelCommand());
            OkCommand = new Command(async () => await OnOkCommand());
            LoadData();
        }

        private async Task OnOkCommand()
        {
            
            await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}?{nameof(TabbedPageSurahDetailViewModel.SurahID)}={SelectedSurah.ID}&{nameof(TabbedPageSurahDetailViewModel.AyahID)}={SelectedAyah.ID}");
            //await Shell.Current.Navigation.PushModalAsync(new NavigationPage(new TabbedPageSurahDetailPage()));
        }

        private async Task OnCancelCommand()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void LoadData()
        {
            try
            {
                Surahs.Clear();

                var getSurahs = await SurahDataService.GetSurahNewAsync(true);
                for (int i = 0; i < getSurahs.Count; i++)
                {
                    Surahs.Add(getSurahs[i]);
                }
                SelectedSurah = Surahs.FirstOrDefault();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_SURAH
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
        }

        private async void LoadAyahs(int surahID)
        {
            Ayahs.Clear();

            var getAyahs = await AyahDataService.GetAsync(surahID, true);
            for (int i = 0; i < getAyahs.Count; i++)
            {
                Ayahs.Add(getAyahs[i]);
            }
            SelectedAyah = Ayahs.FirstOrDefault();
        }
    }
}
