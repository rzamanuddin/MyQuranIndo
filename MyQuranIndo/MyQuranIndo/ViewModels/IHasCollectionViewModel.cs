using MyQuranIndo.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyQuranIndo.ViewModels
{
    public interface IHasCollectionViewModel
    {
        IHasCollectionView View { get; set; }
    }
}
