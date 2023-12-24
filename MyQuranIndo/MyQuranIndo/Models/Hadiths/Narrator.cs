using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Hadiths
{
    public class Narrator : NotifyPropertyChanged
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("total")]
        public int TotalHadith { get; set; }

        public int ID { get; set; }

        public string Description
        {
            get => $"Total Hadis {TotalHadith}";
        }

        public string DisplaySubTitle
        {
            get
            {
                return String.Format($"H.R. {Name}, {TotalHadith} Hadis.");
            }
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
            }
            set
            {
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
            }
        }
    }
}
