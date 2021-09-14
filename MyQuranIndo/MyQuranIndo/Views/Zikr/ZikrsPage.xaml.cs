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
    public partial class ZikrsPage : ContentPage
    {
        ZikrViewModel _viewModel;
        public ZikrsPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new ZikrViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}