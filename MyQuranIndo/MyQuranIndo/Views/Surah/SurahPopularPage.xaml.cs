using MyQuranIndo.ViewModels;
using MyQuranIndo.ViewModels.Surah;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Surah
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurahPopularPage : ContentPage, IHasCollectionView
    {
        SurahPopularsViewModel _viewModel;
        public CollectionView CollectionView => collSurah;
        public SurahPopularPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SurahPopularsViewModel();
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