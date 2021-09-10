using MyQuranIndo.Configuration;
using MyQuranIndo.Models.Qurans;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MyQuranIndo.Helpers
{
    public static class ReciterHelper
    {
        public static string GetReciterName()
        {
            int reciter = Preferences.Get(References.Setting.RECITER_SELECTED, (int)Reciter.AsSudais);
            string reciterName = "";

            switch (reciter)
            {
                case (int)Reciter.AsSudais:
                    reciterName = "Abdurrahman As-Sudais";
                    break;
                case (int)Reciter.AlAfasy:
                    reciterName = "Mishary Rashid Al-Afasy";
                    break;
                case (int)Reciter.AlMatroud:
                    reciterName = "Abdullah Al-Mathrud";
                    break;
                default:
                    goto case (int)Reciter.AsSudais;
            }
            return reciterName;
        }

        public static int GetReciter()
        {
            int reciter = Preferences.Get(References.Setting.RECITER_SELECTED, (int)Reciter.AsSudais);

            return reciter;
        }
        public static string GetReciterUrl()
        {
            int reciter = Preferences.Get(References.Setting.RECITER_SELECTED, (int)Reciter.AsSudais);

            switch (reciter)
            {
                case (int)Reciter.AsSudais:
                    return AppSetting.GetUrlMP3();
                case (int)Reciter.AlAfasy:
                    return AppSetting.GetUrlMP3AlAfasy();
                case (int) Reciter.AlMatroud:
                    return AppSetting.GetUrlMP3AlMatroud();
                default:
                    goto case (int)Reciter.AsSudais;
            }
        }
    }
}
