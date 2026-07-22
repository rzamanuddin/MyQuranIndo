using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models;
using MyQuranIndo.Models.Themes;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Views.Surah;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static Android.Resource;

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
                    _ = LoadSurahDetail(sID, id);
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

        private Task InitStart()
        {
            //Tabs.Clear();
            ////var surahs = await SurahDataService.GetAsync(true);
            //for (int i = 1; i <= Models.Qurans.Surah.SURAH_COUNT; i++)
            //{
            //    SurahDetailViewModel viewModel = new SurahDetailViewModel();
            //    viewModel.SurahID = i.ToString();
            //    viewModel.VisibleSearchBar = true;

            //    //await viewModel.ExecuteLoadCommand();
            //    Tabs.Add(new SurahDetailPage() { BindingContext = viewModel});
            //    //Tabs.Add(new SurahDetailPage());
            //}

            Tabs.Clear();            
            for (int i = Models.Qurans.Surah.SURAH_COUNT; i >= 1; i--)
            {
                SurahDetailViewModel viewModel = new SurahDetailViewModel();
                viewModel.SurahID = i.ToString();
                viewModel.VisibleSearchBar = true;
                Tabs.Add(new SurahDetailPage() { BindingContext = viewModel });
            }

            SelectedTab = Tabs[GetTabIndex(1)];

            return Task.CompletedTask;
        }

        public async Task LoadSurahDetail()
        {
            MP3Service.StopPlayer();
            int surahID = 0;
            int.TryParse(SurahID, out surahID);

            //// Load data if ayah is empty
            //var viewModel = (Tabs[surahID - 1].BindingContext as SurahDetailViewModel);
            ////viewModel.LoadSurahID(SurahID.ToString());
            //if (!viewModel.Ayahs.Any(q => q.SurahID == surahID))
            //{
            //    //viewModel.SurahID = surahID.ToString();
            //    await (viewModel).ExecuteLoadCommand();
            //}

            if (surahID <= 0 || Tabs == null || Tabs.Count == 0)
                return;

            var idx = GetTabIndex(surahID);
            if (idx < 0 || idx >= Tabs.Count)
                return;

            var viewModel = Tabs[idx].BindingContext as SurahDetailViewModel;
            if (viewModel == null)
                return;

            if (!viewModel.Ayahs.Any(q => q.SurahID == surahID))
            {
                await viewModel.ExecuteLoadCommand();
            }
        }

        private int GetTabIndex(int surahId)
        {
            return Math.Abs(surahId - Models.Qurans.Surah.SURAH_COUNT);
        }

        private async Task LoadSurahDetail(int surahID, int ayahID)
        {
            // Kunci utama: Pastikan loop "Tabs.Clear()" & "Tabs.Add()" selesai dulu!
            await Initialization;

            if (ayahID > 0 && Tabs != null && Tabs.Count > 0)
            {
                int targetIndex = GetTabIndex(surahID);
                if (targetIndex >= 0 && targetIndex < Tabs.Count)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SelectedTab = Tabs[targetIndex];

                        var page = (SelectedTab as SurahDetailPage);
                        if (page?.BindingContext is SurahDetailViewModel detailVM)
                        {
                            detailVM.AyahID = ayahID.ToString();
                        }
                    });
                }
            }
        }

        //private  void LoadSurahDetail(int surahID, int ayahID)
        //{
        //    if (ayahID > 0 && Tabs.Count > 0)
        //    {
        //        //SelectedTab = Tabs[surahID - 1];

        //        //var page = (Tabs[surahID - 1] as SurahDetailPage);
        //        ////(page.BindingContext as SurahDetailViewModel).SurahID = surahID.ToString();
        //        //(page.BindingContext as SurahDetailViewModel).AyahID = ayahID.ToString();
        //        ////(page.BindingContext as SurahDetailViewModel).LoadSurahID(surahID.ToString(), ayahID.ToString());

        //        SelectedTab = Tabs[GetTabIndex(surahID)];

        //        var page = (SelectedTab as SurahDetailPage);
        //        (page.BindingContext as SurahDetailViewModel).AyahID = ayahID.ToString();
        //    }
        //}
    }
}
