using MyQuranIndo.Models.Qurans;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Views.Surah;
using MyQuranIndo.Views.Zikr;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Zikr
{
    public class TabbedPageIntentionViewModel : BaseViewModel, IAsyncInitialization    
    {
        public ObservableCollection<Page> Tabs { get; set; }
        private Page selectedTab;

        public Page SelectedTab
        {
            get => selectedTab;
            set => SetProperty(ref selectedTab, value);
        }

        internal void OnAppearing()
        {
            IsBusy = true;
        }

        public Task Initialization { get; private set; }
        public Command SettingTapped { get; }
        public ICommand GoToCommand { get; }

        public TabbedPageIntentionViewModel()
        {
            Title = "Sholat";
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
            for (int i = 1; i <= 2; i++)
            {
                var viewModel = new IntentionViewModel(i);
                //viewModel.VisibleSearchBar = true;

                //await viewModel.ExecuteLoadCommand();
                Tabs.Add(new IntentionsPage() { BindingContext = viewModel });                
                //await viewModel.ExecuteLoadCommand();
                //Tabs.Add(new SurahDetailPage());
            }

            SelectedTab = Tabs[0];
        }
    }
}
