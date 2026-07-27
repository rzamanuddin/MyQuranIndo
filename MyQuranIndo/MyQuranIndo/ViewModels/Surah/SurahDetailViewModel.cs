using MyQuranIndo.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Helpers;
using System.IO;

namespace MyQuranIndo.ViewModels.Surah
{
    [QueryProperty(nameof(SurahID), nameof(SurahID))]
    [QueryProperty(nameof(AyahID), nameof(AyahID))]
    public class SurahDetailViewModel : BaseViewModel, IHasListViewViewModel //TODO:
    {
        private string result = "", errorMessage = "";

        private string surahID;
        private MyQuranIndo.Models.Qurans.Surah surah;
        private string subTitle;
        private bool isBismillahShow;
        private string ayahID;
        private ICommand _searchCommand;

        //private ICommand _swipeCommand;

        public string Bismillah
        {
            get => FontHelper.GetBismillah();
        }

        private async Task OnTogglePlayCommand()
        {
            try
            {
                if (playbackState == PlaybackState.Stopped)
                {
                    // start playback
                    if (Surah == null)
                        return;

                    if (CurrentMP3 <= 0)
                        CurrentMP3 = 1;

                    await OnPlayMP3Command();
                    // OnPlayMP3Command sets state and PlayButtonText
                }
                else if (playbackState == PlaybackState.Playing)
                {
                    // pause
                    if (MP3Service?.Player != null && MP3Service.IsPlaying())
                    {
                        MP3Service.Pause();
                        IsStopVisibled = false;
                        playbackState = PlaybackState.Paused;
                        PlayButtonIcon = "\u25B6"; // ▶ (resume)
                        double duration = MP3Service.GetDuration();
                        if (duration > 0)
                        {
                            double pos = MP3Service.GetCurrentPosition();
                            Progress = (float)(pos / duration);
                            Percent = Progress * 100;
                        }
                        VisibleProgressBar = true;
                    }
                }
                else if (playbackState == PlaybackState.Paused)
                {
                    // resume
                    if (MP3Service?.Player != null)
                    {
                        MP3Service.Play();
                        IsStopVisibled = true;
                        playbackState = PlaybackState.Playing;
                        PlayButtonIcon = "\u23F8"; // ⏸ (pause)
                        double duration = MP3Service.GetDuration();
                        if (duration > 0)
                        {
                            double pos = MP3Service.GetCurrentPosition();
                            Progress = (float)(pos / duration);
                            Percent = Progress * 100;
                        }
                        VisibleProgressBar = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
        }
        public string BismillahUthmani
        {
            get => "بِسۡمِ ٱللَّهِ ٱلرَّحۡمَٰنِ ٱلرَّحِيمِ";
        }
        public string BismillahTranslate
        {
            get => "Dengan nama Allah Yang Maha Pengasih, Maha Penyayang.";
        }

        public bool IsBismillahShow
        {
            get => isBismillahShow;
            set => SetProperty(ref isBismillahShow, value);
        }

        public MyQuranIndo.Models.Qurans.Surah Surah
        {
            get => surah;
            set => SetProperty(ref surah, value);
        }

        public string SubTitle
        {
            get => subTitle;
            set => SetProperty(ref subTitle, value);
        }

        public ObservableCollection<MyQuranIndo.Models.Qurans.Ayah> Ayahs { get; set;  }

        public string ID { get; set; }
        public string SurahID
        {
            get
            {
                return surahID;
            }
            set
            {
                surahID = value;
                LoadSurahID(value);
            }
        }

        public string AyahID
        {
            get
            {
                return ayahID;
            }
            set
            {
                ayahID = value;
                LoadSurahID(SurahID, value);
            }
        }

        private bool visibleSearchBar = false;
        public bool VisibleSearchBar
        {
            get => visibleSearchBar;
            set => SetProperty(ref visibleSearchBar, value);
        }

        private bool visibleProgressBar = true;
        public bool VisibleProgressBar
        {
            get => visibleProgressBar;
            set => SetProperty(ref visibleProgressBar, value);
        }

        public IHasListView View
        {
            get; set;
        }
        private float progress;
        public float Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }

        float percent = 0f;
        public float Percent
        {
            get => percent;
            set => SetProperty(ref percent, value);
        }

        string fileName;
        public string FileName
        {
            get => fileName;
            set => SetProperty(ref fileName, value);
        }
                
        private int countMP3 = 0;
        public int CountMP3
        {
            get => countMP3;
            set => SetProperty(ref countMP3, value);
        }

        private int currentMP3 = 1;
        public int CurrentMP3
        {
            get => currentMP3;
            set => SetProperty(ref currentMP3, value);
        }

        // handler reference so we can detach/attach playback ended handler
        private EventHandler playerPlaybackEndedHandler;
        private enum PlaybackState { Stopped, Playing, Paused }
        private PlaybackState playbackState = PlaybackState.Stopped;

        // private string playButtonText = "Play"; (removed, use PlayButtonIcon)
        // now using icon glyph for button
        private string playButtonIcon = "\u25B6"; // ▶
        public string PlayButtonIcon { get => playButtonIcon; set => SetProperty(ref playButtonIcon, value); }

        private bool isPlayerEnabled = false;
        public bool IsPlayerEnabled { get => isPlayerEnabled; set => SetProperty(ref isPlayerEnabled, value); }

        private bool isStopVisibled = false;
        public bool IsStopVisibled 
        { 
            get => isStopVisibled; 
            set => SetProperty(ref isStopVisibled, value); 
        }

        // Play button now shows an icon glyph via PlayButtonIcon

        public ICommand TogglePlayCommand { get; }


        public Command LoadCommand { get; private set; }
        public ICommand AyahOneTapped { get; }
        public ICommand AyahTwoTapped { get; }
        public Command SettingTapped { get; }
        public ICommand GoBackCommand { get; }
        public ICommand DownloadMP3Command { get; }
        public ICommand PlayMP3Command { get; }
        public ICommand PauseMP3Command { get; }
        public ICommand StopMP3Command { get; }

        public SurahDetailViewModel()
        {
            Title = "Surah";
            Ayahs = new ObservableCollection<Ayah>();            
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            // AyahOneTapped = new Command<Ayah>(OnAyahOneTapped);
            //AyahOneTapped = new Command<Ayah>(async (ayah) => await OnAyahOneTapped(ayah));
            AyahOneTapped = new Command<Ayah>(async (ayah) =>
            {
                if (IsBusy) return;
                IsBusy = true;
                try
                {
                    await OnAyahOneTapped(ayah);
                }
                finally
                {
                    IsBusy = false;
                }
            });
            AyahTwoTapped = new Command<Ayah>(OnAyahTwoTapped);
            SettingTapped = new Command(OnSettingTapped);
            GoBackCommand = new Command(async () => await OnBackTapped());
            DownloadMP3Command = new Command(async () => await OnDownloadMP3Command());
            PlayMP3Command = new Command(async (x) => await OnPlayMP3Command());
            PauseMP3Command = new Command(async (x) => await OnPauseMP3Command());
            StopMP3Command = new Command(async (x) => await OnStopMP3Command());
            TogglePlayCommand = new Command(async () => await OnTogglePlayCommand());

            PlayButtonIcon = "\u25B6"; // ▶ initial
            IsPlayerEnabled = false;
            IsStopVisibled = false;
        }

        public async Task OnDownloadMP3Command()
        {
            await OnStopMP3Command();
            Progress = 0;
            Percent = 0;
            string url = ReciterHelper.GetReciterUrl();
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            int count = surah.NumberOfAyah;
            VisibleProgressBar = true;
            for (int i = 1; i <= count; i++)
            {
                string mp3Name = $"{surah.ID:000}{i:000}.mp3";
                var mp3File = await MP3Service.DownloadAsync(Path.Combine(url, mp3Name));

                string fileName = Path.Combine(filePath, ReciterHelper.GetReciter().ToString("000") + mp3Name);
                File.WriteAllBytes(fileName, mp3File);
                Progress = (float)i / count;
                Percent = Progress * 100;
            }
            VisibleProgressBar = false;
            // enable player when surah available
            IsPlayerEnabled = true;
        }

        private async Task OnStopMP3Command()
        {
            MP3Service.StopPlayer();
            Progress = 0;
            Percent = 0;
            CurrentMP3 = 0;
            VisibleProgressBar = true;
            // reset state
            playbackState = PlaybackState.Stopped;
            PlayButtonIcon = "\u25B6"; // ▶
            IsStopVisibled = false;
            try
            {
                if (playerPlaybackEndedHandler != null && MP3Service?.Player != null)
                {
                    MP3Service.Player.PlaybackEnded -= playerPlaybackEndedHandler;
                    playerPlaybackEndedHandler = null;
                }
            }
            catch { }
        }

        private async Task OnPauseMP3Command()
        {
            try
            {
                if (MP3Service?.Player != null && MP3Service.IsPlaying())
                {
                    MP3Service.Pause();
                    double duration = MP3Service.GetDuration();
                    if (duration > 0)
                    {
                        double pos = MP3Service.GetCurrentPosition();
                        Progress = (float)(pos / duration);
                        Percent = Progress * 100;
                    }
                    // keep progress visible while paused
                    VisibleProgressBar = true;
                    playbackState = PlaybackState.Paused;
                    PlayButtonIcon = "\u25B6"; // ▶ (resume)
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
        }

        private async Task OnPlayMP3Command()
        {
            if (Surah == null)
                return;

            CountMP3 = Surah.NumberOfAyah;
            if (CountMP3 <= 0)
                return;

            if (CurrentMP3 <= 0)
                CurrentMP3 = 1;
            Progress = 0f;
            Percent = 0f;
            FileName = "";
            MP3Service.StopPlayer();
            VisibleProgressBar = true;

            // detach previous handler if any
            try
            {
                if (playerPlaybackEndedHandler != null && MP3Service?.Player != null)
                {
                    MP3Service.Player.PlaybackEnded -= playerPlaybackEndedHandler;
                    playerPlaybackEndedHandler = null;
                }

                // create new handler
                playerPlaybackEndedHandler = async (s, e) =>
                {
                    // run on main thread to update UI-bound properties
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        // advance to next mp3
                        if (CurrentMP3 < CountMP3)
                        {
                            CurrentMP3++;
                            Progress = (float)CurrentMP3 / CountMP3;
                            Percent = Progress * 100;
                            ScrollToItem(Ayahs.FirstOrDefault(q => q.ID == CurrentMP3), true);
                            try
                            {
                                await MP3Service.PlayMurottal(Surah.ID, CurrentMP3, null);
                            }
                            catch { }
                        }
                        else
                        {
                            // finished
                            MP3Service.StopPlayer();
                            Progress = 0;
                            Percent = 0;
                            CurrentMP3 = 0;
                            VisibleProgressBar = true;
                            // reset state
                            playbackState = PlaybackState.Stopped;
                            PlayButtonIcon = "\u25B6"; // ▶
                            IsStopVisibled = false;
                        }
                    });
                };

                if (MP3Service?.Player != null)
                    MP3Service.Player.PlaybackEnded += playerPlaybackEndedHandler;

                // play current ayah
                await MP3Service.PlayMurottal(Surah.ID, CurrentMP3, null);
                Progress = (float)CurrentMP3 / CountMP3;
                Percent = Progress * 100;
                ScrollToItem(Ayahs.FirstOrDefault(q => q.ID == CurrentMP3), true);

                // set state
                playbackState = PlaybackState.Playing;
                PlayButtonIcon = "\u23F8"; // ⏸ (pause)
                IsStopVisibled = true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 + Environment.NewLine + ex.Message, Message.MSG_OK);
                VisibleProgressBar = true;
                Progress = 0;
                percent = 0;
                IsStopVisibled = false;
            }
        }

        private async void OnSettingTapped()
        {
            //MP3Helper.StopPlayer();
            await OnStopMP3Command();
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        public async Task OnBackTapped()
        {
            //MP3Service.StopPlayer();
            //MP3Service.StopPlayer(); 
            await OnStopMP3Command();
            await Shell.Current.GoToAsync("..");
        }
        public void ScrollToItem(Ayah ayah, bool isAnimated = false)
        {
            View.ListView.ScrollTo(ayah, position: ScrollToPosition.Start, isAnimated);
        }

        public async void LoadSurahID(string surahID, string ayahID = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(surahID) || surahID == "0")
                {
                    IsPlayerEnabled = false;
                    return;
                }

                var surah = await SurahDataService.GetSurahAsync(Convert.ToInt32(surahID));

                Surah = surah;
                ID = surah.ID.ToString();
                SubTitle = surah.DisplayNameLatin; // String.Format("{0}. {1} (2)", Surah.ID, surah.NameLatin, surah.DisplayNameLatin);
                Title = String.Format("{0}. {1}", surah.ID, surah.NameLatin);

                // enable player now that a surah is loaded
                IsPlayerEnabled = true;

                if (!string.IsNullOrEmpty(ayahID) && ayahID != "0" && ayahID != "1")
                {
                    ScrollToItem(Ayahs.FirstOrDefault(q => q.ID == Convert.ToInt32(ayahID)));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_LOAD_SURAH 
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadCommand()
        {
            try
            {
                //MP3Helper.StopPlayer();
                await OnStopMP3Command();
                Ayahs.Clear();
                CurrentMP3 = 0; 
                playbackState = PlaybackState.Stopped;
                PlayButtonIcon = "\u25B6"; // ▶ Initial
                VisibleProgressBar = true;
                Progress = 0;
                Percent = 0;

                int surahID = 0;
                int.TryParse(SurahID, out surahID);
                var ayahs = await AyahDataService.GetAsync(surahID);

                Ayah a;

                // Add bismillah except surah Al-Fatihah and At-Taubah
                if (surahID != 1 && surahID != 9)
                {
                    a = new Ayah();
                    a.SurahID = surahID;
                    a.ID = 0;
                    a.ReadText = Bismillah;
                    a.ReadTextUthmani = BismillahUthmani;
                    a.TextIndo = "bismillāhir-raḥmānir-raḥīm";
                    a.TranslateIndo = BismillahTranslate;
                    //a.ReadTajwidText = GetFormattedHtml(a.ReadText);
                    Ayahs.Add(a);
                }

                //int i = 1;
                for (int i = 0; i < ayahs.Count; i++)
                {
                    Ayahs.Add(ayahs[i]);
                }

                if (!string.IsNullOrEmpty(this.AyahID) && this.AyahID != "0" && this.AyahID != "1")
                {
                    ScrollToItem(Ayahs.FirstOrDefault(q => q.ID == Convert.ToInt32(this.AyahID)));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR
                    , Message.MSG_FAIL_GET_SURAH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string GetFormattedHtml(string ayahText)
        {
            string body = "";
            string header = "<html><head></head><body>";
            string footer = "</body></html>";

            //// Gunnah (Nun tasydid, Mim tasydid)
            //string[] gunnah = new string[] { "نَّ", "مَّ" };
            //string[] idghamBigunnah = new string[] { "نْ", "\u064B", "\u064C", "\u064D" };
            ////string ya = "\u0649";
            ////String s = "a\u0304\u0308bc\u0327\u0645";

            //TextElementEnumerator charEnum = StringInfo.GetTextElementEnumerator(ayahText);
            //while (charEnum.MoveNext())
            //{
            //    string text = charEnum.GetTextElement();
            //    var isGunnah = gunnah.Any(q => String.Equals(q, text, StringComparison.InvariantCultureIgnoreCase));
            //    //var isIdghamBigunnah = idghamBigunnah.Any(q => String.Equals(q, text, StringComparison.InvariantCultureIgnoreCase));
            //    if (isGunnah)
            //    {
            //        body += string.Format("<span style='color: green'>{0}</span>", text);
            //    }
            //    else
            //    {
            //        body += string.Format("<span>{0}</span>", text);
            //    }
            //}

            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamBigunnahs, TajwidHelper.COLOR_IDGHAM_BIGUNNAH);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamBilagunnahs, TajwidHelper.COLOR_IDGHAM_BILAGUNNAH);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Ikhfas, TajwidHelper.COLOR_IKHFA);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Iqlabs, TajwidHelper.COLOR_IQLAB);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamMimis, TajwidHelper.COLOR_IDGHAM_MIMI);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IkhfaSyafawis, TajwidHelper.COLOR_IKHFA_SYAFAWI);
            //body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Gunnahs, TajwidHelper.COLOR_GUNNAH);

            // TODO: set qalqalah
            // body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Qalqalahs, TajwidHelper.COLOR_QALQALAH);

            return ""; // header + body + footer;
        }

