using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Android.Content.Res.Resources;

namespace MyQuranIndo.Models.Qurans
{
    public class JuzDetail : NotifyPropertyChanged
    {
        private Color rowColor;
        private int theme = Preferences.Get(Setting.THEME, 0);

        public int JuzID { get; set; }
        public int SurahID { get; set; }
        public string SurahNameLatin { get; set; }
        public int SurahNumberOfAyah { get; set; }
        public string SurahTranslateIndo { get; set; }
        public int AyahID { get; set; }
        public string ReadText { get; set; }
        public string ReadTextUthmani { get; set; }
        public string TextIndo { get; set; }
        public string TranslateIndo { get; set; }
        public string HtmlRead { get; set; }
        public string ReadTajwidText { get; set; }

        public string FontArabicName
        {
            get
            {
                return FontHelper.GetFontArabicName();
            }
        }

        public string ReadTextArabic
        {
            get
            {
                if (RasmHelper.GetRasmType() == (int)RasmType.Utsmani)
                {
                    return ReadTextUthmani;
                }
                else
                {
                    return ReadText;
                }
            }
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
                        if (AyahID % 2 == 0)
                        {
                            rowColor = ((Color)Application.Current.Resources["RowColor"]);
                        }
                        else
                        {
                            if (theme == (int)ThemeStyle.Black)
                            {
                                rowColor = ((Color)Application.Current.Resources["RowColor"]);
                            }
                            else
                            {
                                rowColor = Color.White;
                            }
                        }
                    }
                    return rowColor;
                }
                else
                {
                    if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                    {
                        if (AyahID % 2 == 1)
                        {
                            rowColor = ((Color)Application.Current.Resources["RowColor"]);
                        }
                        else
                        {
                            if (theme == (int)ThemeStyle.Black)
                            {
                                rowColor = ((Color)Application.Current.Resources["RowColor"]);
                            }
                            else
                            {
                                rowColor = Color.White;
                            }
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
        public bool IsVisibleTranslate
        {
            get
            {
                return Preferences.Get(MyQuranIndo.References.Setting.IS_ACTIVE_TRANSLATE, true);
            }
        }

        public bool IsVisibleTransliteration
        {
            get
            {
                return Preferences.Get(MyQuranIndo.References.Setting.IS_ACTIVE_TRANSLITERATION, true);
            }
        }
        public bool IsShowNumber
        {
            get
            {
                if (AyahID == 0)
                {
                    return false;
                }
                else
                {
                    return true;
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
        public double FontSizeArabic
        {
            get
            {
                return FontHelper.GetFontSizeArabic(true);
            }
        }

        public double FontSizeTranslate
        {
            get
            {
                return FontHelper.GetFontSizeTranslate();
            }
        }

        public double FontSizeTextIndo
        {
            get
            {
                return FontHelper.GetFontSizeTextIndo();
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
        public double LineHeight
        {
            get
            {
                if (RasmHelper.GetRasmType() == (int)RasmType.Utsmani)
                {
                    return 1.1;
                }
                else
                {
                    return 1.4;
                }
            }
        }

        public Ayah ToAyah()
        {
            var ayah = new Ayah()
            {
                ID = AyahID,
                ReadText = ReadText,
                SurahID = SurahID,
                TextIndo = TextIndo,
                TranslateIndo = TranslateIndo,
                ReadTextUthmani = ReadTextUthmani
            };

            return ayah;
        }
    }


    public class SurahGroup : ObservableCollection<JuzDetail>
    {
        public int SurahID { get; set; }
        public string SurahName { get; set; }
        public int NumberOfAyah { get; set; }

        public string FoundResult
        {
            get
            {
                return String.Format($"{SurahName} ({NumberOfAyah} Ayat)");
            }
        }

        public ObservableCollection<JuzDetail> JuzDetails { get; private set; }

        public SurahGroup(int surahID, string surahName, int numberOfAyah, ObservableCollection<JuzDetail> juzDetails) : base(juzDetails)
        {
            this.SurahID = surahID;
            this.SurahName = surahName;
            this.NumberOfAyah = numberOfAyah;
            JuzDetails = juzDetails;
        }

        public override string ToString()
        {
            return SurahName;
        }
    }

    public class JuzHeader : BaseModel
    {
        public static int JUZ_COUNT = 30;
        public int SurahIDStart { get; set; }
        public string SurahNameStart { get; set; }
        public int AyahIDStart { get; set; }
        public int SurahIDEnd { get; set; }
        public string SurahNameEnd { get; set; }
        public int AyahIDEnd { get; set; }
        public int TotalAyah { get; set; }

        public string Description
        {
            get
            {
                return $"{SurahNameStart} Ayat {AyahIDStart} - {SurahNameEnd} {AyahIDEnd}{Environment.NewLine}(Total Ayat {TotalAyah})";
            }
        }
        public override string ToString()
        {
            return Description;
        }
    }
}
