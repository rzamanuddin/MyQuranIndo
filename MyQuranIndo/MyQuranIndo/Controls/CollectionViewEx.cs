using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Controls
{
    public class CollectionViewEx : CollectionView
    {
        public static BindableProperty ScrollToItemWithConfigProperty = BindableProperty.Create(nameof(ScrollToItemWithConfig), typeof(IConfigurableScrollItem), typeof(CollectionViewEx), default(IConfigurableScrollItem), BindingMode.Default, propertyChanged: OnScrollToItemWithConfigPropertyChanged);

        public IConfigurableScrollItem ScrollToItemWithConfig
        {
            get => (IConfigurableScrollItem)GetValue(ScrollToItemWithConfigProperty);
            set => SetValue(ScrollToItemWithConfigProperty, value);
        }

        private static void OnScrollToItemWithConfigPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;

            if (bindable is CollectionViewEx current)
            {
                if (newValue is IGroupScrollItem scrollToItemWithGroup)
                {
                    if (scrollToItemWithGroup.Config == null)
                        scrollToItemWithGroup.Config = new ScrollToConfiguration();

                        current.ScrollTo(scrollToItemWithGroup, scrollToItemWithGroup.GroupValue, scrollToItemWithGroup.Config.ScrollToPosition, scrollToItemWithGroup.Config.Animated);

                }
                else if (newValue is IScrollItem scrollToItem)
                {
                    if (scrollToItem.Config == null)
                        scrollToItem.Config = new ScrollToConfiguration();

                        current.ScrollTo(scrollToItem, null, scrollToItem.Config.ScrollToPosition, scrollToItem.Config.Animated);
                }

            }
        }

    }
}
