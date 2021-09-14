using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using MyQuranIndo.Services;
using MyQuranIndo.Themes;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Home;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo
{
    public partial class App : Application
    {
        //public Color OrangeColor = Color.FromHex("#ee4d2d");
        //public Color AccentColor = Color.FromHex("#F49581");

        public App()
        {
            InitializeComponent();

            //MediaManager.CrossMediaManager.Current.Init();
            

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                int theme = Preferences.Get(Setting.THEME, 0);
                if (theme == 0)
                {
                    mergedDictionaries.Add(new BlueTheme());
                }
                else
                {
                    if (theme == (int)ThemeStyle.Blue)
                    {
                        mergedDictionaries.Add(new BlueTheme());
                    }
                    else if (theme == (int)ThemeStyle.Green)
                    {
                        mergedDictionaries.Add(new GreenTheme());
                    }
                    else if (theme == (int)ThemeStyle.Orange)
                    {
                        mergedDictionaries.Add(new OrangeTheme());
                    }
                    else if (theme == (int)ThemeStyle.Black)
                    {
                        mergedDictionaries.Add(new BlackTheme());
                    }
                    else if (theme == (int)ThemeStyle.Purple)
                    {
                        mergedDictionaries.Add(new PurpleTheme());
                    }
                    else if (theme == (int)ThemeStyle.Pink)
                    {
                        mergedDictionaries.Add(new PinkTheme());
                    }
                    else if (theme == (int)ThemeStyle.Red)
                    {
                        mergedDictionaries.Add(new RedTheme());
                    }
                    else if (theme == (int)ThemeStyle.DarkBlue)
                    {
                        mergedDictionaries.Add(new DarkBlueTheme());
                    }
                    else
                    {
                        mergedDictionaries.Add(new BlueTheme());
                    }
                }
            }

            //Resources = new ResourceDictionary();
            //Resources["Primary"] = Color.FromHex("#075e55");
            //Resources["Accent"] = Color.FromHex("#09d260");
            //Resources["SelectedItem"] = Color.FromHex("#09d260");

            //            Primary Blue #2196F3
            //Accent BLue #96d1ff

            //Primary Green #2196F3
            // Primary Green WA #075e55
            //Accent BLue #34bc3d #09d260

            //Primary Orange #ee4d2d
            //Accent Orange #F49581

            DependencyService.Register<SurahDataService>();
            DependencyService.Register<PrayerScheduleDataService>();
            DependencyService.Register<JuzDataService>();
            DependencyService.Register<AyahDataServices>();
            DependencyService.Register<TafsirDataService>();            
            DependencyService.Register<MP3Service>();
            DependencyService.Register<ZikrDataService>();

            // Sharpnado.HorizontalListView.Initializer.Initialize(true, false);
            MainPage = new AppShell();

            //MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