        private bool IsValidTapped(Ayah ayah)
        {
            if (ayah == null)
            {
                return false;
            }

            // Dont show pop up menu at bismillah
            if (ayah.ID == 0)
            {
                return false;
            }

            return true;
        }


        private async void SearchData(string text)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                {
                    ScrollToItem(Ayahs[0]);
                }
                else
                {
                    //MP3Service.StopPlayer();
                    await OnStopMP3Command();
                    int ayahID = 0;
                    int.TryParse(text, out ayahID);
                    string errorMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (ayahID > 0)
                    {
                        if (ayahID > 0 && ((SurahID == "1" || SurahID == "9") && ayahID <= Ayahs.Count) 
                            || ((SurahID != "1" || SurahID != "9") && ayahID < Ayahs.Count))
                        {
                            ScrollToItem(Ayahs.FirstOrDefault(q => q.ID == ayahID));
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var a = Ayahs.FirstOrDefault(q => q.TranslateIndo.ToLower().Contains(text)
                            || q.TextIndo.ToLower().Contains(text));

                        if (a != null)
                        {
                            ScrollToItem(a);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_FIND_AYAH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
        {
            SearchData(text);
        }));

        private async Task ShowTafsir(Ayah ayah, int tafsirType = (int)TafsirType.Kemenag)
        {
            if (IsValidTapped(ayah))
            {
                //MP3Helper.StopPlayer();
                await OnStopMP3Command();
                var oldColor = ayah.RowColor;
                // Show Tafsir
                try
                {
                    ayah.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                    var tafsir = await TafsirDataService.GetAsync(getSurahID(), ayah.ID);
                    //string tafsir = //getSurahID()Tafsir.TafsirID.TafsirKemenag.Text[ayah.Number];
                    string title = $"Tafsir {TafsirTypeHelper.GetTafsirTypeName(tafsirType)} Q.S {ayah.SurahID}:{ayah.AyahID}";

                    string tafsirText = "";
                    switch (tafsirType)
                    {
                        case (int)TafsirType.AlJalalain:
                            tafsirText = tafsir.AlJalalain;
                            break;
                        default:
                            tafsirText = tafsir.Kemenag;
                            break;
                    }
                    var isCopy = await ActionHelper.CopyTafsirAsync(title, tafsirText);
                    if (isCopy)
                    {
                        ToastService.Show(ActionHelper.AYAH_COPY + " Berhasil");
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_TAFSIR + Environment.NewLine
                        + ex.Message, Message.MSG_OK);
                }
                finally
                {
                    ayah.RowColor = oldColor;
                }
            }
        }

        private async void OnAyahTwoTapped(Ayah ayah)
        {
            //await ShowTafsir(ayah);
        }

        private int getSurahID()
        {
            int surahID = 0;
            int.TryParse(SurahID, out surahID);
            return surahID;
        }
        private async Task OnAyahOneTapped(Ayah ayah)
        {
            if (IsValidTapped(ayah))
            {
                await OnStopMP3Command();
                var oldColor = ayah.RowColor;
                ayah.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                result = "";
                errorMessage = "";
                var surah = await SurahDataService.GetSurahAsync(ayah.SurahID);                
                string action = await ActionHelper.DisplayActionAyahAsync(ayah.SurahID, ayah.AyahID, surah.NameLatin);
                string ayahCopied = ActionHelper.GetAyahToShare(ayah, Surah.NameLatin);
                try
                {
                    switch (action)
                    {
                        case ActionHelper.AYAH_SHARE:
                            await ActionHelper.ShareAyahAsync(ayahCopied);
                            break;
                        case ActionHelper.AYAH_COPY:
                            await Clipboard.SetTextAsync(ayahCopied);
                            result = ActionHelper.AYAH_COPY + " Berhasil";
                            break;
                        case ActionHelper.AYAH_BOOKMARK:
                            //SetBookmarkAyah(ayah);
                            ActionHelper.BookmarkAyah(ayah, Surah.NameLatin, out result, out errorMessage);
                            break;
                        case ActionHelper.AYAH_LAST_READ:
                            //await SetLastReadAyah(ayah);
                            var lastRead = await ActionHelper.SetLastReadAyahAsync(ayah, Surah.NameLatin, SurahDataService);
                            result = lastRead.Item1;
                            errorMessage = lastRead.Item2;
                            break;
                        case ActionHelper.PLAY_MP3:
                            //await MP3Helper.PlayMurottal(ayah.SurahID, ayah.AyahID, ToastService);
                            // await MP3Service.PlayMurottal(ayah.SurahID, ayah.AyahID, ToastService);
                            CurrentMP3 = ayah.AyahID;

                            await OnPlayMP3Command();
                            break;
                        case ActionHelper.TAFSIR_KEMENAG_SHOW:
                            await ShowTafsir(ayah, (int)TafsirType.Kemenag);
                            break;
                        case ActionHelper.TAFSIR_Al_JALALAIN_SHOW:
                            await ShowTafsir(ayah, (int)TafsirType.AlJalalain);
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(this.result))
                    {
                        //await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, this.result, Message.MSG_OK);
                        ToastService.Show(this.result);
                    }
                    if (!string.IsNullOrWhiteSpace(this.errorMessage))
                    {
                        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, this.errorMessage, Message.MSG_OK);
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine
                        + ex.Message, Message.MSG_OK);
                }
                finally
                {
                    ayah.RowColor = oldColor;
                }
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
            //SelectedSurah = null;
        }
    }
}
