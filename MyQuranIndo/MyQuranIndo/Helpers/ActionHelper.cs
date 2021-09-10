using MyQuranIndo.Configuration;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Bookmarks;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.References;
using MyQuranIndo.Services;
using MyQuranIndo.ViewModels.Juz;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Views.Juz;
using MyQuranIndo.Views.Surah;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Helpers
{
    public static class ActionHelper
    {
        public static string ACTION_LOOK_AS_SURAH = "Lihat Sebagai Surat";
        public static string ACTION_LOOK_AS_JUZ = "Lihat Sebagai Juz";

        public const string AYAH_SHARE = "Bagikan Ayat";
        public const string AYAH_COPY = "Salin Ayat";
        public const string AYAH_BOOKMARK = "Tambahkan Ke Bookmark";
        public const string AYAH_LAST_READ = "Tandai Terakhir Dibaca";
        public const string TAFSIR_SHARE = "Bagikan Tafsir";
        public const string BOOKMARK_DEL = "Hapus Bookmark";
        public const string PLAY_MP3 = "Play Murottal";

        public async static Task<string> DisplayActionSurahORJuzAsync()
        {

            string action = await App.Current.MainPage.DisplayActionSheet(Message.MSG_TITLE_READ_QURAN, Message.MSG_CANCEL
                , null, ACTION_LOOK_AS_SURAH, ACTION_LOOK_AS_JUZ);

            return action;
        }

        public async static Task<string> DisplayActionSurahORJuzAsync(int surahID, int ayahID, string surahNameLatin)
        {

            string action = await App.Current.MainPage.DisplayActionSheet("Q.S " + surahNameLatin + " " + surahID
                + ": Ayat " + ayahID, Message.MSG_CANCEL, null, ACTION_LOOK_AS_SURAH, ACTION_LOOK_AS_JUZ);

            return action;
        }

        public async static Task OpenSurahORJuzPage()
        {
            string action = await DisplayActionSurahORJuzAsync();

            if (action == ACTION_LOOK_AS_SURAH)
            {
                await Shell.Current.GoToAsync($"{nameof(SurahPage)}");
            }
            else if (action == ACTION_LOOK_AS_JUZ)
            {
                await Shell.Current.GoToAsync($"{nameof(JuzsPage)}");
            }
        }

        public async static Task OpenSurahORJuzTabbedPageAsync()
        {
            string action = await DisplayActionSurahORJuzAsync();

            if (action == ACTION_LOOK_AS_SURAH)
            {
                await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}");
            }
            else if (action == ACTION_LOOK_AS_JUZ)
            {
                await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}");
            }
        }

        public async static Task OpenAyahPageAsync(int surahID, int ayahID, int juzID, string surahNameLatin)
        {
            string action = await App.Current.MainPage.DisplayActionSheet("Q.S " + surahNameLatin + " " + surahID
                + ": Ayat " + ayahID, Message.MSG_CANCEL, null, ACTION_LOOK_AS_SURAH, ACTION_LOOK_AS_JUZ);

            if (action == ACTION_LOOK_AS_SURAH)
            {
                await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}?{nameof(TabbedPageSurahDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageSurahDetailViewModel.AyahID)}={ayahID}");
            }
            else if (action == ACTION_LOOK_AS_JUZ)
            {
                await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}?{nameof(TabbedPageJuzDetailViewModel.JuzID)}={juzID}&{nameof(TabbedPageJuzDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageJuzDetailViewModel.AyahID)}={ayahID}");
            }
        }

        public static async Task<string> DisplayActionAyahAsync(int surahID, int ayahID, string surahNameLatin)
        {
            string title = $"Q.S. {surahID}. {surahNameLatin} Ayat {ayahID}";
            string action = await App.Current.MainPage.DisplayActionSheet(title, //$"Q.S {surahID}:{ayahID}",
                Message.MSG_CANCEL, null, PLAY_MP3, AYAH_SHARE, AYAH_COPY, AYAH_BOOKMARK, AYAH_LAST_READ);

            return action;
        }
        public static string GetAyahToShare(Ayah ayah, string surahNameLatin)
        {           

            string ayahCopied = "Allah Subhanahu Wa Ta'ala berfirman: ";
            string line = Environment.NewLine + Environment.NewLine;

            ayahCopied += $"{line + ayah.ReadText}";
            if (ayah.IsVisibleTransliteration)
            {
                ayahCopied += $"{line + ayah.TextIndo}";
            }
            if (ayah.IsVisibleTranslate)
            {
                ayahCopied += $"{line + ayah.TranslateIndo}";
            }
            ayahCopied += $"{Environment.NewLine}(Q.S. {surahNameLatin} {ayah.AyahID}: Ayat {ayah.ID}){line} *Via {AppSetting.GetApplicationName()}";
            ayahCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return ayahCopied;
        }

        public static string GetAyahToShare(JuzDetail juzDetail)
        {
            string ayahCopied = "Allah Subhanahu Wa Ta'ala berfirman: ";
            string line = Environment.NewLine + Environment.NewLine;

            ayahCopied += $"{line + juzDetail.ReadText}";
            if (juzDetail.IsVisibleTransliteration)
            {
                ayahCopied += $"{line + juzDetail.TextIndo}";
            }
            if (juzDetail.IsVisibleTranslate)
            {
                ayahCopied += $"{line + juzDetail.TranslateIndo}";
            }
            ayahCopied += $"{Environment.NewLine}(Q.S. {juzDetail.SurahNameLatin} {juzDetail.AyahID}: Ayat {juzDetail.AyahID}){line} *Via {AppSetting.GetApplicationName()}";
            ayahCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return ayahCopied;
        }

        public static async Task ShareAyahAsync(string ayahCopied)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = ayahCopied,
                Title = ActionHelper.AYAH_SHARE
            });
        }
        public static async Task ShareTafsirKemenagAsync(string title, string tafsirKemenag)
        {
            var isShare = await App.Current.MainPage.DisplayAlert(title, tafsirKemenag, Message.SHARE, Message.MSG_OK);
            if (isShare)
            {
                string line = Environment.NewLine + Environment.NewLine;
                string tafsirText = $"{title + line + tafsirKemenag + line} *Via {AppSetting.GetApplicationName()}";
                tafsirText += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";
                await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest
                {
                    Text = tafsirText,
                    Title = ActionHelper.TAFSIR_SHARE
                });
            }
        }

        public static void BookmarkAyah(Ayah ayah, string surahNameLatin, out string result, out string errorMessage)
        {
            result = "";
            errorMessage = "";
            try
            {
                List<Bookmark> bookmarks = new List<Bookmark>();
                if (Preferences.ContainsKey(MenuKey.BOOKMARK))
                {
                    List<Bookmark> getBookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(Preferences.Get(MenuKey.BOOKMARK, null));
                    bookmarks.AddRange(getBookmarks);
                }
                for (int i = 0; i < bookmarks.Count; i++)
                {
                    if (bookmarks[i].SurahID == ayah.SurahID && bookmarks[i].AyahID == ayah.ID)
                    {
                        bookmarks.RemoveAt(i);
                    }
                }
                Bookmark b = new Bookmark();
                b.AyahID = ayah.ID;
                b.CreatedDate = DateTime.Now;
                b.SurahID = ayah.SurahID;
                b.SurahNameLatin = surahNameLatin;
                b.Row = bookmarks.Count + 1;

                bookmarks.Add(b);
                Preferences.Set(MenuKey.BOOKMARK, JsonConvert.SerializeObject(bookmarks));

                //result = string.Format("Q.S {0}:{1} berhasil di bookmark."
                //    , ayah.SurahID, ayah.ID);
                result = $"Q.S. {ayah.SurahID}. {surahNameLatin} Ayat {ayah.ID} berhasil di bookmark.";
            }
            catch (Exception ex)
            {
                errorMessage = "Error" + Environment.NewLine + ex.Message;
            }
        }

        public static async Task<Tuple<string, string>> SetLastReadAyahAsync(Ayah ayah, string surahNameLatin, ISurahDataService surahDataService)
        {
            string result = "";
            string errorMessage = "";

            try
            {
                bool isConfirm = true;
                if (Preferences.ContainsKey(MenuKey.LAST_SURAH))
                {
                    //string test = Preferences.Get(Key.LAST_SURAH, "");
                    int lastSurah = Preferences.Get(MenuKey.LAST_SURAH, 0);
                    int lastAyah = Preferences.Get(MenuKey.LAST_AYAH, 0);
                    var lastSurahRead = await surahDataService.GetSurahAsync(lastSurah);

                    string lastRead = string.Format("Ayat terakhir dibaca adalah Q.S. {0}. {1} Ayat {2}, akan diganti menjadi Q.S. {3}. {4} Ayat {5}"
                        , lastSurah
                        , lastSurahRead.NameLatin
                        , lastAyah
                        , ayah.SurahID
                        , surahNameLatin
                        , ayah.ID);

                    isConfirm = await App.Current.MainPage.DisplayAlert(Message.CONFIRM, lastRead, Message.MSG_YES, Message.MSG_NO);
                }

                if (isConfirm)
                {
                    Preferences.Set(MenuKey.LAST_AYAH, ayah.ID);
                    Preferences.Set(MenuKey.LAST_SURAH, ayah.SurahID);

                    //await SecureStorage.SetAsync(Key.LAST_SURAH, Surah.ID.ToString());
                    //await SecureStorage.SetAsync(Key.LAST_AYAH, ayah.Number.ToString());
                    result = $"Ayat terakhir dibaca berhasil diubah menjadi Q.S. {ayah.SurahID}. {surahNameLatin} Ayat {ayah.ID}.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Error" + Environment.NewLine + ex.Message;
            }

            return new Tuple<string, string>(result, errorMessage);
        }
        public async static Task DeleteBookmarkAsync(int surahID, int ayahID, string surahNameLatin, IToastService toastService)
        {

            string action = await App.Current.MainPage.DisplayActionSheet("Q.S " + surahNameLatin + " " + surahID
                + ": Ayat " + ayahID, Message.MSG_CANCEL, null, BOOKMARK_DEL);

            if (action == BOOKMARK_DEL)
            {
                var bookmarks = new List<Bookmark>();
                if (Preferences.ContainsKey(MenuKey.BOOKMARK))
                {
                    List<Bookmark> getBookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(Preferences.Get(MenuKey.BOOKMARK, null));
                    bookmarks.AddRange(getBookmarks);
                }
                for (int i = 0; i < bookmarks.Count; i++)
                {
                    if (bookmarks[i].SurahID == surahID && bookmarks[i].AyahID == ayahID)
                    {
                        bookmarks.RemoveAt(i);
                    }
                }
                Preferences.Set(MenuKey.BOOKMARK, JsonConvert.SerializeObject(bookmarks));
                toastService.Show(Message.MSG_SUCCESS_DEL_BOOKMARK.Replace("<ayat>", $"Q.S. {surahID}. {surahNameLatin} Ayat {ayahID}"));
                //await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, Message.MSG_SUCCESS_DEL_BOOKMARK, Message.MSG_OK);
            }
        }

        public async static Task PlayMurottalAsync()
        {

        }
    }
}
