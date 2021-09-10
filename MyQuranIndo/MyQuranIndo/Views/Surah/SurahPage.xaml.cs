using MyQuranIndo.Messages;
using MyQuranIndo.ViewModels;
using MyQuranIndo.ViewModels.Surah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Surah
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurahPage : ContentPage, IHasCollectionView
    {
        SurahsViewModel _viewModel;
        public CollectionView CollectionView => collSurah;

        public SurahPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SurahsViewModel();
            //BuildForm();
            //MessagingCenter.Subscribe<SurahViewModel, string>(this, Message.KEY_ERROR_GET, async (sender, args) =>
            //{
            //    await DisplayAlert("Error", args, "Ok");
            //});
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
        protected override bool OnBackButtonPressed()
        {
            _viewModel.StopPlayer();
            return base.OnBackButtonPressed();
        }

    }
}