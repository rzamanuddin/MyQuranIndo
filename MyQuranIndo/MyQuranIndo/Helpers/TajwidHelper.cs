using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MyQuranIndo.Helpers
{
    public static class TajwidHelper
    {
        public const string COLOR_GUNNAH = "#ff4508"; //orange
        public const string COLOR_IDGHAM_BIGUNNAH = "purple";
        public const string COLOR_IDGHAM_BILAGUNNAH = "gray";
        public const string COLOR_IDGHAM_MIMI = "#cccc00";
        public const string COLOR_IQLAB = "#1a8cff"; // blue
        public const string COLOR_IKHFA = "red";
        public const string COLOR_IKHFA_SYAFAWI = "#de4568";
        public const string COLOR_QALQALAH = "#27964c";

        public enum Tajwid : short
        {
            None = 0,
            Gunnah = 1,
            IdghamBigunnah = 2,
            IdghamBilagunnah = 3,
            IdghamMimi = 4,
            Iqlab = 5,
            Ikhfa = 6,
            IkhfaSyafawi = 7,
            Qalqalah = 6,
        }

        public static string NunSukun = "نْ";

        // Nun mati
        public static string[] NunSukunForIqlab = new string[] { "نْۢ", "نۢ" };

        public static string MimSukun = "مْ";

        // Tanwin fathah, kashrah, dhomah
        public static string[] Tanwins = new string[] { "\u064b", "\u064c", "\u064d" };

        public static char[] MimIqlab = new char[] { '\u06E2' };

        // Idgham Bigunnah (ya, nun, mim, wau)
        public static string[] IdghamBigunnahs = new string[] { "ي", "ن", "م", "و" };

        // Idgham Bilagunnah (lam, ra)
        public static string[] IdghamBilagunnahs = { "ل", "ر" };

        // Idgam Mimi
        // TODO: tambahkan mim tanwin
        public static string[] IdghamMimis = new string[] { "مَ", "مِ", "مُ", "مَّ", "مِّ", "مٌّ" };

        // Iqlab
        public static string[] Iqlabs = new string[] { "ب" };

        // TODO: ikhfa, idgham bilagunah juga  masih salah (test al baqarah 145)
        // Ikhfa
        public static string[] Ikhfas = new string[] { "ت", "ث", "ج", "د", "ذ", "ز", "س", "ش", "ص", "ض", "ط", "ظ", "ف", "ق", "ك" };

        // Ikhfa syafawi
        public static string[] IkhfaSyafawis = new string[] { "ب" };

        // Qalqalah
        //string[] qalqalahs = new string[] { "بَّ", "", "", "ﻄْ", "قْ" };
        public static string[] Qalqalahs = new string[] { "بْ", "جْ", "دْ", "ﻄْ", "قْ", "طْ" };

        // gunnah (nun tasydid, mim tasydid)
        public static string[] Gunnahs = new string[] { "نّ", "مَّ", "مّۤ" };

        // \u0645 (wakaf lazim (mim) \u0645)
        public static string[] SkipTexts = new string[] { "ا", "ى", "و" }; //, " ", "\u0645" };

        public static string[] Hijaiyahs = new string[]
        {
                "ا","ب","ت","ث","ج","ج","خ","د","ذ","ر","ز","س","ش","ص","ض","ط","ظ","ع","غ","ف","ق","ك","ل","م","ن", "ه","و","ي","ء","ﻻ", "ة", "\u0654"
        };

        // wakaf  \u06D7 ۗ, \u06DA ۚ, 
        public static char[] WaqafStop = new char[]
        {
            '\u06D7','\u06DA','\u06DB','\u06D6'
        };

        public static char[] WaqafContinue = new char[]
        {
            '\u06D9'//,'\u06DA'
        };

        // unicode of fathah, tanwin fatha, tasydid fatha, dhommah, dammatan,
        public static char[] Harakats = new char[]
        {
            '\u064B', '\u064C','\u064D','\u064E','\u0650', '\u064F','\u0650','\u0651','\u0652','\u0653'
            ,'\u0654','\u0655','\u0656','\u0657', '\u065D', '\u065E','\u065F', '\uFC5e', '\uFC60', '\uFC61', '\uFC62'
            ,'\uFCF4','\uFCF2'
        };

        public static string ReplaceTextToTajwidColor2(string body, string[] tajwids, String color)
        {
            StringBuilder sb = new StringBuilder();

            // Use the enumerator returned from GetTextElementEnumerator
            // method to examine each real character.

            TextElementEnumerator charEnum = StringInfo.GetTextElementEnumerator(body);
            while (charEnum.MoveNext())
            {
                var element = charEnum.GetTextElement();
                string tajwid = "";
                if (tajwids == Gunnahs)
                {
                    foreach (var gunnah in Gunnahs)
                    {
                        if (element.Contains(gunnah))
                        {
                            tajwid = $"<span style='color: {color};'>&zwnj;{element}&zwj;</span>";
                            break;
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(tajwid))
                    {
                        sb.Append(tajwid);
                    }
                    else
                    {
                        sb.Append(element);
                    }
                }
                else if (tajwids == Qalqalahs)
                {
                    foreach (var q in Qalqalahs)
                    {
                        if (element.Contains(q))
                        {
                            tajwid = $"<span style='color: {color};'>{element}</span>";
                            break;
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(tajwid))
                    {
                        sb.Append(tajwid);
                    }
                    else
                    {
                        sb.Append(element);
                    }
                }
            }

            return sb.ToString();
        }

        //public static string ReplaceTextToTajwidColor(string body, string[] tajwids
        //    , string color)
        //{
        //    string pattern = "";
        //    string subtitution = "";

        //    for (int i = 0; i < tajwids.Length; i++)
        //    {
        //        if (tajwids == IdghamMimis)
        //        {
        //            // Mim sukun bertemu huruf mim
        //            pattern = string.Format(@"{0}[{1}]|{0} [{1}]", MimSukun, tajwids[i]);
        //            foreach (Match match in Regex.Matches(body, pattern))
        //            {
        //                subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //                body = Regex.Replace(body, pattern, subtitution);
        //            }
        //            continue;
        //        }
        //        if (tajwids == IkhfaSyafawis)
        //        {
        //            // Mim sukun bertemu huruf ba
        //            pattern = string.Format(@"{0}[{1}]|{0} [{1}]", MimSukun, tajwids[i]);
        //            foreach (Match match in Regex.Matches(body, pattern))
        //            {
        //                subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //                body = Regex.Replace(body, pattern, subtitution);
        //            }
        //            continue;
        //        }

        //        if (tajwids == Qalqalahs)
        //        {
        //            pattern = string.Format(@"{0}", tajwids[i]);
        //            foreach (Match match in Regex.Matches(body, pattern))
        //            {
        //                subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //                body = Regex.Replace(body, pattern, subtitution);
        //            }

        //            //    #region Qalqalah Qubra

        //            //    int length = 0;
        //            //    var charAyah = body.ToCharArray();
        //            //    string endAyah = "";
        //            //    ////foreach (var ws in wakafStop)
        //            //    ////{
        //            //    ////    for (int h = 0; h < harakats.Length; h++)
        //            //    ////    {
        //            //    ////        pattern = string.Format(@"{0}{1}[{2}]", tajwids[i][0], harakats[h], ws);
        //            //    ////        foreach (Match match in Regex.Matches(body, pattern))
        //            //    ////        {
        //            //    ////            subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //            //    ////            body = Regex.Replace(body, pattern, subtitution);
        //            //    ////        }
        //            //    ////    }
        //            //    ////}
        //            //    //foreach (var ws in wakafStop)
        //            //    //{
        //            //    //    for (int c = 0; c < charAyah.Length; c++)
        //            //    //    {
        //            //    //        if (charAyah[c] == ws)
        //            //    //        {
        //            //    //            length = c;

        //            //    //            if (length > 2)
        //            //    //            {
        //            //    //                endAyah = "";
        //            //    //                // get only huruf without harokat
        //            //    //                if (charAyah[length - 2] == tajwids[i][0])
        //            //    //                {
        //            //    //                    endAyah = string.Format("<span style='color: {0};'>{1}</span>", color, body.Substring(length - 2, 2));
        //            //    //                    body = body.Substring(0, length - 2);
        //            //    //                    body += endAyah;
        //            //    //                }
        //            //    //                else if (charAyah[length - 3] == tajwids[i][0])
        //            //    //                {
        //            //    //                    endAyah = string.Format("<span style='color: {0};'>{1}</span>", color, body.Substring(length - 3, 3));
        //            //    //                    body = body.Substring(0, length - 3);
        //            //    //                    body += endAyah;
        //            //    //                }
        //            //    //                else if (charAyah[length - 4] == tajwids[i][0])
        //            //    //                {
        //            //    //                    endAyah = string.Format("<span style='color: {0};'>{1}</span>", color, body.Substring(length - 4, 5));
        //            //    //                    body = body.Substring(0, length - 4);
        //            //    //                    body += endAyah;
        //            //    //                }
        //            //    //            }
        //            //    //        }
        //            //    //    }
        //            //    //}


        //            //    length = body.Length;
        //            //    charAyah = body.ToCharArray();

        //            //    if (length > 2)
        //            //    {
        //            //        endAyah = "";
        //            //        // get only huruf without harokat
        //            //        if (charAyah[length - 2] == tajwids[i][0])
        //            //        {
        //            //            endAyah = string.Format("<span style='color: {0};'>{1}</span>", color, body.Substring(length - 2));
        //            //            body = body.Substring(0, length - 2);
        //            //            body += endAyah;
        //            //        }
        //            //        else if (charAyah[length - 3] == tajwids[i][0])
        //            //        {
        //            //            endAyah = string.Format("<span style='color: {0};'>{1}</span>", color, body.Substring(length - 3));
        //            //            body = body.Substring(0, length - 3);
        //            //            body += endAyah;
        //            //        }
        //            //    }
        //            //    continue;
        //            //    #endregion
        //        }

        //        pattern = $@"{NunSukun}[{tajwids[i]}]|{NunSukun} [{tajwids[i]}]"; //|{NunSukun} [<span style='color: {COLOR_GUNNAH};'>{tajwids[i]}</span>]";
        //        //ib = string.Format("[{0} {1}]", nunMati, idghamBigunnah[i]);
        //        foreach (Match match in Regex.Matches(body, pattern))
        //        {
        //            subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //            body = Regex.Replace(body, pattern, subtitution);
        //        }

        //        if (tajwids == Iqlabs)
        //        {
        //            foreach (var nsi in NunSukunForIqlab)
        //            {
        //                pattern = string.Format(@"{0}[{1}]|{0} [{1}]", nsi, tajwids[i]);
        //                //ib = string.Format("[{0} {1}]", nunMati, idghamBigunnah[i]);
        //                foreach (Match match in Regex.Matches(body, pattern))
        //                {
        //                    subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //                    body = Regex.Replace(body, pattern, subtitution);
        //                }
        //            }
        //        }

        //        for (int h = 0; h < Hijaiyahs.Length; h++)
        //        {
        //            foreach (var tanwin in Tanwins)
        //            {
        //                //string.Format("<span style='color: {0};'>{1}</span>", color, match.Value)
        //                pattern = $"{Hijaiyahs[h]}{tanwin}[{tajwids[i]}]|{Hijaiyahs[h]}{tanwin} [{tajwids[i]}]";
        //                foreach (Match match in Regex.Matches(body, pattern))
        //                {
        //                    subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //                    body = Regex.Replace(body, pattern, subtitution);
        //                }

        //                foreach (var skipText in SkipTexts)
        //                {
        //                    pattern = $"{Hijaiyahs[h]}{tanwin}{skipText} [{tajwids[i]}]";
        //                    foreach (Match match in Regex.Matches(body, pattern))
        //                    {
        //                        subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //                        body = Regex.Replace(body, pattern, subtitution);
        //                    }
        //                }
        //            }
        //        }


        //        // Gunnah still replaced by idgham mimi
        //        if (tajwids == Gunnahs)
        //        {
        //            pattern = string.Format(@"{0}", tajwids[i]);
        //            foreach (Match match in Regex.Matches(body, pattern))
        //            {
        //                subtitution = string.Format("<span style='color: {0};'>{1}</span>", color, match.Value);
        //                body = Regex.Replace(body, pattern, subtitution);
        //            }
        //            continue;
        //        }
        //    }

        //    return body;
        //}
    }
}