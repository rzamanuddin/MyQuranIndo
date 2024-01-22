using MyQuranIndo.Models.Hadiths;
using MyQuranIndo.Views.Hadiths;
using MyQuranIndo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MyQuranIndo.Messages;
using System.Linq;
using System.Runtime.InteropServices;
using MyQuranIndo.Helpers;
using Xamarin.Essentials;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Views.Surah;

namespace MyQuranIndo.ViewModels.Hadiths
{
    public class HadithCategoriesViewModel : BaseViewModel
    {
        private string searchQuery;
        private string notFoundMessage;
        private bool isVisibleNotFoundMessage;
        private string numbersFoundAyahAndSurah;
        private Color foundResultColor;
        private HadithCategoryData selectedHC;
        private bool isExpandAll = true;
        private bool isVisibleExpandAll = false;
        public ICommand HCOneTapped { get; }

        public ObservableCollection<HadithCategoryGroup> HCS { get; set; }
        public ObservableCollection<HadithCategoryGroup> HCSCopied { get; set; }

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
        public Command<HadithCategoryGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; set; }

        public ICommand ClearSearchCommand
        {
            get;
        }

        //public FindViewModel(bool emptyGroups = false)
        public HadithCategoriesViewModel()
        {
            Title = "Kumpulan Hadis";
            HCS = new ObservableCollection<HadithCategoryGroup>();
            HCSCopied = new ObservableCollection<HadithCategoryGroup>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            HeaderTapped = new Command<HadithCategoryGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllComand);
            FoundResultColor = Color.Black;
            HCOneTapped = new Command<HadithCategory>(OnHadithCategoryOneTapped, (x) => CanNavigate);
        }

        private void OnExpandAllComand()
        {
            if (IsExpandAll)
            {
                for (int i = 0; i < HCS.Count; i++)
                {
                    HCS[i].Clear();
                }
                IsExpandAll = false;
            }
            else
            {
                for (int i = 0; i < HCSCopied.Count; i++)
                {
                    HCS.Add(HCSCopied[i]);
                }
                IsExpandAll = true;
            }
        }

        private void OnHeaderTapped(HadithCategoryGroup prayGroup)
        {
            prayGroup.IsExpand = !prayGroup.IsExpand;

            if (prayGroup.IsExpand)
            {
                var index = HCS.IndexOf(prayGroup);
                var context = HCSCopied[index];
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
            if (HCS != null)
            {
                HCS.Clear();
            }

            if (HCSCopied != null)
            {
                HCSCopied.Clear();
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

                var hcDatas = await HadithDataService.GetCategoriesAsync(true);

                if (!String.IsNullOrWhiteSpace(SearchQuery))
                {
                    hcDatas = hcDatas.Where(q => q.Title.Trim().ToLower().Contains(SearchQuery.ToLower())
                        || q.Data.Any(a => a.Title.Trim().ToLower().Contains(SearchQuery.ToLower())));
                        
                }

                HCS.Clear();
                HCSCopied.Clear();
                ObservableCollection<HadithCategory> hcLists = new ObservableCollection<HadithCategory>();

                //var surahGroup = ayahs.ToLookup(s => s.SurahID);
                var hcGroups = hcDatas.ToList().ToLookup(q => q.ID);
                int index = 0;
                foreach (var prayData in hcGroups)
                {
                    hcLists = new ObservableCollection<HadithCategory>();
                    var hcInGroups = hcDatas.Where(q => q.ID == prayData.Key).ToList();
                    for (int i = 0; i < hcInGroups.Count; i++)
                    {
                        for (int r = 0; r < hcInGroups[i].Data.Count; r++)
                        {
                            hcLists.Add(hcInGroups[i].Data[r]);
                        }
                    }

                    int totHadith = hcDatas.ToList()[index].TotalHadith;//hcDatas.ToList()[index].Data.Sum(x => x.TotalHadith);
                    string titleHeader = hcDatas.ToList()[index].Title;

                    HCS.Add(new HadithCategoryGroup(prayData.Key, titleHeader, totHadith, hcLists));
                    HCSCopied.Add(new HadithCategoryGroup(prayData.Key, titleHeader, totHadith, hcLists));

                    index++;
                }

                if (HCS.Count == 0)
                {
                    NotFoundMessage = Message.MSG_NOT_FOUND_FIND;
                    IsVisibleNotFoundMessage = true;
                    FoundResultColor = Color.Red;
                }
                else
                {
                    FoundResultColor = Color.Black;
                }

                IsVisibleExpandAll = HCS.Count > 0;

                for (int i = 0; i < HCS.Count; i++)
                {
                    HCS[i].Clear();
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

        private bool IsValidTapped(HadithCategory hc)
        {
            if (hc == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// On Zikr tapped one
        /// </summary>
        /// <param name="hc"></param>
        private async void OnHadithCategoryOneTapped(HadithCategory hc)
        {
            if (IsValidTapped(hc))
            {
                CanNavigate = false;
                var oldColor = hc.RowColor;
                hc.RowColor = (Color)Application.Current.Resources["SelectedItem"];
                try
                {
                    await Shell.Current.GoToAsync($"{nameof(HadithCategoryDetailPage)}?{nameof(HadithCategoryDetailViewModel.CategoryID)}={hc.ID.ToString()}");                    
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine
                        + ex.Message, Message.MSG_OK);
                }
                finally
                {
                    hc.RowColor = oldColor;
                    CanNavigate = true;
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            selectedHC = null;
        }
    }
}
