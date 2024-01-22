using MyQuranIndo.ViewModels;
using MyQuranIndo.ViewModels.Hadiths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Hadiths
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HadithCategoriesPage : ContentPage
    {
        private HadithCategoriesViewModel _viewModel;

        public HadithCategoriesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HadithCategoriesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

    }
}