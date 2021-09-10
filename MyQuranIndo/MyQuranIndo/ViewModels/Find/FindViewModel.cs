using MyQuranIndo.Messages;
using MyQuranIndo.Models;
using MyQuranIndo.Views.Surah;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Models.Finds;
using MyQuranIndo.Models.Qurans;
using System.Windows.Input;

namespace MyQuranIndo.ViewModels.Find
{
    public class FindViewModel : BaseViewModel
    {
        private string searchQuery;
        private string notFoundMessage;
        private bool isVisibleNotFoundMessage;
        private string numbersFoundAyahAndSurah;
        private Color foundResultColor;
        private MyQuranIndo.Models.Finds.Find selectedFind;
        private bool isExpandAll = true;
        private bool isVisibleExpandAll = false;

        //public ObservableCollection<Ayah> Ayahs { get; }
        public ObservableCollection<FindGroup> Finds { get; set; }
        public ObservableCollection<FindGroup> FindsCopy { get; set; }

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

        public MyQuranIndo.Models.Finds.Find SelectedFind
        {
            get => selectedFind;
            set
            {
                SetProperty(ref selectedFind, value);
                OnFindSelected(value);
            }
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
        public Command<MyQuranIndo.Models.Finds.Find> FindTapped { get; }
        public Command<FindGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; set; }

        public ICommand ClearSearchCommand
        {
            get;
        }

        //public FindViewModel(bool emptyGroups = false)
        public FindViewModel()
        {
            Title = "Pencarian Surat / Ayat";
            Finds = new ObservableCollection<FindGroup>();
            FindsCopy = new ObservableCollection<FindGroup>();
            //Ayahs = new ObservableCollection<Ayah>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            FindTapped = new Command<MyQuranIndo.Models.Finds.Find>(OnFindSelected);
            HeaderTapped = new Command<FindGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllComand);
            FoundResultColor = Color.Black;
        }

        private void OnExpandAllComand()
        {
            if (IsExpandAll)
            {
                for (int i = 0; i < Finds.Count; i++)
                {
                    Finds[i].Clear();
                }
                IsExpandAll = false;
            }
            else
            {
                for (int i = 0; i < FindsCopy.Count; i++)
                {
                    Finds.Add(FindsCopy[i]);
                }
                IsExpandAll = true;
            }
        }

