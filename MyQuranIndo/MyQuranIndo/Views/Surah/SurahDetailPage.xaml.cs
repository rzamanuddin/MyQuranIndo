using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyQuranIndo.Models;
using MyQuranIndo.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using MyQuranIndo.ViewModels.Surah;

namespace MyQuranIndo.Views.Surah
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurahDetailPage : ContentPage, IHasListView
    {
        //public CollectionView CollectionView => collAyah;

        public ListView ListView => collAyah;

        private SurahDetailViewModel _viewModel;

        public SurahDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SurahDetailViewModel();
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

        private async void OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    await DisplayAlert("Info", "Swipe Success", "Ok");
                    break;
                default:
                    break;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            _viewModel.StopPlayer();
            return base.OnBackButtonPressed();
        }
    }
}