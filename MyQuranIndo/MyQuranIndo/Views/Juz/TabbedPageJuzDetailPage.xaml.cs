using MyQuranIndo.Controls;
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
    public partial class TabbedPageJuzDetailPage : BindableTabbedPage
    {
        TabbedPageJuzDetailViewModel _viewModel;

        public TabbedPageJuzDetailPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new TabbedPageJuzDetailViewModel();
            BuildForm();
        }

        private async void BuildForm()
        {
            await _viewModel.Initialization;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var juzID = (CurrentPage.BindingContext as JuzDetailViewModel).JuzID;
            var surahID = (CurrentPage.BindingContext as JuzDetailViewModel).SurahID;

            _viewModel.JuzID = juzID;
            _viewModel.SurahID = surahID;
            _viewModel.LoadJuzDetail();
        }
    }
}