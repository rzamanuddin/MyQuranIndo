using MyQuranIndo.Models.Qurans;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MyQuranIndo.Helpers
{
    public static class TafsirTypeHelper
    {
        public static string GetTafsirTypeName()
        {
            int tafsirType = Preferences.Get(References.Setting.TAFSIR_SELECTED, (int)TafsirType.Kemenag);
            string tafsirTypeName = "";

            switch (tafsirType)
            {
                case (int)TafsirType.Kemenag:
                    tafsirTypeName = "Kemenag";
                    break;
                case (int)TafsirType.AlJalalain:
                    tafsirTypeName = "Al-Jalalain";
                    break;
                default:
                    goto case (int)TafsirType.Kemenag;
            }
            return tafsirTypeName;
        }

        public static string GetTafsirTypeName(int tafsirType)
        {
            string tafsirTypeName = "";

            switch (tafsirType)
            {
                case (int)TafsirType.Kemenag:
                    tafsirTypeName = "Kemenag";
                    break;
                case (int)TafsirType.AlJalalain:
                    tafsirTypeName = "Al-Jalalain";
                    break;
                default:
                    goto case (int)TafsirType.Kemenag;
            }
            return tafsirTypeName;
        }

        public static int GetTafsirType()
        {
            int tafsirType = Preferences.Get(References.Setting.TAFSIR_SELECTED, (int)TafsirType.Kemenag);

            return tafsirType;
        }

    }
}
