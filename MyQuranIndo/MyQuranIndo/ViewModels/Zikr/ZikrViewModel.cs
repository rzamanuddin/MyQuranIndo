using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Zikr
{
    [QueryProperty(nameof(ZikrTime), nameof(ZikrTime))]
    public class ZikrViewModel : BaseViewModel, IHasCollectionViewModel
    {
        private ICommand _searchCommand;

        public ObservableCollection<Models.Zikrs.Zikr> Zikrs { get; }
        public ICommand LoadCommand { get; }
        public ICommand ZikrOneTapped { get; }

        private string zikrTime;
        public string ZikrTime
        {
            get => zikrTime;
            set => zikrTime = value;
        }

        public double FontSizeArabic => FontHelper.GetFontSizeArabic();

        private string titlePage;
        public String TitlePage
        {
            get => titlePage;
            set => SetProperty(ref titlePage, value);
        }

        private string subTitlePage;
        public String SubTitlePage
        {
            get => subTitlePage;
            set => SetProperty(ref subTitlePage, value);
        }

        public IHasCollectionView View { get; set; }

        public ZikrViewModel()
        {
            Title = "Dzikir Pagi";
            TitlePage = "Dzikir yang Dibaca di Waktu Pagi";
            SubTitlePage = "(Antara Shubuh hingga siang hari ketika matahari akan bergeser ke barat)";
            Zikrs = new ObservableCollection<Models.Zikrs.Zikr>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ZikrOneTapped = new Command<Models.Zikrs.Zikr>(OnZikrOneTapped);
        }

        private void ScrollToItem(int index, bool isAnimated = false)
        {
            if (View?.CollectionView != null && index > 0 && index <= Zikrs.Count)
            {
                View.CollectionView.ScrollTo(index - 1, -1, position: ScrollToPosition.Start, isAnimated);
            }
        }

        private void ScrollToItem(Models.Zikrs.Zikr zikr, bool isAnimated = false)
        {
            if (View?.CollectionView != null && zikr != null)
            {
                View.CollectionView.ScrollTo(zikr, -1, position: ScrollToPosition.Start, isAnimated);
            }
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                Zikrs.Clear();
                var rawZikrs = await ZikrDataService.GetZikrsAsync();
                if (rawZikrs == null) return;

                IEnumerable<Models.Zikrs.Zikr> zikrs = rawZikrs;

                if (this.ZikrTime == "0")
                {
                    zikrs = zikrs.Where(q => q?.Time != null && q.Time.Trim().ToLower() == "pagi" || string.IsNullOrEmpty(q?.Time));
                    Title = "Dzikir Pagi";
                    TitlePage = "Dzikir yang Dibaca di Waktu Pagi";
                    SubTitlePage = "Antara Shubuh hingga siang hari ketika matahari akan bergeser ke barat";
                }
                else if (this.ZikrTime == "1")
                {
                    zikrs = zikrs.Where(q => q?.Time != null && q.Time.Trim().ToLower() == "petang" || string.IsNullOrEmpty(q?.Time));
                    Title = "Dzikir Petang";
                    TitlePage = "Dzikir yang Dibaca di Waktu Petang";
                    SubTitlePage = "Dari tenggelam matahari atau waktu Maghrib hingga pertengahan malam";
                }

                var zikrsList = zikrs.ToList();
                for (int i = 0; i < zikrsList.Count; i++)
                {
                    var zm = zikrsList[i];
                    if (zm == null) continue;

                    zm.RowID = i + 1;

                    // Ayat Kursi
                    if (zm.ID == 1)
                    {
                        var ayatKursi = await AyahDataService.GetAsync(2, 255);
                        if (ayatKursi != null)
                        {
                            zm.ArabicLatin = ayatKursi.TextIndo;
                            zm.Arabic = ayatKursi.ReadText;
                        }
                    }
                    // Al Ikhlas - An Naas
                    else if (zm.ID == 2)
                    {
                        var alIkhlas = await AyahDataService.GetAsync(112);
                        var alFalaq = await AyahDataService.GetAsync(113);
                        var anNaas = await AyahDataService.GetAsync(114);
                        var bismillah = await AyahDataService.GetAsync(1, 1);

                        zm.Arabic = string.Empty;
                        zm.TranslateID = string.Empty;
                        zm.ArabicLatin = string.Empty;

                        // Helper builder untuk surat pendek
                        void AppendSurahData(IList<Ayah> ayahs)
                        {
                            if (bismillah != null)
                            {
                                zm.Arabic += $"{bismillah.ReadText}\n";
                                zm.TranslateID += $"{bismillah.TranslateIndo}\n";
                                zm.ArabicLatin += $"{bismillah.TextIndo}\n";
                            }

                            if (ayahs != null)
                            {
                                for (int ayah = 0; ayah < ayahs.Count; ayah++)
                                {
                                    var item = ayahs[ayah];
                                    if (item == null) continue;

                                    zm.Arabic += $"{item.ReadText}  ";
                                    zm.TranslateID += $"{item.TranslateIndo} ";
                                    zm.ArabicLatin += item.TextIndo + (ayah < ayahs.Count - 1 ? ", " : " ");
                                }
                            }
                            zm.Arabic += "\n\n";
                            zm.TranslateID += "\n\n";
                            zm.ArabicLatin += "\n\n";
                        }

                        AppendSurahData(alIkhlas);
                        AppendSurahData(alFalaq);
                        AppendSurahData(anNaas);
                    }

                    Zikrs.Add(zm);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_ZIKR
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
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
                    int id = 0;
                    int.TryParse(text, out id);
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);
                    if (id > 0)
                    {
                        if (id <= Zikrs.Count)
                        {
                            ScrollToItem(id);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var s = Zikrs.FirstOrDefault(q => q?.Title != null && q.Title.ToLower().Contains(text.ToLower()));
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

        private bool IsValidTapped(Models.Zikrs.Zikr zikr)
        {
            if (zikr == null || zikr.ID == 0) return false;
            return true;
        }

        private async void OnZikrOneTapped(Models.Zikrs.Zikr zikr)
        {
            if (IsValidTapped(zikr))
            {
                MP3Service.StopPlayer();
                var oldColor = zikr.RowColor;
                zikr.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                string result = "";
                string errorMessage = "";
                string action = await ActionHelper.DisplayActionZikrAsync(zikr.TitleAndNumber);
                string zikrCopied = ActionHelper.GetZikrToShare(zikr, this.Title);
                try
                {
                    switch (action)
                    {
                        case ActionHelper.ZIKR_SHARE:
                            await ActionHelper.ShareZikrAsync(zikrCopied);
                            break;
                        case ActionHelper.ZIKR_COPY:
                            await Clipboard.SetTextAsync(zikrCopied);
                            result = ActionHelper.ZIKR_COPY + " Berhasil.";
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
                        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, errorMessage, Message.MSG_OK);
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine
                        + ex.Message, Message.MSG_OK);
                }
                finally
                {
                    zikr.RowColor = oldColor;
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}