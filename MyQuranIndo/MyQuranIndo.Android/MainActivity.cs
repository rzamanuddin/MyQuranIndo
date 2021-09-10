using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Essentials;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using MyQuranIndo.Themes;
using Android.Content.Res;
using MyQuranIndo.ViewModels.Setting;
using MyQuranIndo.Models.Themes;

namespace MyQuranIndo.Droid
{
    [Activity(Label = "MyQuranIndo", Icon = "@drawable/icon_main", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {

            global::Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental", "Expander_Experimental");
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            //// Add this to support tab right to left
            //Window.DecorView.LayoutDirection = LayoutDirection.Rtl;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            
            LoadApplication(new App());

            ICollection<ResourceDictionary> mergedDictionaries = App.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null && mergedDictionaries.Count > 0)
            {
                try
                {
                    var primary = mergedDictionaries.FirstOrDefault()["Primary"];
                    var colorPrimary = (Color)primary;
                    UpdateColor(colorPrimary.ToHex());
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                }
            }

            MessagingCenter.Subscribe<SettingViewModel, int>(this,
                 MyQuranIndo.Messages.Message.MSG_KEY_CHANGE_THEME,
                 (sender, args) =>
                 {
                     var primary = mergedDictionaries.FirstOrDefault()["Primary"];
                     var colorPrimary = (Color)primary;
                     UpdateColor(colorPrimary.ToHex());
                 });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void UpdateColor(string hex)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            }
            var color = Android.Graphics.Color.ParseColor(hex);
            var system = color.ToSystemColor();
            var system2 = DarkerColor(system);
            var fin = system2.ToPlatformColor();
            Window.SetStatusBarColor(fin);

        }

        public System.Drawing.Color DarkerColor(System.Drawing.Color color, float correctionfactory = 50f)
        {
            const float hundredpercent = 100f;
            return System.Drawing.Color.FromArgb((int)(((float)color.R / hundredpercent) * correctionfactory),
                (int)(((float)color.G / hundredpercent) * correctionfactory), (int)(((float)color.B / hundredpercent) * correctionfactory));
        }
    }
}