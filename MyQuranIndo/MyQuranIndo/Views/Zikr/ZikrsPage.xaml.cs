﻿using MyQuranIndo.ViewModels;
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
    public partial class ZikrsPage : ContentPage, IHasCollectionView
    {
        private ZikrViewModel _viewModel;
        public CollectionView CollectionView => collZikrs;

        public ZikrsPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new ZikrViewModel();
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