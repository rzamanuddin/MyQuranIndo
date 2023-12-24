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
using MyQuranIndo.Views.Tafsir;
using MyQuranIndo.Views.Hadiths;

namespace MyQuranIndo.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand SurahTapped { get; }
        public Command SurahTabTapped { get; }
        public Command JuzTapped { get; }
        public Command JuzTabTapped { get; }
        public ICommand TafsirTapped { get; }
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
        public ICommand JuzAmmaTapped { get; }
        public ICommand HadithTapped { get; }
        public HomeViewModel()
        {
            Title = "MyQuran Indonesia";
            //AsmaulHusnaOneTapped = new Command<Models.AsmaulHusna.AsmaulHusna>(OnAsmaulHusnaOneTapped, (x) => CanNavigate);
            SurahTapped = new Command(async (x) => await OnSurahSelected(), (x) => CanNavigate);
            SurahTabTapped = new Command(async (x) => await OnSurahTabSelected(), (x) => CanNavigate);
            TafsirTapped = new Command(async (x) => await OnTafsirSelected(), (x) => CanNavigate);
            LastReadTapped = new Command(async (x) => await OnLastReadSelected(), (x) => CanNavigate);
            BookmarkTapped = new Command(async (x) => await OnBookmarkSelected(), (x) => CanNavigate);
            FindTapped = new Command(async (x) => await OnFindSelected(), (x) => CanNavigate);
            SettingTapped = new Command(async (x) => await OnSettingSelected(), (x) => CanNavigate);
            AboutTapped = new Command(async (x) => await OnAboutSelected(), (x) => CanNavigate);
            HelpTapped = new Command(async (x) => await OnHelpSelected(), (x) => CanNavigate);
            PrayerScheduleTapped = new Command(async (x) => await OnPrayerScheduleSelected(), (x) => CanNavigate);
            QiblaTapped = new Command(async (x) => await OnQiblaSelected(), (x) => CanNavigate);
            JuzTapped = new Command(async (x) => await OnJuzSelected(), (x) => CanNavigate);
            JuzTabTapped = new Command(async (x) => await OnJuzTabSelected(), (x) => CanNavigate);
            ZikrMorningTapped = new Command(async (x) => await OnZikrMorningSelected(), (x) => CanNavigate);
            ZikrEveningTapped = new Command(async (x) => await OnZikrEveningSelected(), (x) => CanNavigate);
            AsmaulHusnaTapped = new Command(async (x) => await OnAsmaulHusnaSelected(), (x) => CanNavigate);
            PrayTapped = new Command(async (x) => await OnPraySelected(), (x) => CanNavigate);
            IntentionTapped = new Command(async (x) => await OnIntentionSelected(), (x) => CanNavigate);
            JuzAmmaTapped = new Command(async (x) => await OnJuzAmmaSelected(), (x) => CanNavigate);
            HadithTapped = new Command(async (x) => await OnHadithSelected(), (x) => CanNavigate);
        }

        private async Task OnJuzTabSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}");
            CanNavigate = true;
        }

        private async Task OnJuzSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(JuzsPage)}");
            CanNavigate = true;
        }

        private async Task OnQiblaSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(QiblaPage)}");
            CanNavigate = true;
        }

        private async Task OnHelpSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(HelpPage)}");
            CanNavigate = true;
        }

        private async Task OnAboutSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(AboutPage)}");
            CanNavigate = true;
        }

        private async Task OnSurahTabSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}");
            //await ActionHelper.OpenSurahORJuzTabbedPage();
            CanNavigate = true;
        }

        private async Task OnSettingSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
            CanNavigate = true;
        }

        private async Task OnZikrMorningSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(ZikrsPage)}?{nameof(ZikrViewModel.ZikrTime)}=0");
            CanNavigate = true;
        }
        private async Task OnZikrEveningSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(ZikrsPage)}?{nameof(ZikrViewModel.ZikrTime)}=1");
            CanNavigate = true;
        }

        private async Task OnAsmaulHusnaSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(AsmaulHusnaPage)}");
            CanNavigate = true;
        }
        private async Task OnPraySelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(PraysPage)}");
            CanNavigate = true;
        }
        private async Task OnHadithSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(HadithsPage)}");
            CanNavigate = true;
        }
        private async Task OnIntentionSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(IntentionsPage)}");
            CanNavigate = true;
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
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(BookmarksPage)}");
            CanNavigate = true;
        }

        private async Task OnSurahSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(SurahPage)}");
            //await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahsAndJuzsPage)}");
            //await ActionHelper.OpenSurahORJuzPage();
            CanNavigate = true;
        }

        private async Task OnTafsirSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(TafsirsPage)}");
            CanNavigate = true;
        }

        private async Task OnLastReadSelected()
        {
            CanNavigate = false;
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
                _ = await ActionHelper.OpenAyahPageFromBookmarkAsync(surahID, ayahID, juzID, surah.NameLatin);
            }
            CanNavigate = true;
        }

        private async Task OnJuzAmmaSelected()
        {
            CanNavigate = false;
            int surahID = 78;
            int ayahID = 1;

            if (surahID == 0 && ayahID == 0)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, Message.MSG_NOT_YET_MARK_LAST_READ, Message.MSG_OK);
            }
            else
            {
                var surah = await SurahDataService.GetSurahAsync(surahID);
                var juzID = await JuzDataService.GetJuzIDAsync(surahID, ayahID); 
                await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}?{nameof(TabbedPageJuzDetailViewModel.JuzID)}={juzID}&{nameof(TabbedPageJuzDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageJuzDetailViewModel.AyahID)}={ayahID}");

            }
            CanNavigate = true;
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
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(FindPage)}");
            CanNavigate = true;
        }
        private async Task OnPrayerScheduleSelected()
        {
            CanNavigate = false;
            await Shell.Current.GoToAsync($"{nameof(PrayerSchedulePage)}");
            CanNavigate = true;
        }
    }
}
