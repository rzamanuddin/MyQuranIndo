using MyQuranIndo.ViewModels;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.ViewModels.Tafsir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Tafsir
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TafsirsPage : ContentPage, IHasCollectionView
    {
        TafsirsViewModel _viewModel;
        public CollectionView CollectionView => collSurah;

        public TafsirsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new TafsirsViewModel();
        }

        protected override void OnBindingContextChanged()
        {
            if (this.BindingContext is IHasCollectionViewModel hasCollectionViewModel)
            {
                hasCollectionViewModel.View = this;
            }
            base.OnBindingContextChanged();
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