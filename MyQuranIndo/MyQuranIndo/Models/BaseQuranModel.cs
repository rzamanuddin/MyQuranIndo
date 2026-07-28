using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Models
{
    public class BaseQuranModel : NotifyPropertyChanged
    {
        private Color rowColor;
        private int theme = Preferences.Get(Setting.THEME, 0);
        // subscribe to theme change so we can notify UI to refresh RowColor
        public BaseQuranModel()
        {
            try
            {
                Xamarin.Forms.MessagingCenter.Subscribe<MyQuranIndo.ViewModels.Setting.SettingViewModel, int>(this, MyQuranIndo.Messages.Message.MSG_KEY_CHANGE_THEME, (sender, arg) =>
                {
                    // update local theme and notify that RowColor changed
                    theme = Preferences.Get(Setting.THEME, theme);
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(RowColor)));
                });
            }
            catch { }
        }
        public int ID { get; set; }
        public int SurahID { get; set; }

        public Color RowColor
        {
            get
            {
                // compute current row color based on current theme and resources
                try
                {
                    var selectedItem = (Color)Application.Current.Resources["SelectedItem"];
                    var rowColorResource = (Color)Application.Current.Resources["RowColor"];
                    int currentTheme = Preferences.Get(Setting.THEME, theme);

                    Color computed;
                    // Al fatihah and At Taubah
                    if (SurahID == 1 || SurahID == 9)
                    {
                        if (ID % 2 == 0)
                        {
                            computed = rowColorResource;
                        }
                        else
                        {
                            computed = currentTheme == (int)ThemeStyle.Black ? rowColorResource : Color.White;
                        }
                    }
                    else
                    {
                        if (ID % 2 == 1)
                        {
                            computed = rowColorResource;
                        }
                        else
                        {
                            computed = currentTheme == (int)ThemeStyle.Black ? rowColorResource : Color.White;
                        }
                    }

                    // update cache and return
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
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
            }
        }
    }
}
