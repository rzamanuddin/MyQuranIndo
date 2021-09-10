using MyQuranIndo.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Qurans
{
    public class Surah : NotifyPropertyChanged
    {
        private Color rowColor;
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameLatin { get; set; }
        public string TranslateIndo { get; set; }
        public int NumberOfAyah { get; set; }

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

        private bool isVisibleProgressBar = false;
        public bool IsVisibleProgressBar
        {
            get
            {
                return isVisibleProgressBar;
            }
            set
            {
                isVisibleProgressBar = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsVisibleProgressBar)));
            }
        }

        private double progressValue = 0;
        public double ProgressValue
        {
            get
            {
                return progressValue;
            }
            set
            {
                progressValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ProgressValue)));
            }
        }

        private string statusTextProgressBar = "";
        public string StatusTextProgressBar
        {
            get
            {
                return statusTextProgressBar;
            }
            set
            {
                statusTextProgressBar = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(StatusTextProgressBar)));
            }
        }

        public string DisplayNameLatin
        {
            get
            {
                return String.Format("{0}. {1} ({2}), {3} Ayat.", ID, NameLatin, TranslateIndo, NumberOfAyah);
            }
        }
        public string DisplayNameArab
        {
            get
            {
                return String.Format("{0} - {1}", ID, Name);
            }
        }

        public string DisplayIDNameLatin
        {
            get
            {
                return String.Format($"{ID}. {NameLatin}");
            }
        }
        public double FontSizeArabic
        {
            get
            {
                return FontHelper.GetFontSizeArabic();
            }
        }
    }
}
