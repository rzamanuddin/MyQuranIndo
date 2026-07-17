using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Views.Juz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Juz
{
    [QueryProperty(nameof(JuzID), nameof(JuzID))]
    [QueryProperty(nameof(SurahID), nameof(SurahID))]
    [QueryProperty(nameof(AyahID), nameof(AyahID))]
    public class TabbedPageJuzDetailViewModel : BaseViewModel
    {
        public ObservableCollection<Page> Tabs { get; set; }
        private Page selectedTab;
        private string surahID;
        private string ayahID;

        public Page SelectedTab
        {
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

        private string juzID;
        public string JuzID
        {
            get
            {
                return juzID;
            }
            set
            {
                juzID = value;
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
                int jID = 0;
                int.TryParse(juzID, out jID);
                if (id > 0)
                {
                    LoadJuzDetail(jID, sID, id);
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

        public TabbedPageJuzDetailViewModel()
        {
            Title = "Juz (Mode Tab)";
            //SettingTapped = new Command(OnSettingSelected);
            //GoToCommand = new Command(OnGoToSelected);
            Tabs = new ObservableCollection<Page>();
            Initialization = InitStart();
        }

        //private async void OnGoToSelected()
        //{
        //    await Shell.Current.GoToAsync($"{nameof(SearchModalPage)}");
        //}

        //private async void OnSettingSelected()
        //{
        //    await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        //}

        private Task InitStart()
        {
            Tabs.Clear();

            for (int i = JuzHeader.JUZ_COUNT; i >= 1; i--)
            {
                var viewModel = new JuzDetailViewModel();
                viewModel.JuzID = i.ToString();
                viewModel.VisibleSearchBar = true;
                //viewModel.LoadJuzID(i.ToString());
                //viewModel.Title = $"Juz {i} (Total {juzHeaders[i-1].TotalAyah} Ayat)";

                Tabs.Add(new JuzDetailPage() { BindingContext = viewModel });
                //Tabs.Add(new SurahDetailPage());
            }

            SelectedTab = Tabs[GetTabIndex(1)];

            return Task.CompletedTask;
        }

        public async Task LoadJuzDetail()
        {
            MP3Service.StopPlayer();
            int juzID = 0;
            int.TryParse(JuzID, out juzID);

            // Validate
            if (juzID <= 0 || Tabs == null || Tabs.Count == 0)
                return;

            var idx = GetTabIndex(juzID);
            if (idx < 0 || idx >= Tabs.Count)
                return;

            var viewModel = Tabs[idx].BindingContext as JuzDetailViewModel;
            if (viewModel == null)
                return;

            // Load data if ayah is empty
            if (!viewModel.JuzDetails.Any(q => q.JuzID == juzID))
            {
                await viewModel.ExecuteLoadCommand();
            }
        }

        private int GetTabIndex(int juzId)
        {
            return Math.Abs(juzId - JuzHeader.JUZ_COUNT);
        }

        private void LoadJuzDetail(int juzID, int surahID, int ayahID)
        {
            if (ayahID > 0 && Tabs != null && Tabs.Count > 0)
            {
                var idx = GetTabIndex(juzID);
                if (idx < 0 || idx >= Tabs.Count)
                    return;

                SelectedTab = Tabs[idx];

                var page = (SelectedTab as JuzDetailPage);
                if (page?.BindingContext is JuzDetailViewModel vm)
                {
                    vm.SurahID = surahID.ToString();
                    vm.AyahID = ayahID.ToString();
                }
                //var juz = (page.BindingContext as JuzDetailViewModel).JuzDetails.FirstOrDefault(q => q.JuzID == juzID
                //    && q.SurahID == surahID && q.AyahID == ayahID);
                //(page.BindingContext as JuzDetailViewModel).LoadJuzID(juzID.ToString(),
                //    surahID.ToString(), ayahID.ToString());
            }
        }
    }
}
