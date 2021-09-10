using MyQuranIndo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Selectors
{
    public class AlternateColorDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EvenTemplate { get; set; }
        public DataTemplate UnevenTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            ListView lstView = container as ListView;
            if (lstView != null)
            {
                IList listItem = lstView.ItemsSource as IList;
                int idx = listItem.IndexOf(item);

                return idx % 2 == 0 ? EvenTemplate : UnevenTemplate;
            }
            else
            {
                return UnevenTemplate;
            }
        }
    }
}
