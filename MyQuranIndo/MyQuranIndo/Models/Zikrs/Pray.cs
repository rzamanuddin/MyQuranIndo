using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Themes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Zikrs
{
    public class PrayData
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("data")]
        public List<Pray> Data { get; set; }
    }

    public class Pray : BaseGroupModel
    {
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
        [JsonProperty("tag")]
        public List<string> Tag { get; set; }

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

    public class PrayGroup : ObservableCollection<Pray>
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
        public ObservableCollection<Pray> Prays { get; private set; }

        public PrayGroup(int id, string titleHeader, ObservableCollection<Pray> prays) : base(prays)
        {
            if (Preferences.Get(MyQuranIndo.References.Setting.THEME, 0) == (int)ThemeStyle.Black)
            {
                this.imageHeader = "expand_black.png";
            }
            else
            {
                this.imageHeader = "expand.png";
            }
            this.ID = id;
            this.TitleHeader = titleHeader;
            //this.NumberOfAyah = numberOfAyah;
            Prays = prays;
        }

        public override string ToString()
        {
            return TitleHeader;
        }
    }
}
