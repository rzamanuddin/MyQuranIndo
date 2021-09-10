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
    public partial class JuzDetailPage : ContentPage, IHasListView
    {
        public ListView ListView => collJuzDetail;

        private JuzDetailViewModel _viewModel;

        public JuzDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new JuzDetailViewModel();
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
            _viewModel.StopPlayer();
            return base.OnBackButtonPressed();
        }
    }
}