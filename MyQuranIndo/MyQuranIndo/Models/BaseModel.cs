using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Resource;
using Color = Xamarin.Forms.Color;

namespace MyQuranIndo.Models
{
    public class BaseModel : NotifyPropertyChanged
    {
        private int theme = Preferences.Get(Setting.THEME, 0);
        private Color rowColor;

        [JsonProperty("id")]
        public int ID { get; set; }

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
