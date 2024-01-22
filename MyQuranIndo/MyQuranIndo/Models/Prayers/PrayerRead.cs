using MyQuranIndo.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Prayers
{

    public class PrayerRead : NotifyPropertyChanged
    {
        private PrayerRead PrayerReadSelf { get; set; }
        private Color rowColor;

        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("arabic")]
        public string Arabic { get; set; }
        [JsonProperty("arabiclatin")]
        public string ArabicLatin { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("translatedid")]
        public string TranslateID { get; set; }

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
        public Color RowColor
        {
            get
            {
                if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                {
                    if (RowID % 2 != 0)
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

    public class PrayerReadGroup : ObservableCollection<PrayerRead>
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
        public ObservableCollection<PrayerRead> PrayerReads { get; private set; }

        public PrayerReadGroup(int id, string titleHeader, ObservableCollection<PrayerRead> prayerReads) : base(prayerReads)
        {
            this.ID = id;
            this.TitleHeader = titleHeader;
            PrayerReads = prayerReads;
        }

        public override string ToString()
        {
            return TitleHeader;
        }
    }
}
