using MyQuranIndo.Models.Fonts;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Helpers
{
    public static class FontHelper
    {
        public static double GetFontSizeArabic()
        {            
            int fontSize = Preferences.Get(References.Setting.FONT_SIZE_SELECTED, (int)FontSize.Small);

            switch (fontSize)
            {
                case (int)FontSize.Small:
                    return Device.GetNamedSize(NamedSize.Title, typeof(Label));
                case (int)FontSize.Medium:
                    return 28;
                case (int)FontSize.Large:
                    return 32;

                default:
                    goto case (int)FontSize.Small;                
            }
        }
    }
}
