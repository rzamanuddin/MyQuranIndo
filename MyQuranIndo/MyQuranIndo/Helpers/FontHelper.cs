using MyQuranIndo.Models.Fonts;
using MyQuranIndo.Models.Qurans;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Helpers
{
    public static class FontHelper
    {
        public static double GetFontSizeArabic(bool isForQuranOnly = false)
        {
            double multiplier = 1.25;
            double result = 0;
            int fontSize = Preferences.Get(References.Setting.FONT_SIZE_SELECTED, (int)FontSize.Small);

            switch (fontSize)
            {
                case (int)FontSize.Small:
                    result = Device.GetNamedSize(NamedSize.Title, typeof(Label));
                    break;
                case (int)FontSize.Medium:
                    result = 28;
                    break;
                case (int)FontSize.Large:
                    result = 34;
                    break;
                default:
                    goto case (int)FontSize.Small;                
            }

            if (RasmHelper.GetRasmType() == (int)RasmType.Utsmani & isForQuranOnly)
            {
                result *= multiplier;
            }
            
            return result;
        }

        public static string GetFontArabicName()
        {
            if (RasmHelper.GetRasmType() == (int)RasmType.Utsmani)
            {
                return "KFGQPC";
            }
            else
            {
                return "LPMQ";
            }
        }

        public static string GetBismillah()
        {
            if (RasmHelper.GetRasmType() == (int)RasmType.Utsmani)
            {
                return "بِسۡمِ ٱللَّهِ ٱلرَّحۡمَٰنِ ٱلرَّحِيمِ";
            }
            else
            {
                return "بِسْمِ اللّٰهِ الرَّحْمٰنِ الرَّحِيْمِ";
            }
        }
    }
}
