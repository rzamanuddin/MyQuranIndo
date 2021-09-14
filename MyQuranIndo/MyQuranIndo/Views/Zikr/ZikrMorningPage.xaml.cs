using MyQuranIndo.ViewModels.Zikr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Zikr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZikrMorningPage : ContentPage
    {
        ZikrMorningViewModel _viewModel;

        public ZikrMorningPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new ZikrMorningViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}