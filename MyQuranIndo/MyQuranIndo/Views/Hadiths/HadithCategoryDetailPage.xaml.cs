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
    public partial class HadithCategoryDetailPage : ContentPage, IHasListView
    {
        public ListView ListView => collHadith;

        private HadithCategoryDetailViewModel _viewModel;

        public HadithCategoryDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HadithCategoryDetailViewModel();
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

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}