using MyQuranIndo.ViewModels.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Help
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPage : ContentPage
    {
        HelpViewModel _viewModel;
        public HelpPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new HelpViewModel();
        }
    }
}