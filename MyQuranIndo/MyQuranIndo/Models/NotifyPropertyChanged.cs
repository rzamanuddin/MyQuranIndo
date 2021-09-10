using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyQuranIndo.Models
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(null, propertyChangedEventArgs);
            }
        }
    }
}
