using MyQuranIndo.Controls;
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
    public partial class TabbedPageIntentionPage : BindableTabbedPage
    {
        TabbedPageIntentionViewModel _viewModel;

        public TabbedPageIntentionPage()
        {
            InitializeComponent();
            //CurrentPageChanged += CurrentPageHasChanged;
            BindingContext = _viewModel = new TabbedPageIntentionViewModel();
            BuildForm();
        }

        private async void BuildForm()
        {
            await _viewModel.Initialization;

            // assign the combobox
            // _viewMode.cbCategories.DataSource = ocCategories;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            //var surahID = (CurrentPage.BindingContext as IntentionViewModel).TabID;

            //_viewModel.SurahID = surahID;
            //_viewModel.LoadSurahDetail();
        }
    }
}