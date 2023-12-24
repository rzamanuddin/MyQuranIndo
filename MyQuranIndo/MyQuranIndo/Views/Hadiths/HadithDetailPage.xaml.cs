using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyQuranIndo.ViewModels.Hadiths;

namespace MyQuranIndo.Views.Hadiths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HadithDetailPage : ContentPage, IHasListView
	{
        public ListView ListView => lstViewHadith;

        private HadithDetailViewModel _viewModel;

        public HadithDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HadithDetailViewModel();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.BindingContext is IHasListViewViewModel hasListViewViewModel)
            {
                hasListViewViewModel.View = this;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}