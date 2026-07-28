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
        // subscribe to theme change so UI can refresh row colors
        public Surah()
        {
            try
            {
                Xamarin.Forms.MessagingCenter.Subscribe<MyQuranIndo.ViewModels.Setting.SettingViewModel, int>(this, MyQuranIndo.Messages.Message.MSG_KEY_CHANGE_THEME, (sender, arg) =>
                {
                    theme = Preferences.Get(Setting.THEME, theme);
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColorPopular)));
                });
            }
            catch { }
        }
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
                try
                {
                    var rowColorResource = (Color)Application.Current.Resources["RowColor"];
                    int currentTheme = Preferences.Get(Setting.THEME, theme);

                    Color computed;
                    if (ID % 2 == 0)
                    {
                        computed = rowColorResource;
                    }
                    else
                    {
                        computed = currentTheme == (int)ThemeStyle.Black ? rowColorResource : Color.White;
                    }

                    rowColor = computed;
                    return rowColor;
                }
                catch
                {
                    return rowColor;
                }
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
                try
                {
                    var rowColorResource = (Color)Application.Current.Resources["RowColor"];
                    int currentTheme = Preferences.Get(Setting.THEME, theme);

                    Color computed;
                    if (SurahPopularID % 2 == 0)
                    {
                        computed = rowColorResource;
                    }
                    else
                    {
                        computed = currentTheme == (int)ThemeStyle.Black ? rowColorResource : Color.White;
                    }

                    rowColorPopular = computed;
                    return rowColorPopular;
                }
                catch
                {
                    return rowColorPopular;
                }
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
