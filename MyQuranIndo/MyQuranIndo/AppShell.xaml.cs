using MyQuranIndo.ViewModels;
using MyQuranIndo.Views;
using MyQuranIndo.Views.About;
using MyQuranIndo.Views.AsmaulHusna;
using MyQuranIndo.Views.Bookmarks;
using MyQuranIndo.Views.Finds;
using MyQuranIndo.Views.Help;
using MyQuranIndo.Views.Home;
using MyQuranIndo.Views.Juz;
using MyQuranIndo.Views.Prayer;
using MyQuranIndo.Views.Qibla;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Views.Surah;
using MyQuranIndo.Views.TabbedPage;
using MyQuranIndo.Views.Tafsir;
using MyQuranIndo.Views.Zikr;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyQuranIndo
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SurahPage), typeof(SurahPage));
            Routing.RegisterRoute(nameof(SurahDetailPage), typeof(SurahDetailPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(FindPage), typeof(FindPage));
            Routing.RegisterRoute(nameof(TabbedPageSurahDetailPage), typeof(TabbedPageSurahDetailPage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(HelpPage), typeof(HelpPage));
            Routing.RegisterRoute(nameof(BookmarksPage), typeof(BookmarksPage));
            Routing.RegisterRoute(nameof(PrayerSchedulePage), typeof(PrayerSchedulePage));
            Routing.RegisterRoute(nameof(QiblaPage), typeof(QiblaPage));
            Routing.RegisterRoute(nameof(SearchModalPage), typeof(SearchModalPage));
            Routing.RegisterRoute(nameof(JuzsPage), typeof(JuzsPage));
            Routing.RegisterRoute(nameof(JuzDetailPage), typeof(JuzDetailPage));
            Routing.RegisterRoute(nameof(TabbedPageJuzDetailPage), typeof(TabbedPageJuzDetailPage));
            Routing.RegisterRoute(nameof(TabbedPageSurahsAndJuzsPage), typeof(TabbedPageSurahsAndJuzsPage));
            //Routing.RegisterRoute(nameof(ZikrMorningPage), typeof(ZikrMorningPage));
            Routing.RegisterRoute(nameof(ZikrsPage), typeof(ZikrsPage));
            Routing.RegisterRoute(nameof(AsmaulHusnaPage), typeof(AsmaulHusnaPage));
            Routing.RegisterRoute(nameof(PraysPage), typeof(PraysPage));
            Routing.RegisterRoute(nameof(IntentionsPage), typeof(IntentionsPage));
            Routing.RegisterRoute(nameof(TafsirsPage), typeof(TafsirsPage));
            Routing.RegisterRoute(nameof(TafsirDetailPage), typeof(TafsirDetailPage));
            //Routing.RegisterRoute(nameof(PraysPage), typeof(PraysPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
