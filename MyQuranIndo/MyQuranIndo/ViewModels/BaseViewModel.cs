using Javax.Sql;
using MyQuranIndo.Models;
using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using MyQuranIndo.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ISurahDataService SurahDataService => DependencyService.Get<ISurahDataService>();
        public IPrayerScheduleDataService PrayerDataService => DependencyService.Get<IPrayerScheduleDataService>();
        public IJuzDataService JuzDataService => DependencyService.Get<IJuzDataService>();
        public IAyahDataService AyahDataService => DependencyService.Get<IAyahDataService>();
        public ITafsirDataService TafsirDataService => DependencyService.Get<ITafsirDataService>();
        public IToastService ToastService => DependencyService.Get<IToastService>();
        // SINGLETON: Menggunakan MP3Service.Instance alih-alih DependencyService.Get
        // untuk memastikan semua ViewModel berbagi satu instance yang sama.
        // DependencyService.Get<IMP3Service>() membuat instance baru setiap kali,
        // yang menyebabkan ghost playback karena handler dari instance lama masih aktif.
        public IMP3Service MP3Service => Services.MP3Service.Instance;
        public IZikrDataService ZikrDataService => DependencyService.Get<IZikrDataService>();
        public IAsmaulHusnaDataService AsmaulHusnaDataService => DependencyService.Get<IAsmaulHusnaDataService>();
        public IPrayDataService PrayDataService => DependencyService.Get<IPrayDataService>();
        public IIntentionDataService IntentionDataService => DependencyService.Get<IIntentionDataService>();
        public IHelpDataService HelpDataService => DependencyService.Get<IHelpDataService>();
        public ITafsirDataService TafsirData => DependencyService.Get<ITafsirDataService>();
        public IHadithDataService HadithDataService => DependencyService.Get<IHadithDataService>();
        public IPrayerReadDataService PrayerReadDataService => DependencyService.Get<IPrayerReadDataService>();

        //public IColorThemeService ColorThemeService => DependencyService.Get<IColorThemeService>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }


        private bool _canNavigate = true;
        public bool CanNavigate
        {
            get
            {
                return _canNavigate;
            }
            set
            {
                //_canNavigate = value;
                //OnPropertyChanged();
                SetProperty(ref _canNavigate, value);
            }
        }

        protected bool IsConnectedToInternet()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet;
        }

        protected bool IsAudioExist(string audioName)
        {
            List<string> audios = new List<string>();
            if (Preferences.ContainsKey(MenuKey.AUDIO_MP3))
            {
                List<string> getAudioMP3 = JsonConvert.DeserializeObject<List<string>>(Preferences.Get(MenuKey.AUDIO_MP3, null));

                if (getAudioMP3.Contains(audioName))
                {
                    return true;
                }
            }

            return false;
        }
        public void StopPlayer()
        {
            //MP3Helper.StopPlayer();
            MP3Service.StopPlayer();
        }

        public string GetCollapsImageHeaderName()
        {

            if (Preferences.Get(MyQuranIndo.References.Setting.THEME, 0) == (int)ThemeStyle.Black)
            {
                return "collapse_black.png";
            }

            return "collapse.png";
        }

        public string GetExpandImageHeaderName()
        {

            if (Preferences.Get(MyQuranIndo.References.Setting.THEME, 0) == (int)ThemeStyle.Black)
            {
                return "expand_black.png";
            }

            return "expand.png";
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void RaisePropertyChanged(string propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
    }
}
