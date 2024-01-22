using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Zikrs;
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
    //public class PrayViewModel : BaseViewModel, IHasCollectionViewModel
    //{
    //    private ICommand _searchCommand;

    //    public ObservableCollection<Models.Zikrs.Pray> Prays { get; }

    //    public ICommand LoadCommand { get; }

    //    public ICommand PrayOneTapped { get; }

    //    private string zikrTime;
    //    public string ZikrTime
    //    {
    //        get
    //        {
    //            return zikrTime;
    //        }
    //        set
    //        {
    //            zikrTime = value;
    //        }
    //    }
    //    public double FontSizeArabic
    //    {
    //        get
    //        {
    //            return FontHelper.GetFontSizeArabic();
    //        }
    //    }
                
    //    public IHasCollectionView View { get; set; }

    //    public PrayViewModel()
    //    {
    //        Title = "Kumpulan Do'a";
    //        Prays = new ObservableCollection<Models.Zikrs.Pray>();
    //        LoadCommand = new Command(async () => await ExecuteLoadCommand());
    //        //AyahOneTapped = new Command<Ayah>(OnAyahOneTapped);
    //        PrayOneTapped = new Command<Models.Zikrs.Pray>(OnPrayOneTapped);
    //    }

    //    private void ScrollToItem(int index, bool isAnimated = false)
    //    {
    //        View.CollectionView.ScrollTo(index, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
    //    }
    //    private void ScrollToItem(Models.Zikrs.Pray pray, bool isAnimated = false)
    //    {
    //        View.CollectionView.ScrollTo(pray, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
    //    }

    //    private async Task ExecuteLoadCommand()
    //    {
    //        try
    //        {
    //            Prays.Clear();
    //            var prays = await PrayDataService.GetPraysAsync();

    //            for (int i = 0; i < prays.Count; i++)
    //            {
    //                prays[i].RowID = i + 1;
    //                Prays.Add(prays[i]);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_PRAY
    //                + Environment.NewLine + ex.Message, Message.MSG_OK); ;
    //        }
    //        finally
    //        {
    //            IsBusy = false;
    //        }
    //    }

    //    public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
    //    {
    //        try
    //        {
    //            if (String.IsNullOrWhiteSpace(text))
    //            {
    //                ScrollToItem(1);
    //            }
    //            else
    //            {
    //                int id = 0;
    //                int.TryParse(text, out id);
    //                string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);
    //                if (id > 0)
    //                {
    //                    if (id > 0 && id <= Prays.Count)
    //                    {
    //                        ScrollToItem(id - 1);
    //                    }
    //                    else
    //                    {
    //                        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
    //                    }
    //                }
    //                else
    //                {
    //                    var s = Prays.FirstOrDefault(q => q.Title.ToLower().Contains(text));
    //                    if (s != null)
    //                    {
    //                        ScrollToItem(s);
    //                    }
    //                    else
    //                    {
    //                        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET
    //                + Environment.NewLine + ex.Message, Message.MSG_OK);
    //        }
    //        finally
    //        {
    //            IsBusy = false;
    //        }
    //    }));

    //    private bool IsValidTapped(Models.Zikrs.Pray pray)
    //    {
    //        if (pray == null)
    //        {
    //            return false;
    //        }

    //        // Dont show pop up menu at ta'awudz
    //        if (pray.ID == 0)
    //        {
    //            return false;
    //        }

    //        return true;
    //    }

    //    /// <summary>
    //    /// On Zikr tapped one
    //    /// </summary>
    //    /// <param name="pray"></param>
    //    private async void OnPrayOneTapped(Models.Zikrs.Pray pray)
    //    {
    //        if (IsValidTapped(pray))
    //        {
    //            //MP3Helper.StopPlayer();
    //            MP3Service.StopPlayer();
    //            var oldColor = pray.RowColor;
    //            pray.RowColor = (Color)Application.Current.Resources["SelectedItem"];

    //            string result = "";
    //            string errorMessage = "";
    //            //var surah = await SurahDataService.GetSurahAsync(zikr.SurahID);
    //            string action = await ActionHelper.DisplayActionPrayAsync(pray.TitleAndNumber);
    //            string zikrCopied = "";
    //            try
    //            {
    //                switch (action)
    //                {
    //                    case ActionHelper.PRAY_SHARE:
    //                        zikrCopied = ActionHelper.GetPrayToShare(pray, this.Title);
    //                        await ActionHelper.ShareZikrAsync(zikrCopied);
    //                        break;
    //                    case ActionHelper.PRAY_SHARE_ALL:
    //                        zikrCopied = ActionHelper.GetPraysAllToShare(Prays, this.Title);
    //                        await ActionHelper.ShareZikrAsync(zikrCopied);
    //                        break;
    //                    case ActionHelper.PRAY_COPY:
    //                        zikrCopied = ActionHelper.GetPrayToShare(pray, this.Title);
    //                        await Clipboard.SetTextAsync(zikrCopied);
    //                        result = ActionHelper.PRAY_COPY + " Berhasil.";
    //                        break;
    //                    case ActionHelper.PRAY_COPY_ALL:
    //                        zikrCopied = ActionHelper.GetPraysAllToShare(Prays, this.Title);
    //                        await Clipboard.SetTextAsync(zikrCopied);
    //                        result = ActionHelper.PRAY_COPY + " Berhasil.";
    //                        break;
    //                    default:
    //                        break;
    //                }

    //                if (!string.IsNullOrWhiteSpace(result))
    //                {
    //                    ToastService.Show(result);
    //                }
    //                if (!string.IsNullOrWhiteSpace(errorMessage))
    //                {
    //                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, errorMessage, Message.MSG_OK);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine
    //                    + ex.Message, Message.MSG_OK);
    //            }
    //            finally
    //            {
    //                pray.RowColor = oldColor;
    //            }
    //        }
    //    }

    //    public void OnAppearing()
    //    {
    //        IsBusy = true;
    //    }
    //}
    
    public class PrayViewModel : BaseViewModel 
    {

        private string searchQuery;
        private string notFoundMessage;
        private bool isVisibleNotFoundMessage;
        private string numbersFoundAyahAndSurah;
        private Color foundResultColor;
        private MyQuranIndo.Models.Zikrs.PrayData selectedPray;
        private bool isExpandAll = true;
        private bool isVisibleExpandAll = false;
        public ICommand PrayOneTapped { get; }

        //public ObservableCollection<Ayah> Ayahs { get; }
        public ObservableCollection<PrayGroup> Prays { get; set; }
        public ObservableCollection<PrayGroup> PraysCopied { get; set; }

        public string SearchQuery
        {
            get => searchQuery;
            set => SetProperty(ref searchQuery, value);
        }

        public string NotFoundMessage
        {
            get => notFoundMessage;
            set => SetProperty(ref notFoundMessage, value);
        }

        public bool IsVisibleNotFoundMessage
        {
            get => isVisibleNotFoundMessage;
            set => SetProperty(ref isVisibleNotFoundMessage, value);
        }

        public string NumbersFoundAyahAndSurah
        {
            get => numbersFoundAyahAndSurah;
            set => SetProperty(ref numbersFoundAyahAndSurah, value);
        }

        public Color FoundResultColor
        {
            get => foundResultColor;
            set => SetProperty(ref foundResultColor, value);
        }

        public bool IsExpandAll
        {
            get => isExpandAll;
            set
            {
                SetProperty(ref isExpandAll, value);
            }
        }
        public bool IsVisibleExpandAll
        {
            get => isVisibleExpandAll;
            set
            {
                SetProperty(ref isVisibleExpandAll, value);
            }
        }

        public Command LoadCommand { get; }
        public Command<PrayGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; set; }

        public ICommand ClearSearchCommand
        {
            get;
        }

        //public FindViewModel(bool emptyGroups = false)
        public PrayViewModel()
        {
            Title = "Kumpulan Do'a";
            Prays = new ObservableCollection<PrayGroup>();
            PraysCopied = new ObservableCollection<PrayGroup>();
            //Ayahs = new ObservableCollection<Ayah>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            HeaderTapped = new Command<PrayGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllComand);
            FoundResultColor = Color.Black;
            PrayOneTapped = new Command<Models.Zikrs.Pray>(OnPrayOneTapped, (x) => CanNavigate);
        }

        private void OnExpandAllComand()
        {
            if (IsExpandAll)
            {
                for (int i = 0; i < Prays.Count; i++)
                {
                    Prays[i].Clear();
                }
                IsExpandAll = false;
            }
            else
            {
                for (int i = 0; i < PraysCopied.Count; i++)
                {
                    Prays.Add(PraysCopied[i]);
                }
                IsExpandAll = true;
            }
        }

        private void OnHeaderTapped(PrayGroup prayGroup)
        {
            prayGroup.IsExpand = !prayGroup.IsExpand;

            if (prayGroup.IsExpand)
            {
                var index = Prays.IndexOf(prayGroup);
                var context = PraysCopied[index];
                for (int i = 0; i < context.Count; i++)
                {
                    prayGroup.Add(context[i]);
                }
                prayGroup.ImageHeader = "collapse.png";
            }
            else
            {
                prayGroup.Clear();
                prayGroup.ImageHeader = "expand.png";
            }
        }

        private void ClearSearch()
        {
            if (Prays != null)
            {
                Prays.Clear();
            }

            if (PraysCopied != null)
            {
                PraysCopied.Clear();
            }
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                NotFoundMessage = "";
                IsVisibleNotFoundMessage = false;
                NumbersFoundAyahAndSurah = "";
                FoundResultColor = Color.Gray;

                // Don't search if search query is blank
                //if (string.IsNullOrWhiteSpace(searchQuery))
                //{
                //    return;
                //}

                // Ayahs.Clear();

                var hcsData = await PrayDataService.GetPraysAsync(true);

                
                if (!String.IsNullOrWhiteSpace(SearchQuery))
                {
                    hcsData = hcsData.Where(q => q.Title.Trim().ToLower().Contains(SearchQuery.ToLower())
                        || q.Data.Any(a => a.ArabicLatin.Trim().ToLower().Contains(SearchQuery.ToLower())
                        || q.Data.Any(t => t.TranslateID.Trim().ToLower().Contains(SearchQuery.ToLower())
                        || q.Data.Any(x => x.Tag.Contains(SearchQuery.ToLower())) // TODO: find based tag
                        )));
                }

                Prays.Clear();
                PraysCopied.Clear();
                ObservableCollection<MyQuranIndo.Models.Zikrs.Pray> prayList = new ObservableCollection<Models.Zikrs.Pray>();

                //var surahGroup = ayahs.ToLookup(s => s.SurahID);
                var prayGroup = hcsData.ToList().ToLookup(q => q.ID);
                int index = 0;
                foreach (var hcData in prayGroup)
                {
                    prayList = new ObservableCollection<Models.Zikrs.Pray>();
                    var prayInGroup = hcsData.Where(q => q.ID == hcData.Key).ToList();
                    for (int i = 0; i < prayInGroup.Count; i++)
                    {                        
                        for (int r = 0; r < prayInGroup[i].Data.Count; r++)
                        {
                            prayList.Add(prayInGroup[i].Data[r]);
                        }
                    }

                    Prays.Add(new PrayGroup(hcData.Key, hcsData.ToList()[index].Title, prayList));
                    PraysCopied.Add(new PrayGroup(hcData.Key, hcsData.ToList()[index].Title, prayList));

                    index++;
                }

                if (Prays.Count == 0)
                {
                    NotFoundMessage = Message.MSG_NOT_FOUND_FIND;
                    IsVisibleNotFoundMessage = true;
                    FoundResultColor = Color.Red;
                }
                else
                {
                    FoundResultColor = Color.Black;
                }

                IsVisibleExpandAll = Prays.Count > 0;

                for (int i = 0; i < Prays.Count; i++)
                {
                    Prays[i].Clear();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_FIND
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool IsValidTapped(Models.Zikrs.Pray pray)
        {
            if (pray == null)
            {
                return false;
            }

            // Dont show pop up menu at ta'awudz
            if (pray.ID == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// On Zikr tapped one
        /// </summary>
        /// <param name="pray"></param>
        private async void OnPrayOneTapped(Models.Zikrs.Pray pray)
        {
            if (IsValidTapped(pray))
            {
                CanNavigate = false;
                var oldColor = pray.RowColor;
                pray.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                string result = "";
                string errorMessage = "";
                string action = await ActionHelper.DisplayActionPrayAsync(pray.Title);
                string zikrCopied = "";
                try
                {
                    switch (action)
                    {
                        case ActionHelper.PRAY_SHARE:
                            zikrCopied = ActionHelper.GetPrayToShare(pray, this.Title);
                            await ActionHelper.ShareZikrAsync(zikrCopied);
                            break;
                        //case ActionHelper.PRAY_SHARE_ALL:
                        //    zikrCopied = ActionHelper.GetPraysAllToShare(Prays, this.Title);
                        //    await ActionHelper.ShareZikrAsync(zikrCopied);
                        //    break;
                        case ActionHelper.PRAY_COPY:
                            zikrCopied = ActionHelper.GetPrayToShare(pray, this.Title);
                            await Clipboard.SetTextAsync(zikrCopied);
                            result = ActionHelper.PRAY_COPY + " Berhasil.";
                            break;
                        //case ActionHelper.PRAY_COPY_ALL:
                        //    zikrCopied = ActionHelper.GetPraysAllToShare(Prays, this.Title);
                        //    await Clipboard.SetTextAsync(zikrCopied);
                        //    result = ActionHelper.PRAY_COPY + " Berhasil.";
                        //    break;
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
                    pray.RowColor = oldColor;
                    CanNavigate = true;
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            selectedPray = null;
        }
    }
}
