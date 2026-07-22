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
        private bool isExpandAll = false; // Diubah dari true menjadi false agar sinkron di awal
        private bool isVisibleExpandAll = false;

        public ICommand HadithOneTapped { get; }
        public ObservableCollection<HadithGroup> Hadiths { get; set; }
        public ObservableCollection<HadithGroup> HadithCopied { get; set; }

        private string slug;
        public string Slug
        {
            get => slug;
            set => slug = value;
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
            set => SetProperty(ref isExpandAll, value);
        }

        public bool IsVisibleExpandAll
        {
            get => isVisibleExpandAll;
            set => SetProperty(ref isVisibleExpandAll, value);
        }

        public Command LoadCommand { get; }
        public Command<HadithGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; set; }
        public ICommand ClearSearchCommand { get; }

        public HadithDetailViewModel()
        {
            Hadiths = new ObservableCollection<HadithGroup>();
            HadithCopied = new ObservableCollection<HadithGroup>();

            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            HeaderTapped = new Command<HadithGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllComand);
            FoundResultColor = (Color)Application.Current.Resources["TextColor"];
            HadithOneTapped = new Command<Hadith>(OnHadithOneTapped, (x) => CanNavigate);
        }

        private void OnExpandAllComand()
        {
            if (IsExpandAll)
            {
                for (int i = 0; i < Hadiths.Count; i++)
                {
                    Hadiths[i].Clear();
                    Hadiths[i].IsExpand = false;
                    Hadiths[i].ImageHeader = GetExpandImageHeaderName();
                }
                IsExpandAll = false;
            }
            else
            {
                Hadiths.Clear();
                for (int i = 0; i < HadithCopied.Count; i++)
                {
                    var group = HadithCopied[i];
                    group.IsExpand = true;
                    group.ImageHeader = GetCollapsImageHeaderName();
                    Hadiths.Add(group);
                }
                IsExpandAll = true;
            }
        }

        private void OnHeaderTapped(HadithGroup hadithGroup)
        {
            if (hadithGroup == null) return;

            hadithGroup.IsExpand = !hadithGroup.IsExpand;

            if (hadithGroup.IsExpand)
            {
                var index = Hadiths.IndexOf(hadithGroup);
                if (index >= 0 && index < HadithCopied.Count)
                {
                    var context = HadithCopied[index];
                    hadithGroup.Clear();
                    for (int i = 0; i < context.Count; i++)
                    {
                        hadithGroup.Add(context[i]);
                    }
                }
                hadithGroup.ImageHeader = GetCollapsImageHeaderName();
            }
            else
            {
                hadithGroup.Clear();
                hadithGroup.ImageHeader = GetExpandImageHeaderName();
            }
        }

        private void ClearSearch()
        {
            Hadiths?.Clear();
            HadithCopied?.Clear();
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
                            var resultSearch = await HadithDataService.GetHadithResultAPIAsync(narrator.Slug, page, pageSize, true);
                            if (resultSearch?.Items != null)
                            {
                                resultSearch.Items = resultSearch.Items.Where(q => q.Number == number).ToList();
                                haditsResultAPI.Add(resultSearch);
                            }
                            goto loadData;
                        }
                    }

                    page = 1;
                    for (int i = 1; i <= count; i++)
                    {
                        if (i % pageSize == 0)
                        {
                            var result = await HadithDataService.GetHadithResultAPIAsync(narrator.Slug, page, pageSize, true);
                            if (result?.Items != null && !String.IsNullOrWhiteSpace(SearchQuery))
                            {
                                int.TryParse(searchQuery, out number);
                                var query = result.Items.Where(q => q.Id != null && q.Id.Trim().ToLower().Contains(SearchQuery.Trim().ToLower()));
                                result.Items = query.ToList();
                            }
                            if (result != null) haditsResultAPI.Add(result);
                            page++;
                        }
                    }

                    var lastHaditsAPI = await HadithDataService.GetHadithResultAPIAsync(narrator.Slug, page, pageSize, true);
                    if (lastHaditsAPI?.Items != null && !String.IsNullOrWhiteSpace(SearchQuery))
                    {
                        int.TryParse(searchQuery, out number);
                        IEnumerable<Hadith> query = Enumerable.Empty<Hadith>();
                        if (number > 0)
                        {
                            query = lastHaditsAPI.Items.Where(q => q.Number == number);
                        }
                        else
                        {
                            query = lastHaditsAPI.Items.Where(q => q.Id != null && q.Id.Trim().ToLower().Contains(SearchQuery.Trim().ToLower()));
                        }
                        lastHaditsAPI.Items = query.ToList();
                    }
                    if (lastHaditsAPI != null) haditsResultAPI.Add(lastHaditsAPI);

                    loadData:
                    int currentPage = 1;
                    foreach (var hdsData in haditsResultAPI)
                    {
                        if (hdsData?.Items == null || !hdsData.Items.Any()) continue;

                        var hdList = new ObservableCollection<Hadith>(hdsData.Items);

                        string groupTitle = "";
                        var firstItem = hdsData.Items.FirstOrDefault();
                        var lastItem = hdsData.Items.LastOrDefault();

                        if (firstItem?.Number == lastItem?.Number)
                        {
                            groupTitle = firstItem?.Number.ToString();
                        }
                        else
                        {
                            groupTitle = $"{firstItem?.Number} - {lastItem?.Number}";
                        }

                        // 1. Simpan salinan data lengkap ke HadithCopied
                        var fullGroup = new HadithGroup(currentPage, groupTitle, new ObservableCollection<Hadith>(hdList));
                        HadithCopied.Add(fullGroup);

                        // 2. Buat grup untuk ditampilkan dengan status awal Collapse (dikosongkan itemnya)
                        var displayGroup = new HadithGroup(currentPage, groupTitle, new ObservableCollection<Hadith>());
                        displayGroup.IsExpand = false;
                        displayGroup.ImageHeader = GetExpandImageHeaderName();

                        Hadiths.Add(displayGroup);

                        currentPage++;
                    }

                    IsVisibleFoundMessage = !string.IsNullOrWhiteSpace(SearchQuery);

                    int totalItemsCount = haditsResultAPI.Sum(x => x?.Items?.Count ?? 0);
                    if (haditsResultAPI.Count == 0 || totalItemsCount == 0)
                    {
                        NotFoundMessage = Message.MSG_NOT_FOUND_FIND;
                        IsVisibleNotFoundMessage = true;
                        FoundResultColor = Color.Red;
                        NumbersFoundHadith = $"Kata Kunci '{SearchQuery}' tidak ditemukan.";
                    }
                    else
                    {
                        FoundResultColor = (Color)Application.Current.Resources["TextColor"];
                        NumbersFoundHadith = $"Kata Kunci '{SearchQuery}' ditemukan, total {totalItemsCount} hadis.";
                    }

                    IsVisibleExpandAll = Hadiths.Count > 0;
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

        private bool IsValidTapped(Hadith hadith) => hadith != null;

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
                string action = await ActionHelper.DisplayActionHadithAsync(narrator?.Name ?? "", hadith.Number);
                string hadithCopied = ActionHelper.GetHadithToShare(hadith, narrator?.Name ?? "");
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
        }
    }
}