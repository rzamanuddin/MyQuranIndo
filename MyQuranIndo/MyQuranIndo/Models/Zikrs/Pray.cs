using MyQuranIndo.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Zikrs
{
    public class Pray : NotifyPropertyChanged
    {
        private Color rowColor;

        [JsonProperty("id")]
        public long ID { get; set; }
        [JsonProperty("arabic")]
        public string Arabic { get; set; }
        [JsonProperty("arabiclatin")]
        public string ArabicLatin { get; set; }
        [JsonProperty("faedah")]
        public string Faedah { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("narrator")]
        public string Narrator { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("translatedid")]
        public string TranslateID { get; set; }
        [JsonProperty("tag")]
        public List<string> Tag { get; set; }

        public long RowID { get; set; }

        public string TitleAndNumber
        {
            get
            {
                if (ID > 0)
                    return $"{RowID}. {Title}";
                else
                    return Title;
            }
        }
        public double FontSizeArabic
        {
            get
            {
                return FontHelper.GetFontSizeArabic();
            }
        }
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
                        rowColor = Color.White;
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
        public bool IsVisibleArabicLatin
        {
            get
            {
                if (String.IsNullOrWhiteSpace(ArabicLatin))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool IsVisibleFaedah
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Faedah))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsVisibleTitle
        {
            get
            {
                if (ID == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsVisibleSubTitle
        {
            get
            {
                if (ID == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public TextAlignment TextAlignment
        {
            get
            {
                if (ID == 0)
                {
                    return TextAlignment.Center;
                }
                else
                {
                    return TextAlignment.Start;
                }
            }
        }
    }
}
