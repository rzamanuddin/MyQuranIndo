using MyQuranIndo.Controls;
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
    public partial class TabbedPageSurahDetailPage : BindableTabbedPage
    {
        TabbedPageSurahDetailViewModel _viewModel;

        public TabbedPageSurahDetailPage()
        {
            InitializeComponent();
            //CurrentPageChanged += CurrentPageHasChanged;
            BindingContext = _viewModel = new TabbedPageSurahDetailViewModel();
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
            var surahID = (CurrentPage.BindingContext as SurahDetailViewModel).SurahID;

            _viewModel.SurahID = surahID;
            _viewModel.LoadSurahDetail();
        }
        //private void CurrentPageHasChanged(object sender, EventArgs e)
        //{
            //var tabbedPage = (TabbedPage)sender;
            //var surahID = (tabbedPage.CurrentPage.BindingContext as SurahDetailViewModel).SurahID;

            //_viewModel.SurahID = surahID;
            //_viewModel.LoadSurahDetail();
            //if (tabbedPage.CurrentPage.Title.Contains("."))
            //{
                //int surahID = Convert.ToInt32(tabbedPage.CurrentPage.Title.Split('.')[0]);
                //_viewModel.LoadSurahDetail(surahID);
                //if (surahID.ToString() != _viewModel.SurahID)
                //{
                //    _viewModel.SurahID = (surahID).ToString();
                //}
            //}
            //else
            //{
            //    _viewModel.LoadSurahDetail(1);
            //}
        //}
    }
}