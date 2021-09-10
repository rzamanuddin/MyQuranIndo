using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Controls
{
    public class BindableTabbedPage : TabbedPage
    {
        public BindableTabbedPage()
        {
            
        }

        public static BindableProperty ChildrenListProperty = BindableProperty.Create<BindableTabbedPage, IList>(o => o.ChildrenList, new List<Page>(), propertyChanged: UpdateList);

        public IList ChildrenList
        {
            get { return (IList)GetValue(ChildrenListProperty); }
            set { SetValue(ChildrenListProperty, value); }
        }

        private static void UpdateList(BindableObject bindable, IList oldValue, IList newValue)
        {
            var tabbedPage = bindable as BindableTabbedPage;
            if (tabbedPage != null)
            {
                tabbedPage.Children.Clear();
                foreach (var page in newValue)
                {
                    tabbedPage.Children.Add((Page)page);
                }                
            }
        }

    }
}
