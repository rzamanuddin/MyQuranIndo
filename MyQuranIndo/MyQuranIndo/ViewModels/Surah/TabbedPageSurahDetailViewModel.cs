using MyQuranIndo.Models;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Views.Surah;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.Windows.Input;
using MyQuranIndo.Messages;
using MyQuranIndo.Helpers;

namespace MyQuranIndo.ViewModels.Surah
{
    [QueryProperty(nameof(SurahID), nameof(SurahID))]
    [QueryProperty(nameof(AyahID), nameof(AyahID))]
    public class TabbedPageSurahDetailViewModel : BaseViewModel, IAsyncInitialization
    {
        public ObservableCollection<Page> Tabs { get; set; }
        private Page selectedTab;
        private string surahID;
        private string ayahID;

        public Page SelectedTab
        {
            //get { return _selectedtab; }
            //set
            //{

            //    RaisePropertyChanged("selectedtab");
            //}
            get => selectedTab;
            set => SetProperty(ref selectedTab, value);
        }
        public string SurahID
        {
            get
            {
                return surahID;
            }
            set
            {
                surahID = value;

                //int id = 0;
                //int.TryParse(value, out id);
                //LoadSurahDetail(id);
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

                int id = 0;
                int.TryParse(value, out id);
                int sID = 0;
                int.TryParse(surahID, out sID);
                if (id > 0)
                {
                    LoadSurahDetail(sID, id);
                }
            }
        }

        internal void OnAppearing()
        {
            IsBusy = true;
        }

        public Task Initialization { get; private set; }
        public Command SettingTapped { get; }
        public ICommand GoToCommand { get; }

        public TabbedPageSurahDetailViewModel()
        {
            Title = "Baca Al-Qur'an (Mode Tab)";
            SettingTapped = new Command(OnSettingSelected);
            GoToCommand = new Command(OnGoToSelected);
            Tabs = new ObservableCollection<Page>();
            Initialization = InitStart();
        }

        private async void OnGoToSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(SearchModalPage)}");
        }

        private async void OnSettingSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        private async Task InitStart()
        {
            Tabs.Clear();
            //var surahs = await SurahDataService.GetAsync(true);
            for (int i = 1; i <= 114; i++)
            {
                SurahDetailViewModel viewModel = new SurahDetailViewModel();
                viewModel.SurahID = i.ToString();
                viewModel.VisibleSearchBar = true;

                //await viewModel.ExecuteLoadCommand();
                Tabs.Add(new SurahDetailPage() { BindingContext = viewModel});
                //Tabs.Add(new SurahDetailPage());
            }

            SelectedTab = Tabs[0];
        }

        public async void LoadSurahDetail()
        {
            MP3Service.StopPlayer();
            int surahID = 0;
            int.TryParse(SurahID, out surahID);

            // Load data if ayah is empty
            var viewModel = (Tabs[surahID - 1].BindingContext as SurahDetailViewModel);
            //viewModel.LoadSurahID(SurahID.ToString());
            if (!viewModel.Ayahs.Any(q => q.SurahID == surahID))
            {
                //viewModel.SurahID = surahID.ToString();
                await (viewModel).ExecuteLoadCommand();
            }
        }

        private  void LoadSurahDetail(int surahID, int ayahID)
        {
            if (ayahID > 0 && Tabs.Count > 0)
            {
                SelectedTab = Tabs[surahID - 1];

                var page = (Tabs[surahID - 1] as SurahDetailPage);
                //(page.BindingContext as SurahDetailViewModel).SurahID = surahID.ToString();
                (page.BindingContext as SurahDetailViewModel).AyahID = ayahID.ToString();
                //(page.BindingContext as SurahDetailViewModel).LoadSurahID(surahID.ToString(), ayahID.ToString());
            }
        }
    }
}
