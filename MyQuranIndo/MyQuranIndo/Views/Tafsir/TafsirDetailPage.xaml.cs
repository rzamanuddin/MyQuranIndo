using MyQuranIndo.ViewModels;
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
    public partial class TafsirDetailPage : ContentPage, IHasListView
    {
        public ListView ListView => collTafsir;
        private TafsirDetailViewModel _viewModel;
        public TafsirDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new TafsirDetailViewModel();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.BindingContext is IHasListViewViewModel hasListViewViewModel)
            {
                hasListViewViewModel.View = this;
            }
            //if (this.BindingContext is IHasCollectionViewModel hasCollectionViewModel)
            //{
            //    hasCollectionViewModel.View = this;
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //if (!DesignMode.IsDesignModeEnabled)
            //{
            //    ((SurahDetailViewModel)BindingContext).OnAppearing();
            //}
            _viewModel.OnAppearing();
        }


        protected override bool OnBackButtonPressed()
        {
            _viewModel.StopPlayer();
            return base.OnBackButtonPressed();
        }
    }
}