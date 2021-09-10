using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Controls
{
    public class CustomTableView : TableView
    {
        public static BindableProperty SeparatorColorProperty = BindableProperty.Create("SeparatorColor", typeof(Color), typeof(CustomTableView)
            , Color.White);
        public Color SeparatorColor
        {
            get
            {
                return (Color)GetValue(SeparatorColorProperty);
            }
            set
            {
                SetValue(SeparatorColorProperty, value);
            }
        }
        public CustomTableView()
        {
            //InitializeComponent();
        }
    }
}
