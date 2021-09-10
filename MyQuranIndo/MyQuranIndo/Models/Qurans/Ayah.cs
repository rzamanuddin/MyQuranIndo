using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Fonts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Qurans
{
    public class Ayah : NotifyPropertyChanged
    {
        private Color rowColor;
        public int ID { get; set; }
        public int SurahID { get; set; }
        public string ReadText { get; set; }
        public string TextIndo { get; set; }
        public string TranslateIndo { get; set; }

        public int AyahID 
        { 
            get { return ID; }
        }
        public Color RowColor
        {
            get
            {                
                // Al fatihah and At Taubah
                if (SurahID == 1 || SurahID == 9)
                {
                    if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                    {
                        if (ID % 2 == 0)
                        {
                            rowColor = ((Color)Application.Current.Resources["RowColor"]);
                        }
                        else
                        {
                            rowColor = Color.White;
                        }
                    }
                    return rowColor;
                }
                else
                {
                    if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                    {
                        if (ID % 2 == 1)
                        {
                            rowColor =  ((Color)Application.Current.Resources["RowColor"]);
                        }
                        else
                        {
                            rowColor =  Color.White;
                        }
                    }
                    return rowColor;
                }
            }
            set
            {                
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
            }
        }
        public bool IsShowBismillah
        {
            get
            {
                // Show bismillah except Al-Fatihah and At-Taubah
                if ((SurahID == 1 && ID == 0)
                    || (SurahID == 9 && ID == 0)
                   )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool IsShowNumber
        {
            get
            {
                // If ayah number == -1 then hide number
                if (ID == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public TextAlignment TextAlignment
        {
            get
            {
                if (IsShowNumber)
                {
                    return TextAlignment.Start;
                }
                else
                {
                    return TextAlignment.Center;
                }
            }
        }


        public bool IsVisibleTranslate
        {
            get
            {
                if (IsShowNumber)
                {
                    return Preferences.Get(MyQuranIndo.References.Setting.IS_ACTIVE_TRANSLATE, true);
                }
                else
                {
                    return false;
                }
            }
        }

        public Thickness Padding
        {
            get
            {
                if (IsShowNumber)
                {
                    return new Thickness()
                    {
                        Left = 5,
                        Top = 10,
                        Right = 10,
                        Bottom = 5
                    };
                }
                else
                {
                    return new Thickness()
                    {
                        Left = 5,
                        Top = 10,
                        Right = 10,
                        Bottom = 20
                    };
                }
            }
        }
        public bool IsVisibleTransliteration
        {
            get
            {
                if (IsShowNumber)
                {
                    return Preferences.Get(MyQuranIndo.References.Setting.IS_ACTIVE_TRANSLITERATION, true);
                }
                else
                {
                    return false;
                }
            }
        }
        public double FontSizeArabic
        {
            get
            {
                return FontHelper.GetFontSizeArabic();
            }
        }

        
        public string ReadTajwidText
        {
            //get;set;
            get
            {
                string body = ReadText;
                //body = TajwidHelper.ReplaceTextToTajwidColor2(body, TajwidHelper.Gunnahs, TajwidHelper.COLOR_GUNNAH);
                //body = TajwidHelper.ReplaceTextToTajwidColor2(body, TajwidHelper.Qalqalahs, TajwidHelper.COLOR_QALQALAH);

                return body;
            }
            //{
            //    //return "إذا <span style=\"color: red;\">م&zwj;</span>&zwj;ا ط&zwj;<span style=\"color: red;\">م&zwj;</span>&zwj;حت إلى <span style=\"color: red;\">غ&zwj;</span>&zwj;اية";
            //    //return "وَالَّذِيْنَ يُؤْمِنُوْنَ بِمَآ اُ<span style='color: red;'><span style='color: red;'>نْز</span></span>ِلَ اِلَيْكَ وَمَآ اُ<span style='color: red;'><span style='color: red;'>نْز</span></span>ِلَ مِ<span style='color: green;'><span style='color: red;'>نْ ق</span></span>َ<span style='color: green;'>بْ</span>لِكَ ۚ وَبِالْاٰخِرَةِ هُمْ يُوْقِنُوْنَۗ";
            //    //return "وَالَّذِيْنَ يُؤْمِنُوْنَ بِمَآ اُ<span style='color: red;'><span style='color: red;'><span style='color: red;'><span style='color: red;'>نْزِ</span></span></span></span>لَ اِلَيْكَ وَمَآ اُ<span style='color: red;'><span style='color: red;'><span style='color: red;'><span style='color: red;'>نْزِ</span></span></span></span>لَ مِ<span style='color: green;'><span style='color: red;'>نْ قَ</span></span><span style='color: green;'>بْ</span>لِكَ ۚ وَبِالْاٰخِرَةِ هُمْ يُوْقِنُوْنَۗ";
            //}
            //get
            //{

            //    return "الَّذِيْ جَعَلَ لَكُمُ الْاَرْضَ فِرَا<span style='color: purple;'>شًا و</span>َّالسَّمَاۤءَ بِنَاۤءً  ۖوَّاَ<span style='color: red;'>نْزَ</span>لَ مِنَ السَّمَاۤءِ مَاۤ<span style='color: red;'>ءً ف</span>َاَخْرَجَ بِهٖ مِنَ الثَّمَرٰتِ رِزْ<span style='color: gray;'>قًا ل</span>َّكُمْ ۚ فَلَا تَ<span style='color: green;'>جْ</span>عَلُوْا لِلّٰهِ اَ<span style='color: red;'>نْدَ</span>ا<span style='color: purple;'>دًا و</span>َّاَ<span style='color: red;'>نْتُ</span>مْ تَعْلَمُوْنَ";
            //}


            //    //return "وَاِذْ اَخَذْنَا مِيْثَاقَكُمْ لَا تَسْفِكُوْنَ دِمَاۤءَكُمْ وَلَا تُخْرِجُوْنَ اَ<span style=\"color: red;\">نْف</span>ُسَكُ<span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\">مْ م</span></span></span></span></span></span>ِّ<span style=\"color: green;\"><span style=\"color: red;\">نْ د</span></span>ِيَارِكُمْ ۖ ثُ<span style=\"color: orange;\">مَّ</span> اَ<span style=\"color: green;\">قْ</span>رَرْتُمْ وَاَ<span style=\"color: red;\">نْت</span>ُمْ تَشْهَدُوْنَ";


            //    string body = ReadText;
            //    body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Gunnahs, TajwidHelper.COLOR_GUNNAH);
            //    body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Qalqalahs, TajwidHelper.COLOR_QALQALAH);
            //    body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamBigunnahs, TajwidHelper.COLOR_IDGHAM_BIGUNNAH);
            //    body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamBilagunnahs, TajwidHelper.COLOR_IDGHAM_BILAGUNNAH);
            //    body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Ikhfas, TajwidHelper.COLOR_IKHFA);
            //    body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.Iqlabs, TajwidHelper.COLOR_IQLAB);
            //    body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IdghamMimis, TajwidHelper.COLOR_IDGHAM_MIMI);
            //    body = TajwidHelper.ReplaceTextToTajwidColor(body, TajwidHelper.IkhfaSyafawis, TajwidHelper.COLOR_IKHFA_SYAFAWI);

            //    return body;
            //}
        }

        //public HtmlWebViewSource HtmlWebViewSource
        //{
        //    get
        //    {
        //        var htmlSource = new HtmlWebViewSource();
        //        var fontSize = FontHelper.GetFontSizeArabic();
        //        string alignment = "";
        //        if (ID == 0)
        //        {
        //            alignment = "center";
        //        }
        //        else
        //        {
        //            alignment = "right";
        //        }

        //        htmlSource.Html = "<style type=\"text/css\">" +
        //            "@font-face {" +
        //            "font-family: LPMQ;" +
        //            "src: url(\"Fonts/LPMQ.ttf\")" +
        //            "}" +
        //        "p {" +
        //            "font-family: LPMQ;" +
        //            $"font-size: {fontSize}px;" +
        //            $"text-align: {alignment};" +
        //            "line-height: 2.6;" +
        //            //"font-weight: bold;" +
        //            "</style>" +
        //        //$"<p>وَاِذْ اَخَذْنَا مِيْثَاقَكُمْ لَا تَسْفِكُوْنَ دِمَاۤءَكُمْ وَلَا تُخْرِجُوْنَ اَ<span style=\"color: red;\">نْف</span>ُسَكُ<span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\"><span style=\"color: #cccc00;\">مْ م</span></span></span></span></span></span>ِّ<span style=\"color: green;\"><span style=\"color: red;\">نْ د</span></span>ِيَارِكُمْ ۖ ثُ<span style=\"color: orange;\">مَّ</span> اَ<span style=\"color: green;\">قْ</span>رَرْتُمْ وَاَ<span style=\"color: red;\">نْت</span>ُمْ تَشْهَدُوْنَ</p>";
        //        $"<p>{ReadTajwidText}</p>";
        //        htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

        //        return htmlSource;
        //    }
        //}

        public override string ToString()
        {
            return ReadText;
        }
    }
}
