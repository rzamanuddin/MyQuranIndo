using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Qurans
{
    public class Tafsir : BaseModel
    {
        public int SurahID { get; set; }
        public string Kemenag { get; set; }
        public string AlJalalain { get; set; }

        public int TafsirID
        {
            get { return ID; }
        }

        public string TafsirText
        {
            get
            {
                var tafsirType = TafsirTypeHelper.GetTafsirType();
                switch (tafsirType)
                {
                    case (int)TafsirType.AlJalalain:
                        return AlJalalain;
                    default:
                        return Kemenag;
                }
            }
        }

        public double FontSizeTranslate
        {
            get
            {
                return FontHelper.GetFontSizeTranslate();
            }
        }
    }
}
