using MyQuranIndo.Models;
using MyQuranIndo.ViewModels.Find;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Finds
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FindPage : ContentPage
    {
        FindViewModel _viewModel;

        public FindPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new FindViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}