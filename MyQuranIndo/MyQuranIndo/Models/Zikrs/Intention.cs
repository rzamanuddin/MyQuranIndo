using MyQuranIndo.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Zikrs
{
    public class Intention : NotifyPropertyChanged
    {
        private Intention IntentionSelf { get; set; }
        private Color rowColor;

        [JsonProperty("id")]
        public int ID { get; set; }
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
        
        //[JsonProperty("tag")]
        //public List<string> Tag { get; set; }

        public long RowID { get; set; }

        public string TitleAndNumber
        {
            get
            {
                if (ID > 0)
                    return $"{ID}. {Title}";
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

    public class IntentionGroup : ObservableCollection<Intention>
    {
        private bool isExpand = false;
        private string imageHeader = "expand.png";

        public int ID { get; set; }
        public string TitleHeader { get; set; }
        //public int NumberOfAyah { get; set; }

        public bool IsExpand
        {
            get
            {
                return isExpand;
            }
            set
            {
                isExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpand)));
            }
        }

        public string ImageHeader
        {
            get
            {
                return imageHeader;
            }
            set
            {
                imageHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ImageHeader)));
            }
        }

        public string FoundResult
        {
            get
            {
                //return String.Format("{0} ({1} ayat)", SurahName, NumberOfAyah);
                return $"{TitleHeader}";
            }
        }
        public ObservableCollection<Intention> Intentions { get; private set; }

        public IntentionGroup(int id, string titleHeader, ObservableCollection<Intention> intentions) : base(intentions)
        {
            this.ID = id;
            this.TitleHeader = titleHeader;
            //this.NumberOfAyah = numberOfAyah;
            Intentions = intentions;
        }

        public override string ToString()
        {
            return TitleHeader;
        }
    }
}
