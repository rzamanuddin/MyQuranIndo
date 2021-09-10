using MyQuranIndo.Views.Juz;
using MyQuranIndo.Views.Surah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.TabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageSurahsAndJuzsPage : Xamarin.Forms.TabbedPage
    {
        public TabbedPageSurahsAndJuzsPage()
        {
            InitializeComponent();

            //NavigationPage navigationPage = new NavigationPage(new JuzsPage());

            //Children.Add(new SurahPage());
            //Children.Add(new JuzsPage());
        }
    }
}