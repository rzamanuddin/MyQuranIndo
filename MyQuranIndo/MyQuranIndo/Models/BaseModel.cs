using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Resource;
using Color = Xamarin.Forms.Color;

namespace MyQuranIndo.Models
{
    public class BaseModel : NotifyPropertyChanged
    {
        private int theme = Preferences.Get(Setting.THEME, 0);
        private Color rowColor;

        // subscribe to theme change so UI can refresh row colors
        public BaseModel()
        {
            try
            {
                Xamarin.Forms.MessagingCenter.Subscribe<MyQuranIndo.ViewModels.Setting.SettingViewModel, int>(this, MyQuranIndo.Messages.Message.MSG_KEY_CHANGE_THEME, (sender, arg) =>
                {
                    theme = Preferences.Get(Setting.THEME, theme);
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
                });
            }
            catch { }
        }

        [JsonProperty("id")]
        public int ID { get; set; }

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
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
            }
        }
    }
}
