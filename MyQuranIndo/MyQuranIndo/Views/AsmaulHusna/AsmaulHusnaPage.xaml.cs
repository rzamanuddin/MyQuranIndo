using MyQuranIndo.ViewModels;
using MyQuranIndo.ViewModels.AsmaulHusna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.AsmaulHusna
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AsmaulHusnaPage : ContentPage, IHasCollectionView
    {
        private AsmaulHusnaViewModel _viewModel;
        public CollectionView CollectionView => collAsmaulHusnas;


        public AsmaulHusnaPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new AsmaulHusnaViewModel();
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