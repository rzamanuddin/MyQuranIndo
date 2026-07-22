using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Bookmarks;
using MyQuranIndo.References;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Bookmarks
{
    public class BookmarkViewModel : BaseViewModel
    {
        private bool isBookmarkEmpty = true;
        private string emptyDataMessage = string.Empty;

        public ObservableCollection<Bookmark> Bookmarks { get; }
        public ICommand LoadCommand { get; }
        public ICommand ItemTapped { get; }

        public bool IsBookmarkEmpty
        {
            get => isBookmarkEmpty;
            set
            {
                if (SetProperty(ref isBookmarkEmpty, value))
                {
                    OnPropertyChanged(nameof(IsExistData));
                }
            }
        }

        public string EmptyDataMessage
        {
            get => emptyDataMessage;
            set => SetProperty(ref emptyDataMessage, value);
        }

        // Properti kalkulasi untuk performa UI yang lebih bersih & sinkron
        public bool IsExistData => !IsBookmarkEmpty;

        public BookmarkViewModel()
        {
            Title = "Daftar Bookmark";
            Bookmarks = new ObservableCollection<Bookmark>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ItemTapped = new Command<Bookmark>(OnBookmarkSelected);
        }

        private async void OnBookmarkSelected(Bookmark bookmark)
        {
            if (bookmark != null)
            {
                var oldColor = bookmark.RowColor;
                bookmark.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                var juzID = await JuzDataService.GetJuzIDAsync(bookmark.SurahID, bookmark.AyahID);
                var isDeleted = await ActionHelper.OpenAyahPageFromBookmarkAsync(bookmark.SurahID, bookmark.AyahID, juzID, bookmark.SurahNameLatin, true, ToastService);

                if (isDeleted)
                {
                    await ExecuteLoadCommand();
                }
                bookmark.RowColor = oldColor;
            }
            else
            {
                await DisplayAlertSafe(Message.MSG_TITLE_INFO, Message.MSG_NO_BOOKMARK, Message.MSG_OK);
            }
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                var tempBookmarks = new List<Bookmark>();

                if (Preferences.ContainsKey(MenuKey.BOOKMARK))
                {
                    var getBookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(Preferences.Get(MenuKey.BOOKMARK, null));

                    if (getBookmarks != null && getBookmarks.Count > 0)
                    {
                        for (int i = 0; i < getBookmarks.Count; i++)
                        {
                            getBookmarks[i].Row = i + 1;
                            tempBookmarks.Add(getBookmarks[i]);
                        }
                    }
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Bookmarks.Clear();
                    foreach (var item in tempBookmarks)
                    {
                        Bookmarks.Add(item);
                    }

                    IsBookmarkEmpty = Bookmarks.Count == 0;
                    EmptyDataMessage = IsBookmarkEmpty ? Message.MSG_NO_BOOKMARK : string.Empty;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlertSafe(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_BOOKMARK + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
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