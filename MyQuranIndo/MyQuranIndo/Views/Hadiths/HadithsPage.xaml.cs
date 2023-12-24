using MyQuranIndo.ViewModels;
using MyQuranIndo.ViewModels.Hadiths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuranIndo.Views.Hadiths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HadithsPage : ContentPage, IHasCollectionView
	{
		private HadithsViewModel _viewModel;
		public CollectionView CollectionView => collHadiths;

        public HadithsPage ()
		{
			InitializeComponent ();
			BindingContext = _viewModel = new HadithsViewModel();
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
    }
}