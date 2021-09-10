using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyQuranIndo.ViewModels.Prayer;

namespace MyQuranIndo.Views.Prayer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrayerSchedulePage : ContentPage
    {
        PrayerScheduleViewModel _viewModel;       

        public PrayerSchedulePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PrayerScheduleViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}