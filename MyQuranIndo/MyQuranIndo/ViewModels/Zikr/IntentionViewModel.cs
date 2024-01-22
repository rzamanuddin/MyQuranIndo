using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Models.Zikrs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyQuranIndo.ViewModels.Zikr
{
    public class IntentionViewModel : BaseViewModel
    {
        private string searchQuery;
        private string notFoundMessage;
        private bool isVisibleNotFoundMessage;
        private string numbersFoundAyahAndSurah;
        private Color foundResultColor;
        private MyQuranIndo.Models.Zikrs.Intention selectedIntention;
        private bool isExpandAll = true;
        private bool isVisibleExpandAll = false;

        //public ObservableCollection<Ayah> Ayahs { get; }
        public ObservableCollection<IntentionGroup> Intentions { get; set; }
        public ObservableCollection<IntentionGroup> IntentionsCopy { get; set; }

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

        //public MyQuranIndo.Models.Zikrs.Intention SelectedIntention
        //{
        //    get => selectedIntention;
        //    set
        //    {
        //        SetProperty(ref selectedIntention, value);
        //        OnFindSelected(value);
        //    }
        //}
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

        private bool visibleSearchBar = false;
        public bool VisibleSearchBar
        {
            get => visibleSearchBar;
            set => SetProperty(ref visibleSearchBar, value);
        }

        public Command LoadCommand { get; }
        public Command<MyQuranIndo.Models.Finds.Find> FindTapped { get; }
        public Command<IntentionGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; set; }

        public ICommand ClearSearchCommand
        {
            get;
        }

        private readonly int index;
        public Task Initialization { get; private set; }
        public ICommand IntentionOneTapped { get; }

        //public FindViewModel(bool emptyGroups = false)
        public IntentionViewModel(int index)
        {
            if (index == 1)
            {
                Title = "Niat Sholat";
            }
            else
            {
                Title = "Bacaan Sholat";
            }
            Intentions = new ObservableCollection<IntentionGroup>();
            IntentionsCopy = new ObservableCollection<IntentionGroup>();
            //Ayahs = new ObservableCollection<Ayah>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            //FindTapped = new Command<MyQuranIndo.Models.Zikrs.Intention>(OnIntentionSelected);
            HeaderTapped = new Command<IntentionGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllComand);
            FoundResultColor = Color.Black;
            this.index = index;
            Initialization = InitStart();

            IntentionOneTapped = new Command<Intention>(OnIntentionOneTapped, (x) => CanNavigate);
        }

        private async Task InitStart()
        {
            await ExecuteLoadCommand();
        }

        private void OnExpandAllComand()
        {
            if (IsExpandAll)
            {
                for (int i = 0; i < Intentions.Count; i++)
                {
                    Intentions[i].Clear();
                }
                IsExpandAll = false;
            }
            else
            {
                for (int i = 0; i < IntentionsCopy.Count; i++)
                {
                    Intentions.Add(IntentionsCopy[i]);
                }
                IsExpandAll = true;
            }
        }

        private async void OnIntentionOneTapped(Intention intention)
        {
            if (IsValidTapped(intention))
            {
                CanNavigate = false;
                var oldColor = intention.RowColor;
                intention.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                string result = "";
                string errorMessage = "";
                string action = await ActionHelper.DisplayActionIntentionAsync(intention.Title);
                string zikrCopied = "";
                try
                {
                    switch (action)
                    {
                        case ActionHelper.INTENTION_SHARE:
                            zikrCopied = ActionHelper.GetIntentionToShare(intention, this.Title);
                            await ActionHelper.ShareZikrAsync(zikrCopied);
                            break;
                        case ActionHelper.INTENTION_COPY:
                            zikrCopied = ActionHelper.GetIntentionToShare(intention, this.Title);
                            await Clipboard.SetTextAsync(zikrCopied);
                            result = ActionHelper.INTENTION_COPY + " Berhasil.";
                            break;
                        case ActionHelper.INTENTION_COPY_ALL:
                            ObservableCollection<Intention> intentions = new ObservableCollection<Intention>();
                            
                            foreach (var item in Intentions)
                            {
                                foreach (var intentionItem in item.Intentions)
                                {
                                    intentions.Add(intentionItem);
                                }
                            }
                            zikrCopied = ActionHelper.GetIntentionsAllToShare(intentions, this.Title);
                            await Clipboard.SetTextAsync(zikrCopied);
                            result = ActionHelper.INTENTION_COPY_ALL + " Berhasil.";
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
                    intention.RowColor = oldColor;
                    CanNavigate = true;
                }
            }
        }

        private bool IsValidTapped(Intention intention)
        {
            if (intention == null)
            {
                return false;
            }

            return true;
        }

        private void OnHeaderTapped(IntentionGroup intentionGroup)
        {
            intentionGroup.IsExpand = !intentionGroup.IsExpand;

            if (intentionGroup.IsExpand)
            {
                var index = Intentions.IndexOf(intentionGroup);
                var context = IntentionsCopy[index];
                for (int i = 0; i < context.Count; i++)
                {
                    intentionGroup.Add(context[i]);
                }
                intentionGroup.ImageHeader = "collapse.png";
            }
            else
            {
                intentionGroup.Clear();
                intentionGroup.ImageHeader = "expand.png";
            }

            //for (int i = 0; i < Finds.Count; i++)
            //{
            //    if (Finds[i].SurahID == findGroup.SurahID)
            //    {
            //        Finds[i].IsExpand = findGroup.IsExpand;
            //        break;
            //    }
            //}
        }

        private void ClearSearch()
        {
            if (Intentions != null)
            {
                Intentions.Clear();
            }

            if (IntentionsCopy != null)
            {
                IntentionsCopy.Clear();
            }
        }
        //private async void OnIntentionSelected(MyQuranIndo.Models.Zikrs.Intention intention)
        //{
        //    if (intention != null)
        //    {
        //        var oldColor = intention.RowColor;
        //        intention.RowColor = (Color)Application.Current.Resources["SelectedItem"];
        //        await Shell.Current.GoToAsync($"{nameof(SurahDetailPage)}?{nameof(SurahDetailViewModel.SurahID)}={intention.Ayah.SurahID}&{nameof(SurahDetailViewModel.AyahID)}={intention.Ayah.ID}");
        //        intention.RowColor = oldColor;
        //    }
        //}

        public async Task ExecuteLoadCommand()
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

                IEnumerable<Intention> intentions = null;

                if (this.index == 1)
                {
                    intentions = await IntentionDataService.GetIntentions(true);
                }
                else
                {
                    var intentionsList = new List<Intention>();
                    var prayerReads = await PrayerReadDataService.GetPrayerReads(true);
                    foreach (var pr in prayerReads)
                    {
                        var intention = new Intention
                        {
                            Arabic = pr.Arabic,
                            ArabicLatin = pr.ArabicLatin,
                            ID = pr.ID,
                            Title = pr.Title,
                            TranslateID = pr.TranslateID,
                        };

                        intentionsList.Add(intention);
                    }
                    intentions = intentionsList;
                }

                //var ayahs = await AyahDataService.GetAsync(true);

                //var surahs = from s in getSurahs
                //             select s;
                //i ayahs = from a in getAyahs
                //            select a;

                if (!String.IsNullOrWhiteSpace(SearchQuery))
                {
                    // Find in surah and translate
                    //result = quran.Where(q => q.Value.NameLatin.ToLower().Contains(SearchQuery)
                    //    || q.Value.Translation.ID.Text.Any(text => text.Value.ToLower().Contains(searchQuery))
                    //    //|| q.Value.AyahTextIndo.Any(text => text.Value.ToLower().Contains(searchQuery)));
                    //    ).Union(result.Where(q => q.Value.NameLatin.ToLower().Contains(SearchQuery)
                    //            || q.Value.AyahTextIndo.Any(text => text.Value.ToLower().Contains(searchQuery))
                    //            )
                    //    );
                    //surahs = surahs.Where(q => q.NameLatin.ToLower().Contains(SearchQuery));
                    intentions = intentions.Where(q => q.Title.Trim().ToLower().Contains(SearchQuery.ToLower())
                        || q.ArabicLatin.Trim().ToLower().Contains(SearchQuery.ToLower())
                        || q.TranslateID.Trim().ToLower().Contains(SearchQuery.ToLower())
                        );
                }

                Intentions.Clear();
                IntentionsCopy.Clear();
                ObservableCollection<MyQuranIndo.Models.Zikrs.Intention> intentionList = new ObservableCollection<Models.Zikrs.Intention>();

                //var surahGroup = ayahs.ToLookup(s => s.SurahID);
                var intentionGroup = intentions.ToList().ToLookup(q => q.ID);
                int index = 0;
                foreach (var intention in intentionGroup)
                {                    
                    intentionList = new ObservableCollection<Models.Zikrs.Intention>();
                    var intentionInGroup = intentions.Where(q => q.ID == intention.Key).ToList();
                    for (int i = 0; i < intentionInGroup.Count; i++)
                    {
                        MyQuranIndo.Models.Zikrs.Intention intent = new Models.Zikrs.Intention()
                        {
                            Arabic = intentionInGroup[i].Arabic,
                            ArabicLatin = intentionInGroup[i].ArabicLatin,
                            Faedah = intentionInGroup[i].Faedah,
                            ID = intentionInGroup[i].ID,
                            Narrator = intentionInGroup[i].Narrator,
                            Note = intentionInGroup[i].Note,
                            Title = intentionInGroup[i].Title,
                            TranslateID = intentionInGroup[i].TranslateID,
                            //intenti = intentionInGroup[i],
                            RowID = i
                        };
                        intentionList.Add(intent);
                    }
                    
                    Intentions.Add(new IntentionGroup(intention.Key, intentions.ToList()[index].TitleAndNumber, intentionList));
                    IntentionsCopy.Add(new IntentionGroup(intention.Key, intentions.ToList()[index].TitleAndNumber, intentionList));

                    index++;
                }

                if (Intentions.Count == 0)
                {
                    NotFoundMessage = Message.MSG_NOT_FOUND_FIND;
                    IsVisibleNotFoundMessage = true;
                    FoundResultColor = Color.Red;
                }
                else
                {
                    FoundResultColor = Color.Black;
                }
                //NumbersFoundAyahAndSurah = string.Format("Total {0} ayat ditemukan dalam {1} surat.", Finds.Sum(q => q.NumberOfAyah), Finds.Count());
                //int totalAyah = Intentions.Sum(q => q.NumberOfAyah);
                //if (totalAyah > 0)
                //{
                //    NumbersFoundAyahAndSurah = string.Format("Kata Kunci '{0}' ditemukan, total {1} ayat dalam {2} surat.", SearchQuery, Intentions.Sum(q => q.NumberOfAyah), Intentions.Count());
                //}
                //else
                //{
                //    NumbersFoundAyahAndSurah = string.Format("Kata Kunci '{0}' tidak ditemukan.", SearchQuery);
                //}
                IsVisibleExpandAll = Intentions.Count > 0;

                for (int i = 0; i < Intentions.Count; i++)
                {
                    Intentions[i].Clear();
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

        public void OnAppearing()
        {
            IsBusy = true;
            selectedIntention = null;
        }
    }
}

