using MyQuranIndo.Messages;
using MyQuranIndo.Views.Surah;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Models.Finds;
using System.Windows.Input;
using Xamarin.Essentials;

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

        public ObservableCollection<FindGroup> Finds { get; }
        public ObservableCollection<FindGroup> FindsCopy { get; }

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
            set => SetProperty(ref isExpandAll, value);
        }

        public bool IsVisibleExpandAll
        {
            get => isVisibleExpandAll;
            set => SetProperty(ref isVisibleExpandAll, value);
        }

        public Command LoadCommand { get; }
        public Command<MyQuranIndo.Models.Finds.Find> FindTapped { get; }
        public Command<FindGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; set; }
        public ICommand ClearSearchCommand { get; }

        public FindViewModel()
        {
            Title = "Pencarian Surat / Ayat";
            Finds = new ObservableCollection<FindGroup>();
            FindsCopy = new ObservableCollection<FindGroup>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            FindTapped = new Command<MyQuranIndo.Models.Finds.Find>(OnFindSelected, (x) => CanNavigate);
            HeaderTapped = new Command<FindGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllCommand);
            FoundResultColor = Color.Black;
        }

        private void OnExpandAllCommand()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (IsExpandAll)
                {
                    foreach (var group in Finds)
                    {
                        group.Clear();
                    }
                    IsExpandAll = false;
                }
                else
                {
                    Finds.Clear();
                    foreach (var group in FindsCopy)
                    {
                        Finds.Add(group);
                    }
                    IsExpandAll = true;
                }
            });
        }

        private void OnHeaderTapped(FindGroup findGroup)
        {
            if (findGroup == null) return;

            findGroup.IsExpand = !findGroup.IsExpand;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (findGroup.IsExpand)
                {
                    var index = Finds.IndexOf(findGroup);
                    if (index >= 0 && index < FindsCopy.Count)
                    {
                        var context = FindsCopy[index];
                        findGroup.Clear();
                        foreach (var item in context)
                        {
                            findGroup.Add(item);
                        }
                    }
                    findGroup.ImageHeader = GetCollapsImageHeaderName();
                }
                else
                {
                    findGroup.Clear();
                    findGroup.ImageHeader = GetExpandImageHeaderName();
                }
            });
        }

        private void ClearSearch()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Finds.Clear();
                FindsCopy.Clear();
            });
        }

        private async void OnFindSelected(MyQuranIndo.Models.Finds.Find find)
        {
            if (find?.Ayah != null)
            {
                CanNavigate = false;
                var oldColor = find.RowColor;
                find.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                await Shell.Current.GoToAsync($"{nameof(SurahDetailPage)}?{nameof(SurahDetailViewModel.SurahID)}={find.Ayah.SurahID}&{nameof(SurahDetailViewModel.AyahID)}={find.Ayah.ID}");

                find.RowColor = oldColor;
                CanNavigate = true;
            }
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                NotFoundMessage = string.Empty;
                IsVisibleNotFoundMessage = false;
                NumbersFoundAyahAndSurah = string.Empty;
                FoundResultColor = Color.Gray;

                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    return;
                }

                string keyword = searchQuery.Trim().ToLower();

                var surahs = await SurahDataService.GetSurahNewAsync(true);
                var ayahs = await AyahDataService.GetAsync(true);

                if (ayahs != null)
                {
                    ayahs = ayahs.Where(q =>
                        (q.TranslateIndo != null && q.TranslateIndo.ToLower().Contains(keyword)) ||
                        (q.TextIndo != null && q.TextIndo.ToLower().Contains(keyword))
                    ).ToList();
                }

                var tempFinds = new List<FindGroup>();
                var tempFindsCopy = new List<FindGroup>();

                if (ayahs != null && ayahs.Count > 0 && surahs != null)
                {
                    var surahGroup = ayahs.ToLookup(s => s.SurahID);

                    foreach (var surah in surahGroup)
                    {
                        var findList = new ObservableCollection<Models.Finds.Find>();
                        var ayahInSurahGroup = ayahs.Where(q => q.SurahID == surah.Key).ToList();

                        for (int i = 0; i < ayahInSurahGroup.Count; i++)
                        {
                            var find = new Models.Finds.Find
                            {
                                Ayah = ayahInSurahGroup[i],
                                RowID = i
                            };
                            findList.Add(find);
                        }

                        if (surah.Key - 1 >= 0 && surah.Key - 1 < surahs.Count)
                        {
                            string surahName = surahs[surah.Key - 1].NameLatin;
                            tempFinds.Add(new FindGroup(surah.Key, surahName, findList.Count, findList));
                            tempFindsCopy.Add(new FindGroup(surah.Key, surahName, findList.Count, findList));
                        }
                    }
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Finds.Clear();
                    FindsCopy.Clear();

                    foreach (var item in tempFinds) Finds.Add(item);
                    foreach (var item in tempFindsCopy) FindsCopy.Add(item);

                    int totalAyah = Finds.Sum(q => q.NumberOfAyah);
                    if (totalAyah > 0)
                    {
                        NumbersFoundAyahAndSurah = string.Format("Kata Kunci '{0}' ditemukan, total {1} ayat dalam {2} surat.", SearchQuery, totalAyah, Finds.Count);
                        FoundResultColor = Color.Black;
                        IsVisibleNotFoundMessage = false;
                    }
                    else
                    {
                        NumbersFoundAyahAndSurah = string.Format("Kata Kunci '{0}' tidak ditemukan.", SearchQuery);
                        NotFoundMessage = Message.MSG_NOT_FOUND_FIND;
                        IsVisibleNotFoundMessage = true;
                        FoundResultColor = Color.Red;
                    }

                    IsVisibleExpandAll = Finds.Count > 0;

                    foreach (var group in Finds)
                    {
                        group.Clear();
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_FIND + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            SelectedFind = null;
        }

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