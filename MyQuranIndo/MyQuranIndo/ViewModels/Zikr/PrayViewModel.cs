using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Zikrs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Zikr
{
    public class PrayViewModel : BaseViewModel
    {
        private string searchQuery;
        private string notFoundMessage;
        private bool isVisibleNotFoundMessage;
        private string numbersFoundAyahAndSurah;
        private Color foundResultColor;
        private PrayData selectedPray;
        private bool isExpandAll = true;
        private bool isVisibleExpandAll = false;

        public ICommand PrayOneTapped { get; }
        public ObservableCollection<PrayGroup> Prays { get; }
        public ObservableCollection<PrayGroup> PraysCopied { get; }

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
            set => SetProperty(ref isExpandAll, value);
        }

        public bool IsVisibleExpandAll
        {
            get => isVisibleExpandAll;
            set => SetProperty(ref isVisibleExpandAll, value);
        }

        public Command LoadCommand { get; }
        public Command<PrayGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; }
        public ICommand ClearSearchCommand { get; }

        public PrayViewModel()
        {
            Title = "Kumpulan Do'a";
            Prays = new ObservableCollection<PrayGroup>();
            PraysCopied = new ObservableCollection<PrayGroup>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            HeaderTapped = new Command<PrayGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllCommand);
            FoundResultColor = Color.Black;
            PrayOneTapped = new Command<Pray>(OnPrayOneTapped, (x) => CanNavigate);
        }

        private void OnExpandAllCommand()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (IsExpandAll)
                {
                    foreach (var group in Prays)
                    {
                        group.Clear();
                    }
                    IsExpandAll = false;
                }
                else
                {
                    Prays.Clear();
                    foreach (var group in PraysCopied)
                    {
                        Prays.Add(group);
                    }
                    IsExpandAll = true;
                }
            });
        }

        private void OnHeaderTapped(PrayGroup prayGroup)
        {
            if (prayGroup == null) return;

            prayGroup.IsExpand = !prayGroup.IsExpand;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (prayGroup.IsExpand)
                {
                    var index = Prays.IndexOf(prayGroup);
                    if (index >= 0 && index < PraysCopied.Count)
                    {
                        var context = PraysCopied[index];
                        prayGroup.Clear();
                        foreach (var item in context)
                        {
                            prayGroup.Add(item);
                        }
                    }
                    prayGroup.ImageHeader = GetCollapsImageHeaderName();
                }
                else
                {
                    prayGroup.Clear();
                    prayGroup.ImageHeader = GetExpandImageHeaderName();
                }
            });
        }

        private void ClearSearch()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Prays.Clear();
                PraysCopied.Clear();
            });
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                NotFoundMessage = string.Empty;
                IsVisibleNotFoundMessage = false;
                NumbersFoundAyahAndSurah = string.Empty;
                FoundResultColor = Color.Gray;

                var hcsData = await PrayDataService.GetPraysAsync(true);
                if (hcsData == null) return;

                if (!string.IsNullOrWhiteSpace(SearchQuery))
                {
                    string keyword = SearchQuery.Trim().ToLower();
                    hcsData = hcsData.Where(q =>
                        (q.Title != null && q.Title.ToLower().Contains(keyword)) ||
                        (q.Data != null && q.Data.Any(a =>
                            (a.ArabicLatin != null && a.ArabicLatin.ToLower().Contains(keyword)) ||
                            (a.TranslateID != null && a.TranslateID.ToLower().Contains(keyword)) ||
                            (a.Tag != null && a.Tag.Contains(keyword))
                        ))
                    ).ToList();
                }

                var tempPrays = new List<PrayGroup>();
                var tempPraysCopied = new List<PrayGroup>();

                var prayGroup = hcsData.ToLookup(q => q.ID);
                int index = 0;
                var hcsList = hcsData.ToList();

                foreach (var hcData in prayGroup)
                {
                    var prayList = new ObservableCollection<Pray>();
                    var prayInGroup = hcsList.Where(q => q.ID == hcData.Key).ToList();

                    for (int i = 0; i < prayInGroup.Count; i++)
                    {
                        for (int r = 0; r < prayInGroup[i].Data.Count; r++)
                        {
                            prayInGroup[i].Data[r].RowID = r + 1;
                            prayList.Add(prayInGroup[i].Data[r]);
                        }
                    }

                    if (index < hcsList.Count)
                    {
                        string titleGroup = hcsList[index].Title;
                        tempPrays.Add(new PrayGroup(hcData.Key, titleGroup, prayList));
                        tempPraysCopied.Add(new PrayGroup(hcData.Key, titleGroup, prayList));
                    }
                    index++;
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Prays.Clear();
                    PraysCopied.Clear();

                    foreach (var item in tempPrays) Prays.Add(item);
                    foreach (var item in tempPraysCopied) PraysCopied.Add(item);

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

                    foreach (var group in Prays)
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

        private bool IsValidTapped(Pray pray)
        {
            if (pray == null || pray.ID == 0)
            {
                return false;
            }
            return true;
        }

        private async void OnPrayOneTapped(Pray pray)
        {
            if (!IsValidTapped(pray)) return;

            CanNavigate = false;
            var oldColor = pray.RowColor;
            pray.RowColor = (Color)Application.Current.Resources["SelectedItem"];

            string result = string.Empty;
            string errorMessage = string.Empty;
            string action = await ActionHelper.DisplayActionPrayAsync(pray.Title);
            string zikrCopied = string.Empty;

            try
            {
                switch (action)
                {
                    case ActionHelper.PRAY_SHARE:
                        zikrCopied = ActionHelper.GetPrayToShare(pray, Title);
                        await ActionHelper.ShareZikrAsync(zikrCopied);
                        break;
                    case ActionHelper.PRAY_COPY:
                        zikrCopied = ActionHelper.GetPrayToShare(pray, Title);
                        await Clipboard.SetTextAsync(zikrCopied);
                        result = ActionHelper.PRAY_COPY + " Berhasil.";
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
                    await DisplayAlertSafe(Message.MSG_TITLE_ERROR, errorMessage, Message.MSG_OK);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                pray.RowColor = oldColor;
                CanNavigate = true;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            selectedPray = null;
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