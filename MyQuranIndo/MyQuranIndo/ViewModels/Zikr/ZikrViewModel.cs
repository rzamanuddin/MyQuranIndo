using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
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
            get
            {
                return zikrTime;
            }
            set
            {
                zikrTime = value;
            }
        }
        public double FontSizeArabic
        {
            get
            {
                return FontHelper.GetFontSizeArabic();
            }
        }

        private string titlePage;
        public String TitlePage
        {
            get => titlePage;
            set { SetProperty(ref titlePage, value); }
        }

        private string subTitlePage;
        public String SubTitlePage
        {
            get => subTitlePage;
            set { SetProperty(ref subTitlePage, value); }
        }
        public IHasCollectionView View { get; set; }

        public ZikrViewModel()
        {            
            Title = "Dzikir Pagi";
            TitlePage = "Dzikir yang Dibaca di Waktu Pagi";
            SubTitlePage = "(Antara Shubuh hingga siang hari ketika matahari akan bergeser ke barat)";
            Zikrs = new ObservableCollection<Models.Zikrs.Zikr>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            //AyahOneTapped = new Command<Ayah>(OnAyahOneTapped);
            ZikrOneTapped = new Command<Models.Zikrs.Zikr>(OnZikrOneTapped);
        }

        private void ScrollToItem(int index, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(index, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }
        private void ScrollToItem(Models.Zikrs.Zikr zikr, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(zikr, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                Zikrs.Clear();
                IEnumerable<Models.Zikrs.Zikr> zikrs = await ZikrDataService.GetZikrsAsync();

                if (this.ZikrTime == "0")
                {
                    zikrs = zikrs.Where(q => q.Time.Trim().ToLower() == "pagi" || string.IsNullOrEmpty(q.Time));
                    Title = "Dzikir Pagi";
                    TitlePage = "Dzikir yang Dibaca di Waktu Pagi";
                    SubTitlePage = "Antara Shubuh hingga siang hari ketika matahari akan bergeser ke barat";
                }
                else if (this.ZikrTime == "1")
                {
                    zikrs = zikrs.Where(q => q.Time.Trim().ToLower() == "petang" || string.IsNullOrEmpty(q.Time));
                    Title = "Dzikir Petang";
                    TitlePage = "Dzikir yang Dibaca di Waktu Petang";
                    SubTitlePage = "Dari tenggelam matahari atau waktu Maghrib hingga pertengahan malam";
                }
                var zikrsMorning = zikrs.ToList();
                for (int i = 0; i < zikrsMorning.Count; i++)
                {
                    var zm = zikrsMorning[i];
                    zm.RowID = i + 1;
                    // Ayat Kursi
                    if (zm.ID == 1)
                    {
                        var ayatKursi = await AyahDataService.GetAsync(2, 255);
                        zm.ArabicLatin = ayatKursi.TextIndo;
                        zm.Arabic = ayatKursi.ReadText;
                    } // Al Ikhlas - An Naas
                    else if(zm.ID == 2) {
                        var alIkhlas = await AyahDataService.GetAsync(112);
                        var alFalaq = await AyahDataService.GetAsync(113);
                        var anNaas = await AyahDataService.GetAsync(114);
                        var bismillah = await AyahDataService.GetAsync(1, 1);
                        
                        zm.Arabic = "";
                        zm.TranslateID = "";
                        zm.ArabicLatin = "";

                        zm.Arabic += $"{bismillah.ReadText}\n";
                        zm.TranslateID += $"{bismillah.TranslateIndo}\n";
                        zm.ArabicLatin += $"{bismillah.TextIndo}\n";
                        for (int ayah = 0; ayah < alIkhlas.Count; ayah++)
                        {
                            zm.Arabic += $"{alIkhlas[ayah].ReadText}  ";
                            zm.TranslateID += $"{alIkhlas[ayah].TranslateIndo} ";
                            zm.ArabicLatin += $"{alIkhlas[ayah].TextIndo} ";
                        }
                        zm.Arabic += "\n";
                        zm.TranslateID += "\n";
                        zm.ArabicLatin += "\n";

                        zm.Arabic += $"{bismillah.ReadText}\n";
                        zm.TranslateID += $"{bismillah.TranslateIndo}\n";
                        zm.ArabicLatin += $"{bismillah.TextIndo}\n";
                        for (int ayah = 0; ayah < alFalaq.Count; ayah++)
                        {
                            zm.Arabic += $"{alFalaq[ayah]}  ";
                            zm.TranslateID += $"{alFalaq[ayah].TranslateIndo} ";
                            zm.ArabicLatin += $"{alFalaq[ayah].TextIndo} ";
                        }
                        zm.Arabic += "\n";
                        zm.TranslateID += "\n";
                        zm.ArabicLatin += "\n";

                        zm.Arabic += $"{bismillah.ReadText}\n";
                        zm.TranslateID += $"{bismillah.TranslateIndo}\n";
                        zm.ArabicLatin += $"{bismillah.TextIndo}\n";
                        for (int ayah = 0; ayah < anNaas.Count; ayah++)
                        {
                            zm.Arabic += $"{anNaas[ayah]}  ";
                            zm.TranslateID += $"{anNaas[ayah].TranslateIndo} ";
                            zm.ArabicLatin += $"{anNaas[ayah].TextIndo} ";
                        }
                    }

                    Zikrs.Add(zm);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_ZIKR
                    + Environment.NewLine + ex.Message, Message.MSG_OK); ;
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
                        if (id > 0 && id <= Zikrs.Count)
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
                        var s = Zikrs.FirstOrDefault(q => q.Title.ToLower().Contains(text));
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
            if (zikr == null)
            {
                return false;
            }

            // Dont show pop up menu at ta'awudz
            if (zikr.ID == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// On Zikr tapped one
        /// </summary>
        /// <param name="zikr"></param>
        private async void OnZikrOneTapped(Models.Zikrs.Zikr zikr)
        {
            if (IsValidTapped(zikr))
            {
                //MP3Helper.StopPlayer();
                MP3Service.StopPlayer();
                var oldColor = zikr.RowColor;
                zikr.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                string result = "";
                string errorMessage = "";
                //var surah = await SurahDataService.GetSurahAsync(zikr.SurahID);
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
