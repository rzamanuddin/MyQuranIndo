using MyQuranIndo.ViewModels;
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
    public partial class PraysPage : ContentPage, IHasCollectionView
    {
        private PrayViewModel _viewModel;
        public CollectionView CollectionView => collPrays;

        public PraysPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new PrayViewModel();
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
    }
}