using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MyQuranIndo.ViewModels.Bookmarks;
using MyQuranIndo.Views.Bookmarks;
using Xamarin.Forms;
using MyQuranIndo.Models;
using MyQuranIndo.Messages;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using MyQuranIndo.References;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MyQuranIndo.Models.Bookmarks;
using MyQuranIndo.Views.Surah;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Views.Juz;
using MyQuranIndo.ViewModels.Juz;
using MyQuranIndo.Helpers;

namespace MyQuranIndo.ViewModels.Bookmarks
{
    public class BookmarkViewModel : BaseViewModel
    {
        private bool isBookmarkEmpty;
        private string emptyDataMessage;
        private bool isExistData;
        public ObservableCollection<Bookmark> Bookmarks { get; }
        public ICommand LoadCommand { get; }
        public ICommand ItemTapped { get; }
        public ICommand ItemDoubleTapped { get; }

        //public IHasCollectionView View { get; set; }
        //public ObservableCollection<AuthWarehouses> Warehouses { get; set; }
        public bool IsBookmarkEmpty
        {
            get => isBookmarkEmpty;
            set => SetProperty(ref isBookmarkEmpty, value);
        }

        public string EmptyDataMessage
        {
            get => emptyDataMessage;
            set => SetProperty(ref emptyDataMessage, value);
        }
        public bool IsExistData
        {
            get => isExistData;
            set => SetProperty(ref isExistData, value);
        }

        public BookmarkViewModel()
        {
            Title = "Daftar Bookmark";
            Bookmarks = new ObservableCollection<Bookmark>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ItemTapped = new Command<Bookmark>(OnBookmarkSelected);
            ItemDoubleTapped = new Command<Bookmark>(OnBookmarkDeleted);
            isBookmarkEmpty = true;
            emptyDataMessage = "";
            isExistData = false;
        }

        private async void OnBookmarkDeleted(Bookmark bookmark)
        {
            if (bookmark != null)
            {
                var oldColor = bookmark.RowColor;
                bookmark.RowColor = ((Color)Application.Current.Resources["SelectedItem"]);

                await ActionHelper.DeleteBookmarkAsync(bookmark.SurahID, bookmark.AyahID, bookmark.SurahNameLatin, ToastService);
                await ExecuteLoadCommand();
                bookmark.RowColor = oldColor;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, Message.MSG_NO_BOOKMARK, Message.MSG_OK);
            }
        }
        private async void OnBookmarkSelected(Bookmark bookmark)
        {
            if (bookmark!= null)
            {
                var oldColor = bookmark.RowColor;
                bookmark.RowColor = ((Color)Application.Current.Resources["SelectedItem"]);


                //string action = await App.Current.MainPage.DisplayActionSheet("Q.S " + bookmark.SurahNameLatin + " " + bookmark.SurahID
                //    + ": Ayat " + bookmark.AyahID, Message.MSG_CANCEL, null, Message.ACTION_LOOK_AS_SURAH, Message.ACTION_LOOK_AS_JUZ);
                //await OpenAyahPage(bookmark.SurahID, bookmark.AyahID, action);

                var juzID = await JuzDataService.GetJuzIDAsync(bookmark.SurahID, bookmark.AyahID);
                await ActionHelper.OpenAyahPageAsync(bookmark.SurahID, bookmark.AyahID, juzID, bookmark.SurahNameLatin);
                bookmark.RowColor = oldColor;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, Message.MSG_NO_BOOKMARK, Message.MSG_OK);
            }
        }
        //private async Task OpenAyahPage(int surahID, int ayahID, string action)
        //{
        //    if (action == Message.ACTION_LOOK_AS_SURAH)
        //    {
        //        await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}?{nameof(TabbedPageSurahDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageSurahDetailViewModel.AyahID)}={ayahID}");
        //    }
        //    else
        //    {
        //        var juzID = await JuzDataService.GetJuzIDAsync(surahID, ayahID);
        //        await Shell.Current.GoToAsync($"{nameof(JuzDetailPage)}?{nameof(JuzDetailViewModel.JuzID)}={juzID}&{nameof(JuzDetailViewModel.SurahID)}={surahID}&{nameof(JuzDetailViewModel.AyahID)}={ayahID}");
        //    }
        //}

        private async Task ExecuteLoadCommand()
        {
            try
            {
                Bookmarks.Clear();
                if (Preferences.ContainsKey(MenuKey.BOOKMARK))
                {
                    var getBookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(Preferences.Get(MenuKey.BOOKMARK, null));

                    if (getBookmarks != null && getBookmarks.Count > 0)
                    {
                        for (int i = 0; i < getBookmarks.Count; i++)
                        {
                            //if (getBookmarks[i].Row == 0)
                            //{
                            //    getBookmarks[i].Row = i + 1;
                            //}

                            getBookmarks[i].Row = i + 1;

                            Bookmarks.Add(getBookmarks[i]);
                        }
                        IsBookmarkEmpty = false;                        
                    }
                    else
                    {
                        IsBookmarkEmpty = true;
                        EmptyDataMessage = Message.MSG_NO_BOOKMARK;
                    }
                }
                else
                {
                    IsBookmarkEmpty = true;
                    EmptyDataMessage = Message.MSG_NO_BOOKMARK;
                }

                IsExistData = !IsBookmarkEmpty;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_BOOKMARK
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
        }

    }
}
