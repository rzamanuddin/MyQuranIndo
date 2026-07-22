using MyQuranIndo.Configuration;
using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Bookmarks;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.References;
using MyQuranIndo.Views;
using MyQuranIndo.Views.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Juz
{
    [QueryProperty(nameof(JuzID), nameof(JuzID))]
    [QueryProperty(nameof(SurahID), nameof(SurahID))]
    [QueryProperty(nameof(AyahID), nameof(AyahID))]
    public class JuzDetailViewModel : BaseViewModel, IHasListViewViewModel
    {
        private string result = "", errorMessage = "";

        private string juzID;
        private MyQuranIndo.Models.Qurans.JuzHeader juzHeader;
        private string subTitle;
        private bool isBismillahShow;
        private string ayahID;
        private ICommand _searchCommand;

        public string Bismillah => "بِسْمِ اللّٰهِ الرَّحْمٰنِ الرَّحِيْمِ";

        public string BismillahUthmani => "بِسۡمِ ٱللَّهِ ٱلرَّحۡمَٰنِ ٱلرَّحِيمِ";

        public string BismillahTranslate => "Dengan nama Allah Yang Maha Pengasih, Maha Penyayang.";

        public bool IsBismillahShow
        {
            get => isBismillahShow;
            set => SetProperty(ref isBismillahShow, value);
        }

        public MyQuranIndo.Models.Qurans.JuzHeader JuzHeader
        {
            get => juzHeader;
            set => SetProperty(ref juzHeader, value);
        }

        public string SubTitle
        {
            get => subTitle;
            set => SetProperty(ref subTitle, value);
        }

        public ObservableCollection<JuzDetail> JuzDetails { get; set; }

        public ObservableCollection<SurahGroup> SurahGroups { get; set; }

        public string ID { get; set; }

        public string JuzID
        {
            get => juzID;
            set
            {
                juzID = value;
                LoadJuzID(value);
            }
        }

        private string surahID;
        public string SurahID
        {
            get => surahID;
            set => surahID = value;
        }

        public string AyahID
        {
            get => ayahID;
            set
            {
                ayahID = value;
                LoadJuzID(JuzID, SurahID, value);
            }
        }

        public IHasListView View { get; set; }

        private bool visibleSearchBar = false;
        public bool VisibleSearchBar
        {
            get => visibleSearchBar;
            set => SetProperty(ref visibleSearchBar, value);
        }

        public Command LoadCommand { get; private set; }
        public ICommand AyahOneTapped { get; }
        public ICommand AyahTwoTapped { get; }
        public ICommand SettingTapped { get; }
        public ICommand GoBackCommand { get; }

        public JuzDetailViewModel()
        {
            Title = "Juz";
            JuzDetails = new ObservableCollection<JuzDetail>();
            SurahGroups = new ObservableCollection<SurahGroup>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            AyahOneTapped = new Command<JuzDetail>(OnAyahOneTapped);
            AyahTwoTapped = new Command<JuzDetail>(OnAyahTwoTapped);
            SettingTapped = new Command(OnSettingTapped);
            GoBackCommand = new Command(async () => await OnBackTapped());
        }

        private async void OnSettingTapped()
        {
            MP3Service.StopPlayer();
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        // PERBAIKAN BUG A: Pengecekan Null Safe pada ScrollToItem
        public void ScrollToItem(JuzDetail juzDetail, bool isAnimated = false)
        {
            if (juzDetail != null && View?.ListView != null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    View.ListView.ScrollTo(juzDetail, position: ScrollToPosition.Start, isAnimated);
                });
            }
        }

        public async void LoadJuzID(string targetJuzID, string targetSurahID = null, string targetAyahID = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(targetJuzID) || targetJuzID == "0")
                {
                    return;
                }

                var header = await JuzDataService.GetJuzHeaderAsync(Convert.ToInt32(targetJuzID));
                if (header != null)
                {
                    JuzHeader = header;
                    ID = header.ID.ToString();
                    SubTitle = header.Description;
                    Title = $"Juz {ID} ({header.TotalAyah} Ayat)";
                }

                if (!string.IsNullOrEmpty(targetSurahID) && targetSurahID != "0" && !string.IsNullOrEmpty(targetAyahID) && targetAyahID != "0")
                {
                    var targetItem = JuzDetails.FirstOrDefault(q => q.SurahID == Convert.ToInt32(targetSurahID) && q.AyahID == Convert.ToInt32(targetAyahID));
                    ScrollToItem(targetItem);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_LOAD_JUZ + Environment.NewLine + ex.Message, Message.MSG_OK);
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
                MP3Service.StopPlayer();

                if (!int.TryParse(JuzID, out int parsedJuzID) || parsedJuzID == 0)
                {
                    return;
                }

                var rawJuzDetails = await JuzDataService.GetJuzDetailAsync(parsedJuzID);
                if (rawJuzDetails == null) return;

                var tempJuzDetails = new List<JuzDetail>();

                for (int i = 0; i < rawJuzDetails.Count; i++)
                {
                    // Tambahkan bismillah kecuali surat Al-Fatihah (1) dan At-Taubah (9)
                    if (rawJuzDetails[i].SurahID != 1 && rawJuzDetails[i].SurahID != 9 && rawJuzDetails[i].AyahID == 1)
                    {
                        var bismillahDetail = new JuzDetail
                        {
                            JuzID = parsedJuzID,
                            SurahID = rawJuzDetails[i].SurahID,
                            SurahNameLatin = rawJuzDetails[i].SurahNameLatin,
                            SurahNumberOfAyah = rawJuzDetails[i].SurahNumberOfAyah,
                            SurahTranslateIndo = rawJuzDetails[i].SurahTranslateIndo,
                            AyahID = 0,
                            ReadText = Bismillah,
                            ReadTextUthmani = BismillahUthmani,
                            TextIndo = "bismillāhir-raḥmānir-raḥīm",
                            TranslateIndo = BismillahTranslate,
                            HtmlRead = string.Empty
                        };
                        tempJuzDetails.Add(bismillahDetail);
                    }

                    tempJuzDetails.Add(rawJuzDetails[i]);
                }

                // PERBAIKAN PERFORMA B: Mengisi koleksi sekaligus untuk menghindari UI re-render berulang
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    JuzDetails.Clear();
                    foreach (var item in tempJuzDetails)
                    {
                        JuzDetails.Add(item);
                    }

                    SurahGroups.Clear();
                    var groupedSurahs = tempJuzDetails.GroupBy(q => q.SurahID);

                    foreach (var group in groupedSurahs)
                    {
                        var subList = new ObservableCollection<JuzDetail>(group);
                        int validAyahCount = subList.Count(q => q.AyahID > 0);
                        string surahName = group.FirstOrDefault()?.SurahNameLatin ?? string.Empty;

                        SurahGroups.Add(new SurahGroup(group.Key, surahName, validAyahCount, subList));
                    }
                });

                if (!string.IsNullOrEmpty(SurahID) && SurahID != "0" && !string.IsNullOrEmpty(AyahID) && AyahID != "0")
                {
                    var scrollToTarget = JuzDetails.FirstOrDefault(q => q.SurahID == Convert.ToInt32(SurahID) && q.AyahID == Convert.ToInt32(AyahID));
                    ScrollToItem(scrollToTarget);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_JUZ + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string GetFormattedHtml(string ayahText)
        {
            return string.Empty;
        }

        private bool IsValidTapped(JuzDetail juzDetail)
        {
            if (juzDetail == null || juzDetail.AyahID == 0)
            {
                return false;
            }
            return true;
        }

        private async void SearchData(string text)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    if (JuzDetails.Count > 0)
                        ScrollToItem(JuzDetails[0]);
                }
                else
                {
                    MP3Service.StopPlayer();
                    string searchKeyword = text.Trim().ToLower();
                    string errorMessageText = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (int.TryParse(searchKeyword, out int targetAyahID) && targetAyahID > 0)
                    {
                        var foundAyah = JuzDetails.FirstOrDefault(q => q.AyahID == targetAyahID);
                        if (foundAyah != null)
                        {
                            ScrollToItem(foundAyah);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, errorMessageText, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var matchingAyah = JuzDetails.FirstOrDefault(q =>
                            (q.TranslateIndo != null && q.TranslateIndo.ToLower().Contains(searchKeyword)) ||
                            (q.TextIndo != null && q.TextIndo.ToLower().Contains(searchKeyword)));

                        if (matchingAyah != null)
                        {
                            ScrollToItem(matchingAyah);
                        }
                        else
                        {
                            await DisplayAlertSafe(Message.MSG_TITLE_WARNING, errorMessageText, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_FIND_AYAH + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(SearchData));

        private async Task ShowTafsir(JuzDetail juzDetail, int tafsirType = (int)TafsirType.Kemenag)
        {
            if (!IsValidTapped(juzDetail)) return;

            MP3Service.StopPlayer();
            var oldColor = juzDetail.RowColor;

            try
            {
                juzDetail.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                var tafsir = await TafsirDataService.GetAsync(juzDetail.SurahID, juzDetail.AyahID);
                if (tafsir == null) return;

                string title = $"Tafsir {TafsirTypeHelper.GetTafsirTypeName(tafsirType)} Q.S {juzDetail.SurahID}:{juzDetail.AyahID}";
                string tafsirText = tafsirType == (int)TafsirType.AlJalalain ? tafsir.AlJalalain : tafsir.Kemenag;

                var isCopy = await ActionHelper.CopyTafsirAsync(title, tafsirText);
                if (isCopy)
                {
                    ToastService.Show(ActionHelper.AYAH_COPY + " Berhasil");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_TAFSIR + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                juzDetail.RowColor = oldColor;
            }
        }

        private async void OnAyahTwoTapped(JuzDetail juzDetail)
        {
            // Opsional: Implementasi tap kedua jika diperlukan
            await Task.CompletedTask;
        }

        public async Task OnBackTapped()
        {
            MP3Service.StopPlayer();
            await Shell.Current.GoToAsync("..");
        }

        private async void OnAyahOneTapped(JuzDetail juzDetail)
        {
            if (!IsValidTapped(juzDetail)) return;

            MP3Service.StopPlayer();
            var oldColor = juzDetail.RowColor;
            juzDetail.RowColor = (Color)Application.Current.Resources["SelectedItem"];

            result = "";
            errorMessage = "";

            try
            {
                string action = await ActionHelper.DisplayActionAyahAsync(juzDetail.SurahID, juzDetail.AyahID, juzDetail.SurahNameLatin);
                string ayahCopied = ActionHelper.GetAyahToShare(juzDetail);

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
                        ActionHelper.BookmarkAyah(juzDetail.ToAyah(), juzDetail.SurahNameLatin, out result, out errorMessage);
                        break;
                    case ActionHelper.AYAH_LAST_READ:
                        var lastRead = await ActionHelper.SetLastReadAyahAsync(juzDetail.ToAyah(), juzDetail.SurahNameLatin, SurahDataService);
                        result = lastRead.Item1;
                        errorMessage = lastRead.Item2;
                        break;
                    case ActionHelper.PLAY_MP3:
                        await MP3Service.PlayMurottal(juzDetail.SurahID, juzDetail.AyahID, ToastService);
                        break;
                    case ActionHelper.TAFSIR_KEMENAG_SHOW:
                        await ShowTafsir(juzDetail, (int)TafsirType.Kemenag);
                        break;
                    case ActionHelper.TAFSIR_Al_JALALAIN_SHOW:
                        await ShowTafsir(juzDetail, (int)TafsirType.AlJalalain);
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrWhiteSpace(result))
                {
                    ToastService.Show(result);
                }
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    await DisplayAlertSafe(Message.MSG_TITLE_ERROR, errorMessage, Message.MSG_OK);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                juzDetail.RowColor = oldColor;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        // Helper aman untuk menampilkan Alert di Main Thread
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