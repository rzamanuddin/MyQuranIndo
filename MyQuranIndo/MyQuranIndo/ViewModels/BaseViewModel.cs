using MyQuranIndo.Models;
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
        public IMP3Service MP3Service => DependencyService.Get<IMP3Service>();
        public IZikrDataService ZikrDataService => DependencyService.Get<IZikrDataService>();
        public IAsmaulHusnaDataService AsmaulHusnaDataService => DependencyService.Get<IAsmaulHusnaDataService>();
        public IPrayDataService PrayDataService => DependencyService.Get<IPrayDataService>();

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
