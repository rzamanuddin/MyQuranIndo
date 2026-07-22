using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.References;
using MyQuranIndo.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Surah
{
    public class SurahPopularsViewModel : BaseViewModel, IHasCollectionViewModel
    {
        private Models.Qurans.Surah _selectedSurah;
        private ICommand _searchCommand;
        private List<string> audios = new List<string>();
        private int indexAudio = 0;

        public ObservableCollection<Models.Qurans.Surah> Surahs { get; }
        public Command LoadCommand { get; }
        public Command<Models.Qurans.Surah> ItemTapped { get; }
        public ICommand DownloadCommand { get; }

        public Models.Qurans.Surah SelectedSurah
        {
            get => _selectedSurah;
            set
            {
                SetProperty(ref _selectedSurah, value);
                OnSurahSelected(value);
            }
        }

        public IHasCollectionView View { get; set; }
        public Task Initialization { get; private set; }

        public SurahPopularsViewModel()
        {
            Title = "Daftar Surat Populer";
            Surahs = new ObservableCollection<Models.Qurans.Surah>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ItemTapped = new Command<Models.Qurans.Surah>(OnSurahSelected, (x) => CanNavigate);
            DownloadCommand = new Command<Models.Qurans.Surah>(async (surah) => await OnDownloadSelected(surah));
            Initialization = InitStart();
        }

        private void ScrollToItem(int index, bool isAnimated = false)
        {
            if (View?.CollectionView != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    View.CollectionView.ScrollTo(index - 1, -1, position: ScrollToPosition.Start, isAnimated);
                });
            }
        }

        private void ScrollToItem(Models.Qurans.Surah surah, bool isAnimated = false)
        {
            if (View?.CollectionView != null && surah != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    View.CollectionView.ScrollTo(surah, -1, position: ScrollToPosition.Start, isAnimated);
                });
            }
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
        {
            try
            {
                string keyword = text?.Trim()?.ToLower() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    ScrollToItem(1);
                }
                else
                {
                    int surahID = 0;
                    int.TryParse(keyword, out surahID);
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (surahID > 0)
                    {
                        if (surahID <= Surahs.Count)
                        {
                            ScrollToItem(surahID);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var s = Surahs.FirstOrDefault(q => q.NameLatin != null && q.NameLatin.ToLower().Contains(keyword));
                        if (s != null)
                        {
                            ScrollToItem(s);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }));

        private async void OnSurahSelected(Models.Qurans.Surah surah)
        {
            if (surah == null || surah.ID == 0)
            {
                return;
            }

            CanNavigate = false;
            var oldColor = surah.RowColorPopular;
            surah.RowColorPopular = (Color)Application.Current.Resources["SelectedItem"];

            var juzID = await JuzDataService.GetJuzIDAsync(surah.ID, 1);
            await ActionHelper.OpenPageFromPopularAsync(surah.ID, 1, juzID, surah.NameLatin, ToastService);

            surah.RowColorPopular = oldColor;
            CanNavigate = true;
        }

        private async Task InitStart()
        {
            await ExecuteLoadCommand();
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                var surahList = await SurahDataService.GetSurahPopularAsync(true);
                if (surahList == null) return;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Surahs.Clear();
                    for (int i = 0; i < surahList.Count; i++)
                    {
                        surahList[i].SurahPopularID = i + 1;
                        Surahs.Add(surahList[i]);
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_SURAH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnDownloadSelected(Models.Qurans.Surah surah)
        {
            if (surah == null) return;

            string audioName = ReciterHelper.GetReciter().ToString("000") + surah.ID.ToString("000");
            if (IsAudioExist(audioName))
            {
                audios.Clear();
                for (int i = 1; i <= surah.NumberOfAyah; i++)
                {
                    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string mp3Name = surah.ID.ToString("000") + i.ToString("000") + ".mp3";
                    string fileName = Path.Combine(filePath, ReciterHelper.GetReciter().ToString("000") + mp3Name);
                    audios.Add(fileName);
                }
                indexAudio = 0;
                return;
            }

            var isConfirm = await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_CONFIRM, $"Audio murottal {ReciterHelper.GetReciterName()} Q.S {surah.NameLatin} akan diunduh, anda yakin ?", Message.MSG_YES, Message.MSG_CANCEL);

            if (isConfirm)
            {
                surah.IsVisibleProgressBar = true;
                surah.StatusTextProgressBar = "";
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                for (int i = 1; i <= surah.NumberOfAyah; i++)
                {
                    string mp3Name = surah.ID.ToString("000") + i.ToString("000") + ".mp3";
                    string fileName = Path.Combine(filePath, ReciterHelper.GetReciter().ToString("000") + mp3Name);
                    string url = ReciterHelper.GetReciterUrl();

                    try
                    {
                        if (!File.Exists(fileName))
                        {
                            surah.StatusTextProgressBar = $"Unduh Ayat {i} dari {surah.NumberOfAyah}";
                            surah.ProgressValue = (double)i / surah.NumberOfAyah;
                            byte[] mp3File = await MP3Service.DownloadAsync(Path.Combine(url, mp3Name));
                            File.WriteAllBytes(fileName, mp3File);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 + Environment.NewLine + ex.Message, Message.MSG_OK);
                    }
                }
                surah.IsVisibleProgressBar = false;

                var savedAudios = new List<string>();
                if (Preferences.ContainsKey(MenuKey.AUDIO_MP3))
                {
                    List<string> getAudioMP3 = JsonConvert.DeserializeObject<List<string>>(Preferences.Get(MenuKey.AUDIO_MP3, null));
                    if (getAudioMP3 != null)
                    {
                        savedAudios.AddRange(getAudioMP3);
                    }
                }
                savedAudios.Add(audioName);
                Preferences.Set(MenuKey.AUDIO_MP3, JsonConvert.SerializeObject(savedAudios));

                ToastService.Show($"Audio murottal {ReciterHelper.GetReciterName()} surat {surah.NameLatin} berhasil diunduh.", false, true);
            }
        }

        private bool IsAudioExist(string audioName)
        {
            // Placeholder pengecekan file audio jika diperlukan
            return false;
        }

        public void OnAppearing()
        {
            SelectedSurah = null;
        }

        private async Task DisplayAlertSafe(string title, string message, string cancel)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (App.Current?.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert(title, message, cancel);
                }
            });
        }
    }
}