        private void OnHeaderTapped(FindGroup findGroup)
        {
            findGroup.IsExpand = !findGroup.IsExpand;

            if (findGroup.IsExpand)
            {
                var index = Finds.IndexOf(findGroup);
                var context = FindsCopy[index];
                for (int i = 0; i < context.Count; i++)
                {
                    findGroup.Add(context[i]);
                }
                findGroup.ImageHeader = "collapse.png";
            }
            else
            {
                findGroup.Clear();
                findGroup.ImageHeader = "expand.png";
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
            if (Finds != null)
            {
                Finds.Clear();
            }

            if (FindsCopy != null)
            {
                FindsCopy.Clear();
            }
        }
        private async void OnFindSelected(MyQuranIndo.Models.Finds.Find find)
        {
            if (find != null)
            {
                var oldColor = find.RowColor;
                find.RowColor = (Color)Application.Current.Resources["SelectedItem"];
                await Shell.Current.GoToAsync($"{nameof(SurahDetailPage)}?{nameof(SurahDetailViewModel.SurahID)}={find.Ayah.SurahID}&{nameof(SurahDetailViewModel.AyahID)}={find.Ayah.ID}");
                find.RowColor = oldColor;
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
                if(string.IsNullOrWhiteSpace(searchQuery))
                {
                    return;
                }

               // Ayahs.Clear();

                var surahs = await SurahDataService.GetSurahNewAsync(true);
                
                var ayahs = await AyahDataService.GetAsync(true);

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
                    ayahs = ayahs.Where(q => q.TranslateIndo.ToLower().Contains(SearchQuery)
                        || q.TextIndo.ToLower().Contains(SearchQuery)).ToList();
                }

                Finds.Clear();
                FindsCopy.Clear();
                ObservableCollection<MyQuranIndo.Models.Finds.Find> findList = new ObservableCollection<Models.Finds.Find>();
                                
                var surahGroup = ayahs.ToLookup(s => s.SurahID);

                foreach (var surah in surahGroup)
                {
                    findList = new ObservableCollection<Models.Finds.Find>();
                    var ayahInSurahGroup = ayahs.Where(q => q.SurahID == surah.Key).ToList();
                    for (int i = 0; i < ayahInSurahGroup.Count; i++)
                    {
                        MyQuranIndo.Models.Finds.Find find = new Models.Finds.Find()
                        {
                            Ayah = ayahInSurahGroup[i],
                            Row = i
                        };
                        findList.Add(find);
                    }
                    Finds.Add(new FindGroup(surah.Key, surahs[surah.Key - 1].NameLatin, findList.Count(), findList));
                    FindsCopy.Add(new FindGroup(surah.Key, surahs[surah.Key - 1].NameLatin, findList.Count(), findList));
                }

                //if (ayahs != null && ayahs.Count > 0)
                //{
                //    int surahID = 0;
                //    for (int i = 0; i < ayahs.Count; i++)
                //    {
                //        if  (ayahs[i].SurahID == ayahs[i + 1].SurahID)
                //        {
                //            MyQuranIndo.Models.Finds.Find find = new Models.Finds.Find()
                //            {
                //                Ayah = ayahs[i],
                //                Row = i
                //            };
                //            findList.Add(find);
                //        }
                //            Finds.Add(new FindGroup(surahID, surahs[surahID - 1].NameLatin, findList.Count(), findList));
                //            FindsCopy.Add(new FindGroup(surahID, surahs[surahID - 1].NameLatin, findList.Count(), findList));
                //            findList = new ObservableCollection<Models.Finds.Find>();
                        
                //        surahID = ayahs[i].SurahID;
                //    }
                //}

                //foreach (var item in surahs)
                //{
                //    ObservableCollection<MyQuranIndo.Models.Finds.Find> findList = new ObservableCollection<Models.Finds.Find>();
                //    int row = 0;
                //    for (int i = 1; i <= item.Value.Text.Count; i++)
                //    {
                //        // filter translation based on searchQuery
                //        if (item.Value.Translation.ID.Text[i].ToLower().Contains(searchQuery))
                //        {
                //            MyQuranIndo.Models.Finds.Find f = new MyQuranIndo.Models.Finds.Find();
                //            f.Ayah = new Ayah();
                //            f.Ayah.Number = i;
                //            f.Ayah.Name = item.Value.Text[i];
                //            f.Ayah.SurahID = item.Value.Number;
                //            f.Ayah.TafsirKemenag = item.Value.Tafsir.TafsirID.TafsirKemenag.Text[i];
                //            f.Ayah.TranslateID = item.Value.Translation.ID.Text[i];
                //            f.Ayah.TextIndo = item.Value.AyahTextIndo[i];
                //            f.Row = row;
                //            row++;
                //            findList.Add(f);
                //        }

                //        // filter transliteration based on searchQUery
                //        if (item.Value.AyahTextIndo[i].ToLower().Contains(searchQuery))
                //        {
                //            MyQuranIndo.Models.Finds.Find f = new MyQuranIndo.Models.Finds.Find();
                //            f.Ayah = new Ayah();
                //            f.Ayah.Number = i;
                //            f.Ayah.Name = item.Value.Text[i];
                //            f.Ayah.SurahID = item.Value.Number;
                //            f.Ayah.TafsirKemenag = item.Value.Tafsir.TafsirID.TafsirKemenag.Text[i];
                //            f.Ayah.TranslateID = item.Value.Translation.ID.Text[i];
                //            f.Ayah.TextIndo = item.Value.AyahTextIndo[i];
                //            f.Row = row;
                //            row++;
                //            findList.Add(f);
                //        }
                //    }

                //    Finds.Add(new FindGroup(item.Value.Number, item.Value.NameLatin, findList.Count(), findList));
                //    FindsCopy.Add(new FindGroup(item.Value.Number, item.Value.NameLatin, findList.Count(), findList));
                //}

                if (Finds.Count == 0)
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
                int totalAyah = Finds.Sum(q => q.NumberOfAyah);
                if (totalAyah > 0)
                {
                    NumbersFoundAyahAndSurah = string.Format("Kata Kunci '{0}' ditemukan, total {1} ayat dalam {2} surat.", SearchQuery, Finds.Sum(q => q.NumberOfAyah), Finds.Count());
                }
                else
                {
                    NumbersFoundAyahAndSurah = string.Format("Kata Kunci '{0}' tidak ditemukan.", SearchQuery);
                }
                IsVisibleExpandAll = Finds.Count > 0;
                
                for (int i = 0; i < Finds.Count; i++)
                {
                    Finds[i].Clear();
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
            //IsBusy = true;
            selectedFind = null;
        }
    }
}
