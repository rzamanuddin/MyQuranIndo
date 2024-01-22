using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MyQuranIndo.Views.Surah;
using MyQuranIndo.Views.About;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Views.Finds;
using MyQuranIndo.Views.Help;
using MyQuranIndo.Views.Bookmarks;
using Xamarin.Essentials;
using MyQuranIndo.Messages;
using MyQuranIndo.References;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Views.Prayer;
using MyQuranIndo.Views.Qibla;
using MyQuranIndo.Views.Juz;
using MyQuranIndo.Helpers;
using MyQuranIndo.Views.Zikr;
using MyQuranIndo.ViewModels.Zikr;
using MyQuranIndo.Views.AsmaulHusna;
using MyQuranIndo.Views.Tafsir;
using MyQuranIndo.Views.Hadiths;
using MyQuranIndo.ViewModels.Juz;

namespace MyQuranIndo.ViewModels
{   public class AppShellViewModel : BaseViewModel
    {
        public ICommand ReadSurahModeTabCommand { get; }
        public ICommand ReadSurahCommand { get; }
        public ICommand ReadJuzModeTabCommand { get; }
        public ICommand ReadJuzCommand { get; }
        public ICommand ReadTafsirCommand {  get; }
        public ICommand FindCommand { get; }
        public ICommand HelpCommand { get; }
        public ICommand SettingCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand LastReadCommand { get; }
        public ICommand BookmarkCommand { get; }
        public ICommand PrayerScheduleCommand { get; }
        public ICommand QiblaCommand { get; }
        public ICommand ZikrMorningCommand { get; }
        public ICommand ZikrEveningCommand { get; }
        public ICommand AsmaulHusnaCommand { get; }
        public ICommand IntentionCommand { get; }
        public ICommand PrayCommand { get; }
        public ICommand HadithCommand { get; }
        public ICommand JuzAmmaCommand { get; }

        public AppShellViewModel()
        {
            ReadSurahModeTabCommand = new Command(async () => await NavigateToReadQuranModeTabPage());
            ReadSurahCommand = new Command(async (x) => await NavigateToReadQuranPage(), (x) => CanNavigate);
            ReadJuzModeTabCommand = new Command(async () => await NavigateToReadJuzModeTabPage());
            ReadJuzCommand = new Command(async () => await NavigateToReadJuzPage());
            ReadTafsirCommand = new Command(async () => await NavigateToReadTafsirPage());
            FindCommand = new Command(async () => await NavigateToFindPage());
            HelpCommand = new Command(async () => await NavigateToHelpPage());
            SettingCommand = new Command(async () => await NavigateToSettingPage());
            AboutCommand = new Command(async () => await NavigateToAboutPage());
            LastReadCommand = new Command(async () => await NavigateToLastReadPage());
            BookmarkCommand = new Command(async () => await NavigateToBookmarkPage());
            PrayerScheduleCommand = new Command(async () => await NavigateToPraySchedulePage());
            QiblaCommand = new Command(async () => await NavigateToQiblaPage());
            ZikrMorningCommand = new Command(async () => await NavigateToZikrMorningPage());
            ZikrEveningCommand = new Command(async () => await NavigateToZikrEveningPage());
            AsmaulHusnaCommand = new Command(async () => await NavigateToAsmaulHusnaPage());
            PrayCommand = new Command(async () => await NavigateToPrayPage());
            IntentionCommand = new Command(async () => await NavigateToIntentionPage());
            HadithCommand = new Command(async () => await NavigateToHadithPage());
            JuzAmmaCommand = new Command(async () => await NavigateToJuzAmmaPage());
        }

        private async Task NavigateToReadJuzPage()
        {
            await Shell.Current.GoToAsync($"{nameof(JuzsPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToReadJuzModeTabPage()
        {
            await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }
        private async Task NavigateToReadTafsirPage()
        {
            await Shell.Current.GoToAsync($"{nameof(TafsirsPage)}");
            Shell.Current.FlyoutIsPresented=false;
        }

        private async Task NavigateToQiblaPage()
        {
            await Shell.Current.GoToAsync($"{nameof(QiblaPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToZikrMorningPage()
        {
            await Shell.Current.GoToAsync($"{nameof(ZikrsPage)}?{nameof(ZikrViewModel.ZikrTime)}=0");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToZikrEveningPage()
        {
            await Shell.Current.GoToAsync($"{nameof(ZikrsPage)}?{nameof(ZikrViewModel.ZikrTime)}=1");
            Shell.Current.FlyoutIsPresented = false;
        }
        private async Task NavigateToAsmaulHusnaPage()
        {
            await Shell.Current.GoToAsync($"{nameof(AsmaulHusnaPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }
        private async Task NavigateToPrayPage()
        {
            await Shell.Current.GoToAsync($"{nameof(PraysPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }
        private async Task NavigateToIntentionPage()
        {
            //await Shell.Current.GoToAsync($"{nameof(IntentionsPage)}");
            await Shell.Current.GoToAsync($"{nameof(TabbedPageIntentionPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToHadithPage()
        {
            await Shell.Current.GoToAsync($"{nameof(HadithsPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToJuzAmmaPage()
        {
            int surahID = 78;
            int ayahID = 1;

            var surah = await SurahDataService.GetSurahAsync(surahID);
            var juzID = await JuzDataService.GetJuzIDAsync(surahID, ayahID);

            await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}?{nameof(TabbedPageJuzDetailViewModel.JuzID)}={juzID}&{nameof(TabbedPageJuzDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageJuzDetailViewModel.AyahID)}={ayahID}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToPraySchedulePage()
        {
            await Shell .Current.GoToAsync($"{nameof(PrayerSchedulePage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToBookmarkPage()
        {
            await Shell.Current.GoToAsync($"{nameof(BookmarksPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToAboutPage()
        {
            await Shell.Current.GoToAsync($"{nameof(AboutPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToSettingPage()
        {
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToFindPage()
        {
            await Shell.Current.GoToAsync($"{nameof(FindPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToHelpPage()
        {
            await Shell.Current.GoToAsync($"{nameof(HelpPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToReadQuranPage()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(SurahPage)}");
            Shell.Current.FlyoutIsPresented = false;
            CanNavigate = true;
        }

        private async Task NavigateToReadQuranModeTabPage()
        {
            await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async Task NavigateToLastReadPage()
        {
            int surahID = Preferences.Get(MenuKey.LAST_SURAH, 0);
            int ayahID = Preferences.Get(MenuKey.LAST_AYAH, 0);

            if (surahID == 0 && ayahID == 0)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, Message.MSG_NOT_YET_MARK_LAST_READ, Message.MSG_OK);
            }
            else
            {
                //await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}?{nameof(TabbedPageSurahDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageSurahDetailViewModel.AyahID)}={ayahID}");
                var surah = await SurahDataService.GetSurahAsync(surahID);
                var juzID = await JuzDataService.GetJuzIDAsync(surahID, ayahID);
                _ = await ActionHelper.OpenAyahPageFromBookmarkAsync(surahID, ayahID, juzID, surah.NameLatin);
                Shell.Current.FlyoutIsPresented = false;
            }
        }
    }
}
