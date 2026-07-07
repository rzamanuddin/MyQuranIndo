using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.References;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Surah;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
        private List<String> audios = new List<string>();
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
            //SurahFilter = new SurahFilter();
            Surahs = new ObservableCollection<Models.Qurans.Surah>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ItemTapped = new Command<Models.Qurans.Surah>(OnSurahSelected, (x) => CanNavigate);
            DownloadCommand = new Command<Models.Qurans.Surah>(async (surah) => await OnDownloadSelected(surah));
            Initialization = InitStart();
        }

        private void ScrollToItem(int index, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(index - 1, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }
        private void ScrollToItem(Models.Qurans.Surah surah, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(surah, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
        {
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                {
                    ScrollToItem(1);
                }
                else
                {
                    int surahID = 0;
                    int.TryParse(text, out surahID);
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (surahID > 0)
                    {
                        if (surahID > 0 && surahID <= Surahs.Count)
                        {
                            ScrollToItem(surahID);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var s = Surahs.FirstOrDefault(q => q.NameLatin.ToLower().Contains(text));
                        if (s != null)
                        {
                            ScrollToItem(s);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }));

        private async void OnSurahSelected(Models.Qurans.Surah surah)
        {
            if (surah == null)
            {
                return;
            }
            if (surah.ID == 0)
            {
                return;
            }

            CanNavigate = false;
            var oldColor = surah.RowColorPopular;
            surah.RowColorPopular = (Color)Application.Current.Resources["SelectedItem"];
            await Shell.Current.GoToAsync($"{nameof(SurahDetailPage)}?{nameof(SurahDetailViewModel.SurahID)}={surah.ID}");
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
                Surahs.Clear();
                var surah = await SurahDataService.GetSurahPopularAsync(true);
                for (int i = 0; i < surah.Count; i++)
                {
                    surah[i].SurahPopularID = i + 1;
                    Surahs.Add(surah[i]);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_SURAH
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnDownloadSelected(Models.Qurans.Surah surah)
        {
            if (surah != null)
            {
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

                        FileStream fs = null;

                        byte[] mp3File = null;
                        try
                        {
                            if (!File.Exists(fileName))
                            {
                                surah.StatusTextProgressBar = $"Unduh Ayat {i} dari {surah.NumberOfAyah}";
                                surah.ProgressValue = (double)i / (double)surah.NumberOfAyah;
                                mp3File = await MP3Service.DownloadAsync(Path.Combine(url, mp3Name));
                                File.WriteAllBytes(fileName, mp3File);
                            }
                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 +
                                Environment.NewLine + ex.Message, Message.MSG_OK);
                        }
                        finally
                        {
                            if (fs != null)
                                fs.Dispose();
                        }
                    }
                    surah.IsVisibleProgressBar = false;

                    var audios = new List<string>();
                    if (Preferences.ContainsKey(MenuKey.AUDIO_MP3))
                    {
                        List<string> getAudioMP3 = JsonConvert.DeserializeObject<List<string>>(Preferences.Get(MenuKey.AUDIO_MP3, null));
                        audios.AddRange(getAudioMP3);
                    }
                    audios.Add(audioName);
                    Preferences.Set(MenuKey.AUDIO_MP3, JsonConvert.SerializeObject(audios));

                    ToastService.Show($"Audio murottal {ReciterHelper.GetReciterName()} surat {surah.NameLatin} berhasil diunduh.", false, true);
                }
            }
        }

        public void OnAppearing()
        {
            //IsBusy = true;
            SelectedSurah = null;
        }

    }
}
