using MyQuranIndo.ViewModels.Qibla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Qibla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QiblaPage : ContentPage
    {
        public QiblaPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!DesignMode.IsDesignModeEnabled)
            {
                ((QiblaViewModel)BindingContext).OnAppearing();
                ((QiblaViewModel)BindingContext).StartCommand.Execute(null);
                ((QiblaViewModel)BindingContext).LoadCommand.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (!DesignMode.IsDesignModeEnabled)
                ((QiblaViewModel)BindingContext).StopCommand.Execute(null);
        }
    }
}