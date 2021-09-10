using MyQuranIndo.ViewModels.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        private SettingViewModel _viewModel;
        public SettingPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SettingViewModel();
        }

        protected async override void OnBindingContextChanged()
        {
            if (BindingContext is SettingViewModel)
            {
                await _viewModel.LoadDataAsync();
            }
            base.OnBindingContextChanged();
        }
        //private async void BuildForm()
        //{
        //    await _viewModel.Initialization;

        //    // assign the combobox
        //    // _viewMode.cbCategories.DataSource = ocCategories;
        //}
    }
}