using MyQuranIndo.Models.Qurans;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MyQuranIndo.Helpers
{
    public class RasmHelper
    {
        public static string GetRasmTypeName()
        {
            int rasmType = Preferences.Get(References.Setting.TAFSIR_SELECTED, (int)RasmType.IndoPak);
            string rasmTypeName = "";

            switch (rasmType)
            {
                case (int)RasmType.Utsmani:
                    rasmTypeName = "Utsmani";
                    break;
                default:
                    rasmTypeName = "IndoPak";
                    break;
            }
            return rasmTypeName;
        }

        public static int GetRasmType()
        {
            int result = Preferences.Get(References.Setting.RASM_SELECTED, (int)RasmType.IndoPak);
            return result;
        }
    }
}
