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
    public partial class IntentionsPage : ContentPage
    {
        private IntentionViewModel _viewModel;
        public IntentionsPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new IntentionViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}