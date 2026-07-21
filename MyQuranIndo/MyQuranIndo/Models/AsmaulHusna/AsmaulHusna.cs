using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Models.AsmaulHusna
{
    public class AsmaulHusna : NotifyPropertyChanged
    {
        private int theme = Preferences.Get(Setting.THEME, 0);

        [JsonProperty("urutan")]
        public long ID { get; set; }

        [JsonProperty("arab")]
        public string Arabic { get; set; }

        [JsonProperty("latin")]
        public string ArabicLatin { get; set; }

        [JsonProperty("arti")]
        public string TranslateID { get; set; }
        public long RowID { get; set; }

        public string Title
        {
            get
            {
                return $"{ID}. {ArabicLatin} - {TranslateID}";
            }
        }
        public double FontSizeArabic
        {
            get
            {
                return FontHelper.GetFontSizeArabic();
            }
        }
        public double FontSizeTranslate
        {
            get
            {
                return FontHelper.GetFontSizeTranslate();
            }
        }

        public double FontSizeTextIndo
        {
            get
            {
                return FontHelper.GetFontSizeTextIndo();
            }
        }

        private Color rowColor;
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
