using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Messages
{
    public class ErrorMessage
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public Color BackgroundColor 
        { 
            get { return Color.Pink; }
        }
        public Color TextColor
        {
            get { return Color.Red; }
        }

    }
}
