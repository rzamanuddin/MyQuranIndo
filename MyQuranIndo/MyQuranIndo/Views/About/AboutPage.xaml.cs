using MyQuranIndo.ViewModels.About;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.About
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //if (!DesignMode.IsDesignModeEnabled)
            //{
            //    (BindingContext as AboutViewModel).OpenPlayStoreCommand.Execute(null);
            //}
        }
    }
}