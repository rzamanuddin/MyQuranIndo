using MyQuranIndo.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Qurans
{
    public class Tafsir : NotifyPropertyChanged
    {
        public int ID { get; set; }
        public int SurahID { get; set; }
        public string Kemenag { get; set; }
        public string AlJalalain { get; set; }

        public int TafsirID
        {
            get { return ID; }
        }

        private Color rowColor;
        public Color RowColor
        {
            get
            {
                if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                {
                    if (ID % 2 == 0)
                    {
                        rowColor = ((Color)Application.Current.Resources["RowColor"]);
                    }
                    else
                    {
                        rowColor = Color.White;
                    }
                }
                return rowColor;

                //// Al fatihah and At Taubah
                //if (SurahID == 1 || SurahID == 9)
                //{
                //    if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                //    {
                //        if (ID % 2 == 0)
                //        {
                //            rowColor = ((Color)Application.Current.Resources["RowColor"]);
                //        }
                //        else
                //        {
                //            rowColor = Color.White;
                //        }
                //    }
                //    return rowColor;
                //}
                //else
                //{
                //    if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                //    {
                //        if (ID % 2 == 1)
                //        {
                //            rowColor = ((Color)Application.Current.Resources["RowColor"]);
                //        }
                //        else
                //        {
                //            rowColor = Color.White;
                //        }
                //    }
                //    return rowColor;
                //}
            }
            set
            {
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
            }
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
    }
}
