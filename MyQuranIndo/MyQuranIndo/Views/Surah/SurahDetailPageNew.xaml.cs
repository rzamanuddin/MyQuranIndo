using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Services;

namespace MyQuranIndo.Views.Surah
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurahDetailPageNew : ContentPage, IQueryAttributable, INotifyPropertyChanged
    {
        private int carouselPosition;
        public int CarouselPosition
        {
            get => carouselPosition;
            set
            {
                if (carouselPosition != value)
                {
                    carouselPosition = value;
                    OnPropertyChanged(nameof(CarouselPosition));
                    _ = LoadAyahForCurrentSurah();
                }
            }
        }

        private MyQuranIndo.Models.Qurans.Surah selectedSurahTab;
        public MyQuranIndo.Models.Qurans.Surah SelectedSurahTab
        {
            get => selectedSurahTab;
            set
            {
                if (selectedSurahTab != value)
                {
                    selectedSurahTab = value;
                    OnPropertyChanged(nameof(SelectedSurahTab));
                    if (selectedSurahTab != null)
                        CarouselPosition = SurahList.IndexOf(selectedSurahTab);
                }
            }
        }

        public ObservableCollection<MyQuranIndo.Models.Qurans.Surah> SurahList { get; } = new ObservableCollection<MyQuranIndo.Models.Qurans.Surah>();
        public ObservableCollection<MyQuranIndo.Models.Qurans.Ayah> CurrentAyahs { get; } = new ObservableCollection<MyQuranIndo.Models.Qurans.Ayah>();

        public Command<MyQuranIndo.Models.Qurans.Ayah> AyahOneTapped { get; }

        private string pendingSurahID;
        private string pendingAyahID;

        public string CurrentSurahName { get; set; }

        public SurahDetailPageNew()
        {
            InitializeComponent();

            AyahOneTapped = new Command<Ayah>(OnAyahOneTapped);

            BindingContext = this;

            // start loading surah list
            _ = InitializeData();
        }

        private async Task InitializeData()
        {
            try
            {
                var surahService = new MyQuranIndo.Services.SurahDataService();
                for (int i = 1; i <= 114; i++)
                {
                    var s = await surahService.GetSurahAsync(i);
                    if (s != null)
                        SurahList.Add(s);
                }

                // select based on pending query if present
                if (!string.IsNullOrEmpty(pendingSurahID) && int.TryParse(pendingSurahID, out var sID) && sID > 0 && sID <= SurahList.Count)
                {
                    CarouselPosition = sID - 1;
                }
                else
                {
                    CarouselPosition = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Init error: {ex.Message}");
            }
        }

        private async Task LoadAyahForCurrentSurah()
        {
            if (CarouselPosition < 0 || CarouselPosition >= SurahList.Count) return;

            var currentSurah = SurahList[CarouselPosition];
            CurrentSurahName = $"{currentSurah.ID}. {currentSurah.NameLatin}";
            OnPropertyChanged(nameof(CurrentSurahName));

            CurrentAyahs.Clear();

            var ayahService = new MyQuranIndo.Services.AyahDataServices();
            var ayahs = await ayahService.GetAsync(currentSurah.ID);

            if (currentSurah.ID != 1 && currentSurah.ID != 9)
            {
                CurrentAyahs.Add(new MyQuranIndo.Models.Qurans.Ayah
                {
                    SurahID = currentSurah.ID,
                    ID = 0,
                    ReadText = "?????? ??????? ???????????? ??????????",
                    TranslateIndo = "Dengan nama Allah Yang Maha Pengasih, Maha Penyayang."
                });
            }

            foreach (var ayah in ayahs)
                CurrentAyahs.Add(ayah);

            // if pending ayah requested (from query), scroll to it
            if (!string.IsNullOrEmpty(pendingAyahID) && int.TryParse(pendingAyahID, out var aID) && aID > 0)
            {
                var target = CurrentAyahs.FirstOrDefault(q => q.ID == aID);
                if (target != null)
                {
                    // notify code-behind UI to scroll; use MessagingCenter for consistency with other pages
                    MessagingCenter.Send(this, "ScrollToAyah", target);
                }
                pendingAyahID = null; // consumed
            }
        }

        private void OnAyahOneTapped(MyQuranIndo.Models.Qurans.Ayah ayah)
        {
            // implement interactions if needed
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.TryGetValue("SurahID", out var s))
                pendingSurahID = s;
            if (query.TryGetValue("AyahID", out var a))
                pendingAyahID = a;

            // if list is already loaded, apply immediately
            if (SurahList.Count > 0 && !string.IsNullOrEmpty(pendingSurahID) && int.TryParse(pendingSurahID, out var sID) && sID > 0 && sID <= SurahList.Count)
            {
                CarouselPosition = sID - 1;
            }
        }

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (e.CurrentItem is MyQuranIndo.Models.Qurans.Surah currentSurah)
            {
                tabsCollectionView.ScrollTo(currentSurah, position: ScrollToPosition.MakeVisible, animate: true);
            }
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MyQuranIndo.Views.Surah.SearchModalPage));
        }

        private async void OnSettingClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MyQuranIndo.Views.Setting.SettingPage));
        }
    }
}
