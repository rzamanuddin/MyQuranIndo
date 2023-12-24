using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using MyQuranIndo.Models.Hadiths;
using System.Linq;
using MyQuranIndo.Messages;
using Xamarin.Forms.Extended;
using static Android.Icu.Util.LocaleData;
using MyQuranIndo.Models.Zikrs;
using Android.Widget;
using static Android.App.DownloadManager;
using System.Net;

namespace MyQuranIndo.ViewModels.Hadiths
{
    [QueryProperty(nameof(Slug), nameof(Slug))]

    public class HadithDetailViewModel : BaseViewModel
    {

        private string searchQuery;
        private string notFoundMessage;
        private bool isVisibleNotFoundMessage;
        private string numbersFoundHadith;
        private Color foundResultColor;
        //private MyQuranIndo.Models.Zikrs.PrayData selectedHadith;
        private bool isExpandAll = true;
        private bool isVisibleExpandAll = false;
        public ICommand HadithOneTapped { get; }

        public ObservableCollection<HadithGroup> Hadiths { get; set; }
        public ObservableCollection<HadithGroup> HadithCopied { get; set; }

        private string slug;
        public string Slug
        {
            get
            {
                return slug;
            }
            set
            {
                slug = value;
                //LoadNarrator(value);
            }
        }

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

        private bool isVisibleFoundMessage;
        public bool IsVisibleFoundMessage
        {
            get => isVisibleFoundMessage;
            set => SetProperty(ref isVisibleFoundMessage, value);
        }

        public string NumbersFoundHadith
        {
            get => numbersFoundHadith;
            set => SetProperty(ref numbersFoundHadith, value);
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
        public Command<HadithGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; set; }

        public ICommand ClearSearchCommand
        {
            get;
        }

        //public FindViewModel(bool emptyGroups = false)
        public HadithDetailViewModel()
        {
            Hadiths = new ObservableCollection<HadithGroup>();
            HadithCopied = new ObservableCollection<HadithGroup>();
            
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            HeaderTapped = new Command<HadithGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllComand);
            FoundResultColor = Color.Black;
            HadithOneTapped = new Command<Hadith>(OnHadithOneTapped, (x) => CanNavigate);
        }

        private void OnExpandAllComand()
        {
            if (IsExpandAll)
            {
                for (int i = 0; i < Hadiths.Count; i++)
                {
                    Hadiths[i].Clear();
                }
                IsExpandAll = false;
            }
            else
            {
                for (int i = 0; i < HadithCopied.Count; i++)
                {
                    Hadiths.Add(HadithCopied[i]);
                }
                IsExpandAll = true;
            }
        }

        private void OnHeaderTapped(HadithGroup hadithGroup)
        {
            hadithGroup.IsExpand = !hadithGroup.IsExpand;

            if (hadithGroup.IsExpand)
            {
                var index = Hadiths.IndexOf(hadithGroup);
                var context = HadithCopied[index];
                for (int i = 0; i < context.Count; i++)
                {
                    hadithGroup.Add(context[i]);
                }
                hadithGroup.ImageHeader = "collapse.png";
            }
            else
            {
                hadithGroup.Clear();
                hadithGroup.ImageHeader = "expand.png";
            }
        }

