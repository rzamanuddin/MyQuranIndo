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
using MyQuranIndo.Messages;
using System.Linq;

namespace MyQuranIndo.ViewModels.Hadiths
{
    [QueryProperty(nameof(CategoryID), nameof(CategoryID))]
    [QueryProperty(nameof(HadithID), nameof(HadithID))]
    public class HadithCategoryDetailViewModel : BaseViewModel, IHasListViewViewModel
    {
        private string result = "", errorMessage = "";

        private string categoryID;
        private HadithCategory hc;
        private string subTitle;
        //private string hadithID;
        private ICommand _searchCommand;
              

        public HadithCategory HadithCategory
        {
            get => hc;
            set => SetProperty(ref hc, value);
        }

        public string SubTitle
        {
            get => subTitle;
            set => SetProperty(ref subTitle, value);
        }

        public ObservableCollection<HadithCategoryDetail> HCDS { get; set; }

        public string ID { get; set; }
        public string CategoryID
        {
            get
            {
                return categoryID;
            }
            set
            {
                categoryID = value;
                LoadHadithCategory(value);
            }
        }

        private string hadithID;
        public string HadithID
        {
            get
            {
                return hadithID;
            }
            set
            {
                hadithID = value;
                LoadHadithCategory(CategoryID, value);
            }
        }

        private bool visibleSearchBar = false;
        public bool VisibleSearchBar
        {
            get => visibleSearchBar;
            set => SetProperty(ref visibleSearchBar, value);
        }

        public IHasListView View
        {
            get; set;
        }

        public Command LoadCommand { get; private set; }
        public ICommand HCDOneTapped { get; }
        public ICommand GoBackCommand { get; }

        public HadithCategoryDetailViewModel()
        {
            Title = "Surah";
            HCDS = new ObservableCollection<HadithCategoryDetail>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            HCDOneTapped = new Command<HadithCategoryDetail>(OnHCDOneTapped);
            GoBackCommand = new Command(async () => await OnBackTapped());
        }

        public async Task OnBackTapped()
        {
            await Shell.Current.GoToAsync("..");
        }
        public void ScrollToItem(HadithCategoryDetail hcd, bool isAnimated = false)
        {
            View.ListView.ScrollTo(hcd, position: ScrollToPosition.Start, isAnimated);
        }

        public async void LoadHadithCategory(string categoryID, string hadithID = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryID) || categoryID == "0")
                {
                    return;
                }

                int catID = Convert.ToInt32(categoryID);
                var hc = await HadithDataService.GetCategoryAsync(catID);

                HadithCategory = hc;
                ID = hc.ID.ToString();
                SubTitle = hc.DisplaySubTitle;
                Title = $"{hc.DisplaySubTitle}, {hc.Description}";

                if (!string.IsNullOrEmpty(hadithID) && hadithID != "0" && hadithID != "1")
                {
                    ScrollToItem(HCDS.FirstOrDefault(q => q.ID == Convert.ToInt32(hadithID)));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_LOAD_HADITH
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
                HCDS.Clear();

                int categoryID = 0;
                int.TryParse(CategoryID, out categoryID);
                var hcds = await HadithDataService.GetCategoryDetailAsync(categoryID);

                for (int i = 0; i < hcds.Data.Count; i++)
                {
                    HCDS.Add(hcds.Data[i]);
                }

                if (!string.IsNullOrEmpty(this.HadithID) && this.HadithID != "0" && this.HadithID != "1")
                {
                    ScrollToItem(HCDS.FirstOrDefault(q => q.ID == Convert.ToInt32(this.HadithID)));
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

        private bool IsValidTapped(HadithCategoryDetail hcd)
        {
            if (hcd == null)
            {
                return false;
            }

            // Dont show pop up menu at bismillah
            if (hcd.ID == 0)
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
                    ScrollToItem(HCDS[0]);
                }
                else
                {
                    int hadithID = 0;
                    int.TryParse(text, out hadithID);
                    string errorMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);

                    if (hadithID > 0)
                    {
                        if (hadithID > 0 && hadithID <= HCDS.Count)
                        {
                            ScrollToItem(HCDS.FirstOrDefault(q => q.ID == hadithID));
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, errorMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var hcd = HCDS.FirstOrDefault(q => q.Title.Contains(text));

                        if (hcd != null)
                        {
                            ScrollToItem(hcd);
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
        
        private async void OnHCDOneTapped(HadithCategoryDetail hcd)
        {
            //if (IsValidTapped(hcd))
            //{
            //    var oldColor = hcd.RowColor;
            //    hcd.RowColor = (Color)Application.Current.Resources["SelectedItem"];

            //    result = "";
            //    errorMessage = "";
            //    var surah = await SurahDataService.GetSurahAsync(hcd.SurahID);
            //    string action = await ActionHelper.DisplayActionAyahAsync(hcd.SurahID, hcd.AyahID, surah.NameLatin);
            //    string ayahCopied = ActionHelper.GetAyahToShare(hcd, HadithCategory.NameLatin);
            //    try
            //    {
            //        switch (action)
            //        {
            //            case ActionHelper.AYAH_SHARE:
            //                await ActionHelper.ShareAyahAsync(ayahCopied);
            //                break;
            //            case ActionHelper.AYAH_COPY:
            //                await Clipboard.SetTextAsync(ayahCopied);
            //                result = ActionHelper.AYAH_COPY + " Berhasil";
            //                break;
            //            //case ActionHelper.AYAH_BOOKMARK:
            //            //    //SetBookmarkAyah(ayah);
            //            //    ActionHelper.BookmarkAyah(hcd, HadithCategory.NameLatin, out result, out errorMessage);
            //            //    break;
            //            //case ActionHelper.AYAH_LAST_READ:
            //            //    //await SetLastReadAyah(ayah);
            //            //    var lastRead = await ActionHelper.SetLastReadAyahAsync(hcd, HadithCategory.NameLatin, SurahDataService);
            //            //    result = lastRead.Item1;
            //            //    errorMessage = lastRead.Item2;
            //            //    break;
            //            default:
            //                break;
            //        }

            //        if (!string.IsNullOrWhiteSpace(this.result))
            //        {
            //            //await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, this.result, Message.MSG_OK);
            //            ToastService.Show(this.result);
            //        }
            //        if (!string.IsNullOrWhiteSpace(this.errorMessage))
            //        {
            //            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, this.errorMessage, Message.MSG_OK);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine
            //            + ex.Message, Message.MSG_OK);
            //    }
            //    finally
            //    {
            //        hcd.RowColor = oldColor;
            //    }
            //}
        }
        

        public void OnAppearing()
        {
            IsBusy = true;
            //SelectedSurah = null;
        }
    }
}