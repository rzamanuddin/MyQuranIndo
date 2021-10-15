using MyQuranIndo.Messages;
using MyQuranIndo.References;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Views.About;
using MyQuranIndo.Views.Finds;
using MyQuranIndo.Views.Help;
using MyQuranIndo.Views.Setting;
using MyQuranIndo.Views.Surah;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using MyQuranIndo.Views.Bookmarks;
using MyQuranIndo.Views.Prayer;
using MyQuranIndo.Views.Qibla;
using System.Windows.Input;
using MyQuranIndo.Views.Juz;
using MyQuranIndo.ViewModels.Juz;
using MyQuranIndo.Views.TabbedPage;
using MyQuranIndo.Helpers;
using MyQuranIndo.Views.Zikr;
using MyQuranIndo.ViewModels.Zikr;
using MyQuranIndo.Views.AsmaulHusna;

namespace MyQuranIndo.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        public Command SurahTapped { get; }
        public Command SurahTabTapped { get; }
        public Command JuzTapped { get; }
        public Command JuzTabTapped { get; }
        public ICommand LastReadTapped { get; }
        public Command BookmarkTapped { get; }
        public Command FindTapped { get; }
        public Command SettingTapped { get; }
        public Command AboutTapped { get; }
        public Command HelpTapped { get; }
        public Command PrayerScheduleTapped { get; }
        public Command QiblaTapped { get; }
        public ICommand ZikrMorningTapped { get; }
        public ICommand ZikrEveningTapped { get; }
        public ICommand AsmaulHusnaTapped { get; }
        public ICommand PrayTapped { get; }
        public ICommand IntentionTapped { get; }
        public HomeViewModel()
        {
            Title = "Home";
            SurahTapped = new Command(async () => await OnSurahSelected());
            SurahTabTapped = new Command(async () => await OnSurahTabSelected());
            LastReadTapped = new Command(async () => await OnLastReadSelected());
            BookmarkTapped = new Command(async () => await OnBookmarkSelected());
            FindTapped = new Command(async () => await OnFindSelected());
            SettingTapped = new Command(async () => await OnSettingSelected());
            AboutTapped = new Command(async () => await OnAboutSelected());
            HelpTapped = new Command(async () => await OnHelpSelected());
            PrayerScheduleTapped = new Command(async () => await OnPrayerScheduleSelected());
            QiblaTapped = new Command(async () => await OnQiblaSelected());
            JuzTapped = new Command(async () => await OnJuzSelected());
            JuzTabTapped = new Command(async () => await OnJuzTabSelected());
            ZikrMorningTapped = new Command(async () => await OnZikrMorningSelected());
            ZikrEveningTapped = new Command(async () => await OnZikrEveningSelected());
            AsmaulHusnaTapped = new Command(async () => await OnAsmaulHusnaSelected());
            PrayTapped = new Command(async () => await OnPraySelected());
            IntentionTapped = new Command(async () => await OnIntentionSelected());
        }

        private async Task OnJuzTabSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}");
        }

        private async Task OnJuzSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(JuzsPage)}");
        }

        private async Task OnQiblaSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(QiblaPage)}");
        }

        private async Task OnHelpSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(HelpPage)}");
        }

        private async Task OnAboutSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(AboutPage)}");
        }

        private async Task OnSurahTabSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}");
            //await ActionHelper.OpenSurahORJuzTabbedPage();
        }

        private async Task OnSettingSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        private async Task OnZikrMorningSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(ZikrsPage)}?{nameof(ZikrViewModel.ZikrTime)}=0");
        }
        private async Task OnZikrEveningSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(ZikrsPage)}?{nameof(ZikrViewModel.ZikrTime)}=1");
        }

        private async Task OnAsmaulHusnaSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(AsmaulHusnaPage)}");
        }
        private async Task OnPraySelected()
        {
            await Shell.Current.GoToAsync($"{nameof(PraysPage)}");
        }
        private async Task OnIntentionSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(IntentionsPage)}");
        }

        private async Task OnBookmarkSelected()
        {
            //if (Preferences.ContainsKey(MenuKey.BOOKMARK))
            //{
            //    var bookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(Preferences.Get(MenuKey.BOOKMARK, null));

            //    string[] buttons = new string[bookmarks.Count + 1];
            //    var surahs = await SurahDataService.GetAsync(true);
            //    //List<Bookmark> bookmarks = new List<Bookmark>();
            //    for (int i = 0; i < bookmarks.Count; i++)
            //    {
            //        ////var surah = surahs[getBookmarks[i].SurahID];

            //        //var b = new Bookmark();
            //        //b.AyahID = getBookmarks[i].AyahID;
            //        //b.SurahID = getBookmarks[i].SurahID; // surah.Number;
            //        //b.SurahNameLatin = getBookmarks[i].SurahNameLatin; //surah.NameLatin;

            //        if (bookmarks[i].CreatedDate != null)
            //        {
            //            buttons[i] = bookmarks[i].ToString();// + bookmarks[i].CreatedDate.ToString("dd/MM/yy HH:mm");
            //        }
            //        else
            //        {
            //            buttons[i] = bookmarks[i].ToString();
            //        }
            //        //bookmarks.Add(b);
            //    }

            //    if (buttons.Length > 0)
            //    {
            //        buttons[buttons.Length - 1] = Message.MSG_DELETE_BOOKMARKS;
            //    }

            //    string result = await App.Current.MainPage.DisplayActionSheet(Message.MSG_BOOKMARK, "Cancel", null, buttons);
            //    if (!String.IsNullOrWhiteSpace(result))
            //    {
            //        // If delete bookmarks then remove bookmarks value
            //        if (result == Message.MSG_DELETE_BOOKMARKS)
            //        {
            //            Preferences.Remove(MenuKey.BOOKMARK);
            //        }
            //        else
            //        {
            //            for (int i = 0; i < buttons.Length; i++)
            //            {
            //                if (result == buttons[i])
            //                {
            //                    await OpenAyahPage(bookmarks[i].SurahID, bookmarks[i].AyahID);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, Message.MSG_NO_BOOKMARK, Message.MSG_OK);
            //}
            await Shell.Current.GoToAsync($"{nameof(BookmarksPage)}");
        }

        private async Task OnSurahSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(SurahPage)}");
            //await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahsAndJuzsPage)}");
            //await ActionHelper.OpenSurahORJuzPage();
        }

        private async Task OnLastReadSelected()
        {
            int surahID = Preferences.Get(MenuKey.LAST_SURAH, 0);
            int ayahID = Preferences.Get(MenuKey.LAST_AYAH, 0);

            if (surahID == 0 && ayahID == 0)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, Message.MSG_NOT_YET_MARK_LAST_READ, Message.MSG_OK);
            }
            else
            {
                var surah = await SurahDataService.GetSurahAsync(surahID);
                var juzID = await JuzDataService.GetJuzIDAsync(surahID, ayahID);
                await ActionHelper.OpenAyahPageAsync(surahID, ayahID, juzID, surah.NameLatin);
            }
        }

        //private async Task OpenAyahPage(int surahID, int ayahID, string action)
        //{
        //    if (action == ActionHelper.ACTION_LOOK_AS_SURAH)
        //    {
        //        await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}?{nameof(TabbedPageSurahDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageSurahDetailViewModel.AyahID)}={ayahID}");
        //    }
        //    else if (action == ActionHelper.ACTION_LOOK_AS_JUZ)
        //    {
        //        var juzID = await JuzDataService.GetJuzIDAsync(surahID, ayahID);
        //        await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}?{nameof(TabbedPageJuzDetailViewModel.JuzID)}={juzID}&{nameof(TabbedPageJuzDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageJuzDetailViewModel.AyahID)}={ayahID}");
        //    }
        //}

        private async Task OnFindSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(FindPage)}");
        }
        private async Task OnPrayerScheduleSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(PrayerSchedulePage)}");
        }
    }
}