        private void ClearSearch()
        {
            if (Hadiths != null)
            {
                Hadiths.Clear();
            }

            if (HadithCopied != null)
            {
                HadithCopied.Clear();
            }
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                NotFoundMessage = "";
                IsVisibleNotFoundMessage = false;
                NumbersFoundHadith = "";
                FoundResultColor = Color.Gray;

                var narrator = await HadithDataService.GetNarratorAsync(Slug, true);

                if (narrator != null)
                {
                    Title = $"H.R. {narrator.Name}";

                    int count = narrator.TotalHadith;
                    int pageSize = 300;

                    Hadiths.Clear();
                    HadithCopied.Clear();

                    var haditsResultAPI = new List<HadithResultAPI>();
                    int page = 1;
                    int number = 0;
                    if (!String.IsNullOrWhiteSpace(SearchQuery))
                    {
                        int.TryParse(searchQuery, out number);
                        if (number > 0)
                        {                            
                            page = (number + pageSize - 1) / pageSize;
                            HadithResultAPI result = await HadithDataService.GetHadithResultAPIAsync(narrator.Slug, page, pageSize, true);
                            var query = result.Items.Where(q => q.Number == number);
                            result.Items = query.ToList();
                            haditsResultAPI.Add(result);
                            goto loadData;
                        }
                    }

                    page = 1;
                    for (int i = 1; i <= count; i++)
                    {
                        if (i % pageSize == 0)
                        {
                            HadithResultAPI result = await HadithDataService.GetHadithResultAPIAsync(narrator.Slug, page, pageSize, true);
                            if (!String.IsNullOrWhiteSpace(SearchQuery))
                            {
                                var query = Enumerable.Empty<Hadith>();
                                int.TryParse(searchQuery, out number);
                                //if (number > 0)
                                //{
                                //    //query = result.Items.Where(q => q.Number == number);
                                //}
                                //else
                                //{
                                //result.Items.RemoveAll(q => !q.Id.Trim().ToLower().Contains(searchQuery.Trim().ToLower()));
                                query = result.Items.Where(q => q.Id.Trim().ToLower().Contains(searchQuery.Trim().ToLower()));
                                //}
                                result.Items = query.ToList();
                            }
                            haditsResultAPI.Add(result);
                            page++;
                        }
                    }

                    var lastHaditsAPI = await HadithDataService.GetHadithResultAPIAsync(narrator.Slug, page, pageSize, true);
                    if (!String.IsNullOrWhiteSpace(SearchQuery))
                    {
                        IEnumerable<Hadith> query = Enumerable.Empty<Hadith>();
                        number = 0;
                        int.TryParse(searchQuery, out number);
                        if (number > 0)
                        {

                            query = lastHaditsAPI.Items.Where(q => q.Number == number);
                        }
                        else
                        {
                            //lastHaditsAPI.Items.RemoveAll(q => !q.Id.Trim().ToLower().Contains(searchQuery.Trim().ToLower()));
                            query = lastHaditsAPI.Items.Where(q => q.Id.Trim().ToLower().Contains(searchQuery.Trim().ToLower()));
                        }

                        lastHaditsAPI.Items = query.ToList();
                    }
                    haditsResultAPI.Add(lastHaditsAPI);

                    loadData:
                    int lastNumber = 0;
                    count = haditsResultAPI.Count;
                    int currentPage = 1;
                    for (int i = 0; i < count; i++)
                    {
                        var hdsData = haditsResultAPI[i];
                        ObservableCollection<Hadith> hdList = new ObservableCollection<Hadith>();

                        var hdGroup = hdsData.Items.ToLookup(q => currentPage);

                        int index = 0;
                        foreach (var hadithData in hdGroup)
                        {
                            hdList = new ObservableCollection<Hadith>();
                            var hadithInGroup = hdsData.Items.Where(q => currentPage == hadithData.Key).ToList();
                            for (int g = 0; g < hadithInGroup.Count; g++)
                            {
                                hdList.Add(hadithInGroup[g]);
                            }

                            string groupTitle = "";
                            if (hdsData.Items.FirstOrDefault().Number == hdsData.Items.LastOrDefault().Number)
                            {
                                groupTitle = hdsData.Items.FirstOrDefault().Number.ToString();
                            }
                            else
                            {
                                groupTitle = $"{hdsData.Items.FirstOrDefault().Number} - {hdsData.Items.Last().Number}";
                            }
                            Hadiths.Add(new HadithGroup(hadithData.Key, groupTitle, hdList));
                            HadithCopied.Add(new HadithGroup(hadithData.Key, groupTitle, hdList));

                            index++;
                        }
                        lastNumber = i;
                        currentPage++;
                    }

                    if (string.IsNullOrWhiteSpace(SearchQuery))
                    {
                        IsVisibleFoundMessage = false;
                    }
                    else
                    {
                        IsVisibleFoundMessage = true;
                    }

                    if (haditsResultAPI == null || haditsResultAPI.Count == 0 || haditsResultAPI.Sum(x => x.Items.Count) == 0)
                    {
                        NotFoundMessage = Message.MSG_NOT_FOUND_FIND;
                        IsVisibleNotFoundMessage = true;
                        FoundResultColor = Color.Red;
                        NumbersFoundHadith = $"Kata Kunci '{SearchQuery}' tidak ditemukan.";
                    }
                    else
                    {
                        FoundResultColor = Color.Black;
                        NumbersFoundHadith = $"Kata Kunci '{SearchQuery}' ditemukan, total {haditsResultAPI.Sum(x => x.Items.Count)} hadis.";
                    }
                    
                    IsVisibleExpandAll = Hadiths.Count > 0;

                    for (int i = 0; i < Hadiths.Count; i++)
                    {
                        Hadiths[i].Clear();
                    }
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

        private bool IsValidTapped(Hadith hadith)
        {
            if (hadith == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// On Zikr tapped one
        /// </summary>
        /// <param name="hadith"></param>
        private async void OnHadithOneTapped(Hadith hadith)
        {
            if (IsValidTapped(hadith))
            {
                CanNavigate = false;
                var oldColor = hadith.RowColor;
                hadith.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                string result = "";
                string errorMessage = "";
                var narrator = await HadithDataService.GetNarratorAsync(Slug);
                string action = await ActionHelper.DisplayActionHadithAsync(narrator.Name, hadith.Number);
                string hadithCopied = ActionHelper.GetHadithToShare(hadith, narrator.Name);
                try
                {
                    switch (action)
                    {
                        case ActionHelper.HADITH_SHARE:
                            await ActionHelper.ShareHadithAsync(hadithCopied);
                            break;
                        case ActionHelper.HADITH_COPY:
                            await Clipboard.SetTextAsync(hadithCopied);
                            result = ActionHelper.HADITH_COPY + " Berhasil";
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
                    hadith.RowColor = oldColor;
                    CanNavigate = true;
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            //selectedHadith = null;
        }
    }
}


    //[QueryProperty(nameof(Slug), nameof(Slug))]
    //[QueryProperty(nameof(Number), nameof(Number))]
    //public class HadithDetailViewModel : BaseViewModel, IHasListViewViewModel //TODO:
    //{
    //    private string result = "", errorMessage = "";

    //    private string slug;
    //    private Narrator narrator;
    //    private string number;
    //    private ICommand _searchCommand;


    //    public Narrator Narrator
    //    {
    //        get => narrator;
    //        set => SetProperty(ref narrator, value);
    //    }

    //    public ObservableCollection<Hadith> Hadiths { get; }

    //    public string ID { get; set; }
    //    public string Slug
    //    {
    //        get
    //        {
    //            return slug;
    //        }
    //        set
    //        {
    //            slug = value;
    //            LoadNarrator(value);
    //        }
    //    }

    //    public string Number
    //    {
    //        get
    //        {
    //            return number;
    //        }
    //        set
    //        {
    //            number = value;
    //            LoadNarrator(Slug, value);
    //        }
    //    }

    //    private bool visibleSearchBar = false;
    //    public bool VisibleSearchBar
    //    {
    //        get => visibleSearchBar;
    //        set => SetProperty(ref visibleSearchBar, value);
    //    }

    //    private string subTitle;
    //    public string SubTitle
    //    {
    //        get => subTitle;
    //        set => SetProperty(ref subTitle, value);
    //    }

    //    public IHasListView View
    //    {
    //        get; set;
    //    }

    //    public Command LoadCommand { get; private set; }
    //    public ICommand HadithOneTapped { get; }
    //    public Command SettingTapped { get; }
    //    public ICommand GoBackCommand { get; }

    //    public HadithDetailViewModel()
    //    {
    //        Title = "Hadis";
    //        Hadiths = new ObservableCollection<Hadith>();

    //        //Hadiths = new InfiniteScrollCollection<Hadith>
    //        //{
    //        //    OnLoadMore = async () =>
    //        //    {
    //        //        // load the next page
    //        //        var page = Hadiths.Count / 5;
    //        //        var results = await HadithDataService.GetHadithResultAPIAsync(slug, page + 1, 5);

    //        //        // return the items that need to be added
    //        //        return results.Items;
    //        //    }
    //        //};
    //        //Hadiths.LoadMoreAsync();

    //        //Hadiths = new InfiniteScrollCollection<Hadith>
    //        //{
    //        //    OnLoadMore = async () =>
    //        //    {
    //        //        // load the next page
    //        //        var page = Hadiths.Count / 5;
    //        //        var results = await HadithDataService.GetHadithResultAPIAsync(slug, page, 5);// DataItems.GetItemsAsync(page, PageSize);
    //        //        IsBusy = false;
    //        //        return results.Items;
    //        //    }
    //        //};
    //        //// load the initial data
    //        //LoadDataAsync();

    //        //Hadiths = new ObservableCollection<Hadith>();
    //        LoadCommand = new Command(async () => await ExecuteLoadCommand());
    //        HadithOneTapped = new Command<Hadith>(OnHadithOneTapped);
    //        SettingTapped = new Command(OnSettingTapped);
    //        GoBackCommand = new Command(async () => await OnBackTapped());
    //    }

    //    private async Task LoadDataAsync()
    //    {
    //        var results = await HadithDataService.GetHadithResultAPIAsync(slug, 0, 5); 
    //        //DataItems.GetItemsAsync(pageIndex: 0, pageSize: PageSize);

    //        //Hadiths.AddRange(results.Items);
    //        //ListsingleItems.ItemsSource = Items;

    //    }

    //    private async void OnSettingTapped()
    //    {
    //        await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
    //    }

    //    public async Task OnBackTapped()
    //    {
    //        await Shell.Current.GoToAsync("..");
    //    }
    //    public void ScrollToItem(Hadith hadith, bool isAnimated = false)
    //    {
    //        View.ListView.ScrollTo(hadith, position: ScrollToPosition.Start, isAnimated);
    //    }

    //    public async void LoadNarrator(string slug, string number = null)
    //    {
    //        try
    //        {
    //            if (string.IsNullOrWhiteSpace(slug) || slug == "0")
    //            {
    //                return;
    //            }

    //            var narrator = await HadithDataService.GetNarratorAsync(slug);

    //            Narrator = narrator;
    //            Title = $"Hadis {narrator.Name}";
    //            SubTitle = narrator.DisplaySubTitle;

    //            if (!string.IsNullOrEmpty(number) && number != "0" && number != "1")
    //            {
    //                ScrollToItem(Hadiths.FirstOrDefault(q => q.Number == Convert.ToInt32(number)));
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_LOAD_HADITH
    //                + Environment.NewLine + ex.Message, Message.MSG_OK);
    //        }
    //        finally
    //        {
    //            IsBusy = false;
    //        }
    //    }

    //    public async Task ExecuteLoadCommand()
    //    {
    //        try
    //        {
    //            Hadiths.Clear();

    //            var slug = Slug;
    //            var hadiths = await HadithDataService.GetHadithResultAPIAsync(slug, 1, 300);

    //            if (hadiths != null && hadiths.Items.Count > 0)
    //            {
    //                for (int i = 0; i < hadiths.Items.Count; i++)
    //                {
    //                    Hadiths.Add(hadiths.Items[i]);
    //                }
    //            }

    //            if (!string.IsNullOrEmpty(this.Number) && this.Number != "0" && this.Number != "1")
    //            {
    //                ScrollToItem(Hadiths.FirstOrDefault((q => q.Number == Convert.ToInt32(this.Number))));
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR
    //                , Message.MSG_FAIL_GET_HADITH + Environment.NewLine + ex.Message, Message.MSG_OK);
    //        }
    //        finally
    //        {
    //            IsBusy = false;
    //        }
    //    }

    //    private bool IsValidTapped(Hadith hadith)
    //    {
    //        if (hadith == null)
    //        {
    //            return false;
    //        }

    //        return true;
    //    }


    //    private async void SearchData(string text)
    //    {
    //        try
    //        {
    //            if (String.IsNullOrWhiteSpace(text))
    //            {
    //                ScrollToItem(Hadiths[0]);
    //            }
    //            else
    //            {
    //                int number = 0;
    //                int.TryParse(text, out number);
    //                string errorMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

    //                if (number > 0)
    //                {
    //                    if (number > 0)
    //                    {
    //                        ScrollToItem(Hadiths.FirstOrDefault(q => q.Number == number));
    //                    }
    //                    else
    //                    {
    //                        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
    //                    }
    //                }
    //                else
    //                {
    //                    var h = Hadiths.FirstOrDefault(q => q.Id.ToLower().Contains(text));

    //                    if (h != null)
    //                    {
    //                        ScrollToItem(h);
    //                    }
    //                    else
    //                    {
    //                        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_FIND_HADITH + Environment.NewLine + ex.Message, Message.MSG_OK);
    //        }
    //    }

    //    public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
    //    {
    //        SearchData(text);
    //    }));

    //    private string getSlug()
    //    {
    //        return Slug;
    //    }

    //    private async void OnHadithOneTapped(Hadith hadith)
    //    {
    //        if (IsValidTapped(hadith))
    //        {
    //            var oldColor = hadith.RowColor;
    //            hadith.RowColor = (Color)Application.Current.Resources["SelectedItem"];

    //            result = "";
    //            errorMessage = "";
    //            var narrator = await HadithDataService.GetNarratorAsync(Slug);
    //            string action = await ActionHelper.DisplayActionHadithAsync(narrator.Name, hadith.Number);
    //            string ayahCopied = ActionHelper.GetHadithToShare(hadith, narrator.Name);

    //            try
    //            {
    //                switch (action)
    //                {
    //                    case ActionHelper.HADITH_SHARE:
    //                        await ActionHelper.ShareHadithAsync(ayahCopied);
    //                        break;
    //                    case ActionHelper.HADITH_COPY:
    //                        await Clipboard.SetTextAsync(ayahCopied);
    //                        result = ActionHelper.HADITH_COPY + " Berhasil";
    //                        break;
    //                    default:
    //                        break;
    //                }

    //                if (!string.IsNullOrWhiteSpace(this.result))
    //                {
    //                    ToastService.Show(this.result);
    //                }
    //                if (!string.IsNullOrWhiteSpace(this.errorMessage))
    //                {
    //                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, this.errorMessage, Message.MSG_OK);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine
    //                    + ex.Message, Message.MSG_OK);
    //            }
    //            finally
    //            {
    //                hadith.RowColor = oldColor;
    //            }
    //        }
    //    }

    //    //private async Task SetLastReadAyah(Ayah ayah)
    //    //{

    //    //    try
    //    //    {
    //    //        bool isConfirm = true;
    //    //        if (Preferences.ContainsKey(MenuKey.LAST_SURAH))
    //    //        {
    //    //            //string test = Preferences.Get(Key.LAST_SURAH, "");
    //    //            int lastSurah = Preferences.Get(MenuKey.LAST_SURAH, 0);
    //    //            int lastAyah = Preferences.Get(MenuKey.LAST_AYAH, 0);
    //    //            var lastSurahRead = await SurahDataService.GetSurahAsync(lastSurah);

    //    //            string lastRead = string.Format("Ayat terakhir dibaca adalah Q.S. {0}. {1} Ayat {2}, akan diganti menjadi Q.S. {3}. {4} Ayat {5}"
    //    //                , lastSurah
    //    //                , lastSurahRead.NameLatin
    //    //                , lastAyah
    //    //                , ayah.SurahID
    //    //                , Surah.NameLatin
    //    //                , ayah.ID);

    //    //            isConfirm = await App.Current.MainPage.DisplayAlert(Message.CONFIRM, lastRead, Message.MSG_YES, Message.MSG_NO);
    //    //        }

    //    //        if (isConfirm)
    //    //        {
    //    //            Preferences.Set(MenuKey.LAST_AYAH, ayah.ID);
    //    //            Preferences.Set(MenuKey.LAST_SURAH, ayah.SurahID);

    //    //            //await SecureStorage.SetAsync(Key.LAST_SURAH, Surah.ID.ToString());
    //    //            //await SecureStorage.SetAsync(Key.LAST_AYAH, ayah.Number.ToString());
    //    //            result = string.Format("Ayat terakhir dibaca berhasil diubah menjadi Q.S. {0}. {1} Ayat {2}."
    //    //                , getSurahID(), Surah.NameLatin, ayah.ID);
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        errorMessage = "Error" + Environment.NewLine + ex.Message;
    //    //    }
    //    //}

    //    //private void SetBookmarkAyah(Ayah ayah)
    //    //{
    //    //    result = "";
    //    //    errorMessage = "";
    //    //    try
    //    //    {
    //    //        List<Bookmark> bookmarks = new List<Bookmark>();
    //    //        if (Preferences.ContainsKey(MenuKey.BOOKMARK))
    //    //        {
    //    //            List<Bookmark> getBookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(Preferences.Get(MenuKey.BOOKMARK, null));
    //    //            bookmarks.AddRange(getBookmarks);
    //    //        }
    //    //        for (int i = 0; i < bookmarks.Count; i++)
    //    //        {
    //    //            if (bookmarks[i].SurahID == ayah.SurahID && bookmarks[i].AyahID == ayah.ID)
    //    //            {
    //    //                bookmarks.RemoveAt(i);
    //    //            }
    //    //        }
    //    //        Bookmark b = new Bookmark();
    //    //        b.AyahID = ayah.ID;
    //    //        b.CreatedDate = DateTime.Now;
    //    //        b.SurahID = ayah.SurahID;
    //    //        b.SurahNameLatin = Surah.NameLatin;
    //    //        b.Row = bookmarks.Count + 1;

    //    //        bookmarks.Add(b);
    //    //        Preferences.Set(MenuKey.BOOKMARK, JsonConvert.SerializeObject(bookmarks));

    //    //        result = string.Format("Q.S {0}:{1} berhasil di bookmark."
    //    //            , getSurahID(), ayah.ID);
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        errorMessage = "Error" + Environment.NewLine + ex.Message;
    //    //    }
    //    //}

    //    public void OnAppearing()
    //    {
    //        IsBusy = true;
    //        //SelectedSurah = null;
    //    }
    //}

//}
