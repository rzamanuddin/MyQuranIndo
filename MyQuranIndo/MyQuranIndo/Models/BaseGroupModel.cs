using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Models
{
    public class BaseGroupModel : NotifyPropertyChanged
    {

        private int theme = Preferences.Get(Setting.THEME, 0);
        private Color rowColor;

        public int RowID { get; set; }
        public Color RowColor
        {
            get
            {
                if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                {
                    if (RowID % 2 == 0)
                    {
                        rowColor = ((Color)Application.Current.Resources["RowColor"]);
                    }
                    else
                    {
                        if (theme == (int)ThemeStyle.Black)
                        {
                            rowColor = ((Color)Application.Current.Resources["RowColor"]);
                        }
                        else
                        {
                            rowColor = Color.White;
                        }
                    }
                }
                return rowColor;
            }
            set
            {
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
            }
        }
    }
}
