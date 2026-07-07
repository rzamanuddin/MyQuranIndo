using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Donate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DonatePage : ContentPage
    {
        public DonatePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //if (!DesignMode.IsDesignModeEnabled)
            //{
            //    (BindingContext as AboutViewModel).OpenPlayStoreCommand.Execute(null);
            //}
        }
    }
}