﻿using MyQuranIndo.Models.Fonts;
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

        public static double GetFontSizeTranslate()
        {
            double result = 0;
            int fontSize = Preferences.Get(References.Setting.FONT_SIZE_TRANSLATE_SELECTED, (int)FontSize.Caption);

            switch (fontSize)
            {
                case (int)FontSize.Caption:
                    result = Device.GetNamedSize(NamedSize.Caption, typeof(Label));
                    break;
                case (int)FontSize.Small:
                    result = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                    break;
                case (int)FontSize.Medium:
                    result = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    break;
                case (int)FontSize.Large:
                    result = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    break;
                default:
                    goto case (int)FontSize.Caption;
            }

            return result;
        }

        public static double GetFontSizeTextIndo()
        {
            double result = 0;
            int fontSize = Preferences.Get(References.Setting.FONT_SIZE_TEXT_INDO_SELECTED, (int)FontSize.Caption);

            switch (fontSize)
            {
                case (int)FontSize.Caption:
                    result = Device.GetNamedSize(NamedSize.Caption, typeof(Label));
                    break;
                case (int)FontSize.Small:
                    result = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                    break;
                case (int)FontSize.Medium:
                    result = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    break;
                case (int)FontSize.Large:
                    result = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    break;
                default:
                    goto case (int)FontSize.Caption;
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

        public static string GetBismillahTranslate()
        {
            return "Dengan nama Allah Yang Maha Pengasih, Maha Penyayang.";
        }

        public static string GetBismillahTextIndo()
        {
            return "bismillāhir-raḥmānir-raḥīm";
        }
    }
}
