using MyQuranIndo.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Zikrs
{
    public class ListOfZikr
    {
        public List<Zikr> data { get; set; }
    }
    public class Zikr : NotifyPropertyChanged
    {
        private Color rowColor;

        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("arabic")]
        public string Arabic { get; set; }

        [JsonProperty("arabic_latin")]
        public string ArabicLatin { get; set; }

        [JsonProperty("faedah")]
        public string Faedah { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("narrator")]
        public string Narrator { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("translated_id")]
        public string TranslateID { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

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
            }
            set
            {
                //if (value == ((Color)Application.Current.Resources["SelectedItem"]))
                //{
                //    rowColor = value;
                //    OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
                //}
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
    }
}
