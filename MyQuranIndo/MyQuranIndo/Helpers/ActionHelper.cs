using MyQuranIndo.Configuration;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Bookmarks;
using MyQuranIndo.Models.Hadiths;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Models.Zikrs;
using MyQuranIndo.References;
using MyQuranIndo.Services;
using MyQuranIndo.ViewModels.Juz;
using MyQuranIndo.ViewModels.Surah;
using MyQuranIndo.Views.Bookmarks;
using MyQuranIndo.Views.Juz;
using MyQuranIndo.Views.Surah;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Helpers
{
    public static class ActionHelper
    {
        public const string ACTION_LOOK_AS_SURAH = "Lihat Sebagai Surat";
        public const string ACTION_LOOK_AS_JUZ = "Lihat Sebagai Juz";

        public const string AYAH_SHARE = "Bagikan Ayat";
        public const string AYAH_COPY = "Salin Ayat";
        public const string AYAH_BOOKMARK = "Tambahkan Ke Bookmark";
        public const string AYAH_LAST_READ = "Tandai Terakhir Dibaca";

        public const string TAFSIR_SHARE = "Bagikan Tafsir";
        public const string TAFSIR_COPY = "Salin Tafsir";
        public const string TAFSIR_KEMENAG_SHOW = "Tampilkan Tafsir Kemenag";
        public const string TAFSIR_Al_JALALAIN_SHOW = "Tampilkan Tafsir Al-Jalalain";

        public const string BOOKMARK_DEL = "Hapus Bookmark";
        public const string PLAY_MP3 = "Dengarkan Murottal"; 
        public const string ZIKR_SHARE = "Bagikan Dzikir";
        public const string ZIKR_COPY = "Salin Dzikir";
        //public const string ZIKR_SHARE_ALL = "Bagikan Semua Dzikir";
        //public const string ZIKR_COPY_ALL = "Salin Dzikir";
        public const string ASMAUL_HUSNA_SHARE = "Bagikan Asmaul Husna";
        public const string ASMAUL_HUSNA_COPY = "Salin Asmaul Husna";
        public const string ASMAUL_HUSNA_SHARE_ALL = "Bagikan Semua Asmaul Husna";
        public const string ASMAUL_HUSNA_COPY_ALL = "Salin Semua Asmaul Husna";

        public const string PRAY_SHARE = "Bagikan Do'a";
        public const string PRAY_COPY = "Salin Do'a";
        public const string PRAY_SHARE_ALL = "Bagikan Kumpulan Do'a";
        public const string PRAY_COPY_ALL = "Salin Kumpulan Do'a";

        public const string HADITH_SHARE = "Bagikan Hadis";
        public const string HADITH_COPY = "Salin Hadis";

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

        public async static Task<bool> OpenAyahPageFromBookmarkAsync(int surahID, int ayahID, int juzID, string surahNameLatin, bool isOpenFromBookmark = false, IToastService toastService = null)
        {
            string action = "";
            if (isOpenFromBookmark)
            {
                action = await App.Current.MainPage.DisplayActionSheet("Q.S " + surahNameLatin + " " + surahID
                    + ": Ayat " + ayahID, Message.MSG_CANCEL, null, ACTION_LOOK_AS_SURAH, ACTION_LOOK_AS_JUZ, BOOKMARK_DEL);
            }
            else
            {
                action = await App.Current.MainPage.DisplayActionSheet("Q.S " + surahNameLatin + " " + surahID
                    + ": Ayat " + ayahID, Message.MSG_CANCEL, null, ACTION_LOOK_AS_SURAH, ACTION_LOOK_AS_JUZ);
            }

            if (action == ACTION_LOOK_AS_SURAH)
            {
                await Shell.Current.GoToAsync($"{nameof(TabbedPageSurahDetailPage)}?{nameof(TabbedPageSurahDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageSurahDetailViewModel.AyahID)}={ayahID}");
            }
            else if (action == ACTION_LOOK_AS_JUZ)
            {
                await Shell.Current.GoToAsync($"{nameof(TabbedPageJuzDetailPage)}?{nameof(TabbedPageJuzDetailViewModel.JuzID)}={juzID}&{nameof(TabbedPageJuzDetailViewModel.SurahID)}={surahID}&{nameof(TabbedPageJuzDetailViewModel.AyahID)}={ayahID}");
            }
            else if (action == BOOKMARK_DEL)
            {
                await DeleteBookmarkAsync(surahID, ayahID, surahNameLatin, toastService);
                return true;
            }
            return false;
        }

        public static async Task<string> DisplayActionAyahAsync(int surahID, int ayahID, string surahNameLatin)
        {
            string title = $"Q.S. {surahID}. {surahNameLatin} Ayat {ayahID}";
            string action = await App.Current.MainPage.DisplayActionSheet(title, //$"Q.S {surahID}:{ayahID}",
                Message.MSG_CANCEL, null, PLAY_MP3, AYAH_SHARE, AYAH_COPY, AYAH_BOOKMARK, AYAH_LAST_READ, TAFSIR_KEMENAG_SHOW,
                    TAFSIR_Al_JALALAIN_SHOW);

            return action;
        }

        public static async Task<string> DisplayActionHadithAsync(string narratorName, int number)
        {
            string title = $"H.R. {narratorName} No. {number}";
            string action = await App.Current.MainPage.DisplayActionSheet(title,
                Message.MSG_CANCEL, null, HADITH_SHARE, HADITH_COPY);

            return action;
        }

        public static async Task<string> DisplayActionTafsirAsync(int surahID, int ayahID, string surahNameLatin)
        {
            string title = $"Q.S. {surahID}. {surahNameLatin} Ayat {ayahID}";
            string action = await App.Current.MainPage.DisplayActionSheet(title, //$"Q.S {surahID}:{ayahID}",
                Message.MSG_CANCEL, null, TAFSIR_SHARE, TAFSIR_COPY, ACTION_LOOK_AS_SURAH, ACTION_LOOK_AS_JUZ);

            return action;
        }

        public static async Task<string> DisplayActionZikrAsync(string title)
        {
            string action = await App.Current.MainPage.DisplayActionSheet(title,
                Message.MSG_CANCEL, null, ZIKR_SHARE,ZIKR_COPY);

            return action;
        }

        public static async Task<string> DisplayActionPrayAsync(string title)
        {
            string action = await App.Current.MainPage.DisplayActionSheet(title,
                Message.MSG_CANCEL, null, PRAY_SHARE, PRAY_COPY);// PRAY_SHARE_ALL, PRAY_COPY, PRAY_COPY_ALL);

            return action;
        }

        public static async Task<string> DisplayActionAsmaulHusnaAsync(string title)
        {
            string action = await App.Current.MainPage.DisplayActionSheet(title,
                Message.MSG_CANCEL, null, ASMAUL_HUSNA_SHARE, ASMAUL_HUSNA_SHARE_ALL, ASMAUL_HUSNA_COPY, ASMAUL_HUSNA_COPY_ALL);

            return action;
        }

        public static string GetAyahToShare(Ayah ayah, string surahNameLatin)
        {           

            string ayahCopied = "Allah Subhanahu Wa Ta'ala berfirman: ";
            string line = Environment.NewLine + Environment.NewLine;

            ayahCopied += $"{line + ayah.ReadTextArabic}";
            if (ayah.IsVisibleTransliteration)
            {
                ayahCopied += $"{line + ayah.TextIndo}";
            }
            if (ayah.IsVisibleTranslate)
            {
                ayahCopied += $"{line + ayah.TranslateIndo}";
            }
            ayahCopied += $"{Environment.NewLine}(Q.S. {surahNameLatin} {ayah.AyahID}: Ayat {ayah.ID}){line}*Via {AppSetting.GetApplicationName()}";
            ayahCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return ayahCopied;
        }

        public static string GetHadithToShare(Hadith hadith, string narratorName)
        {

            string hadithCopied = $"H.R. {narratorName} No. {hadith.Number}";
            string line = Environment.NewLine + Environment.NewLine;

            hadithCopied += $"{line + hadith.Arabic}";
            hadithCopied += $"{line + hadith.Id}";
            
            hadithCopied += $"{line}*Via {AppSetting.GetApplicationName()}";
            hadithCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return hadithCopied;
        }

        public static string GetTafsirToShare(Tafsir tafsir, string surahNameLatin)
        {

            string tafsirCopied = $"Tafsir {TafsirTypeHelper.GetTafsirTypeName()} Q.S {tafsir.SurahID}:{tafsir.TafsirID}: ";
            string line = Environment.NewLine + Environment.NewLine;

            tafsirCopied += $"{line + tafsir.TafsirText}";
            tafsirCopied += $"{line}*Via {AppSetting.GetApplicationName()}";
            tafsirCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return tafsirCopied;
        }

        public static string GetAyahToShare(JuzDetail juzDetail)
        {
            string ayahCopied = "Allah Subhanahu Wa Ta'ala berfirman: ";
            string line = Environment.NewLine + Environment.NewLine;

            ayahCopied += $"{line + juzDetail.ReadTextArabic}";
            if (juzDetail.IsVisibleTransliteration)
            {
                ayahCopied += $"{line + juzDetail.TextIndo}";
            }
            if (juzDetail.IsVisibleTranslate)
            {
                ayahCopied += $"{line + juzDetail.TranslateIndo}";
            }
            ayahCopied += $"{Environment.NewLine}(Q.S. {juzDetail.SurahNameLatin} {juzDetail.AyahID}: Ayat {juzDetail.AyahID}){line}*Via {AppSetting.GetApplicationName()}";
            ayahCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return ayahCopied;
        }

        public static string GetZikrToShare(Zikr zikr, string title)
        {

            string zikrCopied = zikr.TitleAndNumber + Environment.NewLine + zikr.Note;
            string line = Environment.NewLine + Environment.NewLine;

            zikrCopied += $"{line + zikr.Arabic}";
            if (zikr.IsVisibleArabicLatin)
            {
                zikrCopied += $"{line + zikr.ArabicLatin}";
            }
            zikrCopied += $"{line + zikr.TranslateID}";
            if (zikr.IsVisibleFaedah)
            {
                zikrCopied += $"{line}Faedah : {zikr.Faedah}";
            }
            //zikrCopied += $"{Environment.NewLine}({title}){line} *Via {AppSetting.GetApplicationName()}";
            zikrCopied += $"{line}{title}{Environment.NewLine}*Via {AppSetting.GetApplicationName()}";
            zikrCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return zikrCopied;
        }

        public static string GetPrayToShare(Pray pray, string title)
        {

            string zikrCopied = pray.Title; // pray.TitleAndNumber;
            string line = Environment.NewLine + Environment.NewLine;

            zikrCopied += $"{line + pray.Arabic}";
            if (pray.IsVisibleArabicLatin)
            {
                zikrCopied += $"{line + pray.ArabicLatin}";
            }
            zikrCopied += $"{line + pray.TranslateID}";
            if (pray.IsVisibleFaedah)
            {
                zikrCopied += $"{line}Faedah : {pray.Faedah}";
            }
            zikrCopied += $"{Environment.NewLine}({pray.Narrator})";
            //zikrCopied += $"{Environment.NewLine}({title}){line} *Via {AppSetting.GetApplicationName()}";
            zikrCopied += $"{line}{title}{Environment.NewLine}*Via {AppSetting.GetApplicationName()}";
            zikrCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return zikrCopied;
        }

        public static string GetPraysAllToShare(ObservableCollection<Models.Zikrs.Pray> prays, string title)
        {
            string line = Environment.NewLine + Environment.NewLine;
            string zikrCopied = title;
            foreach (var pray in prays)
            {
                zikrCopied += $"{line + pray.TitleAndNumber}";
                zikrCopied += $"{line + pray.Arabic}";
                if (pray.IsVisibleArabicLatin)
                {
                    zikrCopied += $"{line + pray.ArabicLatin}";
                }
                zikrCopied += $"{Environment.NewLine + pray.TranslateID}";
                if (pray.IsVisibleFaedah)
                {
                    zikrCopied += $"{line}Faedah : {pray.Faedah}";
                }
            }
            zikrCopied += $"{line}*Via {AppSetting.GetApplicationName()}";
            zikrCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return zikrCopied;
        }

        public static string GetAsmaulHusnaToShare(Models.AsmaulHusna.AsmaulHusna ah, string title)
        {

            string zikrCopied = title;
            string line = Environment.NewLine + Environment.NewLine;

            zikrCopied += $"{line + ah.Arabic}";
            zikrCopied += $"{line + ah.Title}";
            zikrCopied += $"{line}*Via {AppSetting.GetApplicationName()}";
            zikrCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return zikrCopied;
        }

        public static string GetAsmaulHusnaAllToShare(ObservableCollection<Models.AsmaulHusna.AsmaulHusna> ahs, string title)
        {            
            string line = Environment.NewLine + Environment.NewLine;
            string zikrCopied = title;
            foreach (var ah in ahs)
            {
                zikrCopied += $"{line + ah.Arabic}";
                zikrCopied += $"{Environment.NewLine + ah.Title}";
            }
            zikrCopied += $"{line}*Via {AppSetting.GetApplicationName()}";
            zikrCopied += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

            return zikrCopied;
        }

        public static async Task ShareAyahAsync(string ayahCopied)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = ayahCopied,
                Title = ActionHelper.AYAH_SHARE
            });
        }
        public static async Task ShareHadithAsync(string hadithCopied)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = hadithCopied,
                Title = ActionHelper.HADITH_SHARE
            });
        }
        public static async Task ShareZikrAsync(string copiedText)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = copiedText,
                Title = ActionHelper.ZIKR_SHARE
            });
        }

        public static async Task SharePrayAsync(string copiedText)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = copiedText,
                Title = ActionHelper.PRAY_SHARE
            });
        }

        public static async Task ShareAsmaulHusnaAsync(string copiedText)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = copiedText,
                Title = ActionHelper.ASMAUL_HUSNA_SHARE
            });
        }

        public static async Task<bool> CopyTafsirAsync(string title, string tafsir)
        {
            var isCopy = await App.Current.MainPage.DisplayAlert(title, tafsir, Message.COPY, Message.MSG_OK);
            if (isCopy)
            {
                string line = Environment.NewLine + Environment.NewLine;
                string tafsirText = $"{title + line + tafsir + line}*Via {AppSetting.GetApplicationName()}";
                tafsirText += $"{Environment.NewLine}{AppSetting.GetUrlPlayStore()}";

                await Clipboard.SetTextAsync(tafsirText);
                //await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest
                //{
                //    Text = tafsirText,
                //    Title = ActionHelper.TAFSIR_SHARE
                //});
                return true;
            }
            return false;
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
                result = $"Q.S. {ayah.SurahID}. {surahNameLatin} Ayat {ayah.ID} berhasil di bookmark";
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
                    result = $"Ayat terakhir dibaca berhasil diubah menjadi Q.S. {ayah.SurahID}. {surahNameLatin} Ayat {ayah.ID}";
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Error" + Environment.NewLine + ex.Message;
            }

            return new Tuple<string, string>(result, errorMessage);
        }
        //public async static Task DeleteBookmarkAsync(int surahID, int ayahID, string surahNameLatin, IToastService toastService)
        //{

        //    string action = await App.Current.MainPage.DisplayActionSheet("Q.S " + surahNameLatin + " " + surahID
        //        + ": Ayat " + ayahID, Message.MSG_CANCEL, null, BOOKMARK_DEL);

        //    if (action == BOOKMARK_DEL)
        //    {
        //        var bookmarks = new List<Bookmark>();
        //        if (Preferences.ContainsKey(MenuKey.BOOKMARK))
        //        {
        //            List<Bookmark> getBookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(Preferences.Get(MenuKey.BOOKMARK, null));
        //            bookmarks.AddRange(getBookmarks);
        //        }
        //        for (int i = 0; i < bookmarks.Count; i++)
        //        {
        //            if (bookmarks[i].SurahID == surahID && bookmarks[i].AyahID == ayahID)
        //            {
        //                bookmarks.RemoveAt(i);
        //            }
        //        }
        //        Preferences.Set(MenuKey.BOOKMARK, JsonConvert.SerializeObject(bookmarks));
        //        toastService.Show(Message.MSG_SUCCESS_DEL_BOOKMARK.Replace("<ayat>", $"Q.S. {surahID}. {surahNameLatin} Ayat {ayahID}"));
        //        //await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_INFO, Message.MSG_SUCCESS_DEL_BOOKMARK, Message.MSG_OK);
        //    }
        //}

        public async static Task DeleteBookmarkAsync(int surahID, int ayahID, string surahNameLatin, IToastService toastService)
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
        }

        public async static Task PlayMurottalAsync()
        {

        }
    }
}
