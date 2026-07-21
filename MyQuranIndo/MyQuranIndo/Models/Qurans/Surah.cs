using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Qurans
{
    public class Surah : NotifyPropertyChanged
    {
        public static int SURAH_COUNT = 114;
        int theme = Preferences.Get(Setting.THEME, 0);

        private Color rowColor;
        private Color rowColorPopular;
        public int ID { get; set; }
        public int SurahPopularID { get; set; }
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

        public Color RowColorPopular
        {
            get
            {
                if (rowColorPopular != ((Color)Application.Current.Resources["SelectedItem"]))
                {
                    if (SurahPopularID % 2 == 0)
                    {
                        rowColorPopular = ((Color)Application.Current.Resources["RowColor"]);
                    }
                    else
                    {
                        if (theme == (int)ThemeStyle.Black)
                        {
                            rowColorPopular = ((Color)Application.Current.Resources["RowColor"]);
                        }
                        else
                        {
                            rowColorPopular = Color.White;
                        }
                    }
                }
                return rowColorPopular;
            }
            set
            {
                rowColorPopular = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColorPopular)));
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
