using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Bookmarks
{
    public class Bookmark : NotifyPropertyChanged
    {
        private Color rowColor;
        public int SurahID { get; set; }
        public string SurahNameLatin { get; set; }
        public int AyahID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Row { get; set; }

        public Color RowColor
        {
            get
            {
                if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                { 
                    if (Row % 2 == 0)
                    {
                        rowColor = ((Color)Application.Current.Resources["RowColor"]);
                    }
                    else
                    {
                        rowColor = Color.White;
                    }
                }
                return rowColor;
            }
            set
            {
                //if (value == ((Color)Application.Current.Resources["SelectedItem"])
                //    || value == ((Color)Application.Current.Resources["RowColor"]))
                //{
                    rowColor = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
                //}
            }
        }

        //private static void OnPropertyChanged(string propertyName)
        //{
        //    var changed = PropertyChanged;
        //    if (changed != null)
        //    {
        //        PropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        public string Description { 
            get
            {
                return string.Format("Q.S. {0}. {1} Ayat {2}.", SurahID, SurahNameLatin, AyahID);
            }
        }
        public override string ToString()
        {
            return Description;
        }
    }
}
