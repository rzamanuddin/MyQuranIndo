using MyQuranIndo.ViewModels;
using MyQuranIndo.ViewModels.Juz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Juz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JuzsPage : ContentPage, IHasCollectionView
    {
        private JuzsViewModel _viewModel;
        public CollectionView CollectionView => collJuzs;

        public JuzsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new JuzsViewModel();
            //BuildForm();
        }

        private async void BuildForm()
        {
            await _viewModel.Initialization;
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