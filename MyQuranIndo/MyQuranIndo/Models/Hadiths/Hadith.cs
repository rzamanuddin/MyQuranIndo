using MyQuranIndo.Helpers;
using MyQuranIndo.Models.Finds;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Models.Zikrs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Hadiths
{
    public class HadithCategoryDetailContent
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("hadeeth")]
        public string Hadeeth { get; set; }
        public string HadeethArabic { get; set; }
        [JsonProperty("attribution")]
        public string Attribution { get; set; }
        [JsonProperty("grade")]
        public string Grade { get; set; }
        [JsonProperty("explanation")]
        public string Explanation { get; set; }
        [JsonProperty("hints")]
        public List<string> Hints { get; set; }
        [JsonProperty("categories")]
        public List<int> Categories { get; set; }

        public string SubTitle
        {
            get
            {
                return $"{Attribution} ({Grade})";
            }
        }

    }
    public class HadithCategoryDetail : NotifyPropertyChanged
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        public HadithCategoryDetailContent Content { get; set; } = new HadithCategoryDetailContent();

        private Color rowColor;
        public Color RowColor
        {
            get
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
            set
            {
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
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
                return TextAlignment.Start;
            }
        }

        public Thickness Padding
        {
            get
            {
                return new Thickness()
                {
                    Left = 5,
                    Top = 8,
                    Right = 10,
                    Bottom = 5
                };
            }
        }
    }

    public class HCDMetadata
    {
        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }
        [JsonProperty("last_page")]
        public int LastPage { get; set; }
        [JsonProperty("total_items")]
        public int TotalItems { get; set; }
        [JsonProperty("per_page")]
        public int PerPage { get; set; }
    }

    public class HadithCategoryDetailResultAPI
    {
        [JsonProperty("data")]
        public List<HadithCategoryDetail> Data { get; set; } = new List<HadithCategoryDetail>();
        [JsonProperty("meta")]
        public HCDMetadata Metadata { get; set; }
    }

    public class HadithCategoryData
    {

        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("total_hadith")]
        public int TotalHadith { get; set; }

        [JsonProperty("data")]
        public List<HadithCategory> Data { get; set; } = new List<HadithCategory>();
    }

    public class HadithCategoryGroup : ObservableCollection<HadithCategory>
    {
        private bool isExpand = false;
        private string imageHeader = "expand.png";

        public int ID { get; set; }
        public string TitleHeader { get; set; }
        public int TotalHadith { get; set; }

        public bool IsExpand
        {
            get
            {
                return isExpand;
            }
            set
            {
                isExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpand)));
            }
        }

        public string ImageHeader
        {
            get
            {
                return imageHeader;
            }
            set
            {
                imageHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ImageHeader)));
            }
        }

        public string FoundResult
        {
            get
            {
                return $"{TitleHeader} ({TotalHadith} hadis)";
            }
        }

        private Color rowColor;
        public Color RowColor
        {
            get
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
            set
            {
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
            }
        }

        public ObservableCollection<HadithCategory> HadithCategories { get; private set; }

        public HadithCategoryGroup(int? id, string title, int totalHadith, ObservableCollection<HadithCategory> hadithCategories) : base(hadithCategories)
        {
            this.ID = id ?? 0;
            this.TitleHeader = title;
            this.TotalHadith = totalHadith;
            HadithCategories = hadithCategories;
        }

        public override string ToString()
        {
            return TitleHeader;
        }
    }
        
    public class HadithCategory : NotifyPropertyChanged
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("hadeeths_count")]
        public int TotalHadith { get; set; }
        [JsonProperty("parent_id")]
        public int? ParentId { get; set; } = 0;

        public string Description
        {
            get => $"{TotalHadith} Hadis";
        }

        public string DisplaySubTitle
        {
            get
            {
                return $"{Title}";
            }
        }

        private Color rowColor;
        public Color RowColor
        {
            get
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
            set
            {
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
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
    }

    public class Pagination
    {
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        [JsonProperty("startPage")]
        public int StartPage { get; set; }
        [JsonProperty("endPage")]
        public int EndPage { get; set; }
        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }
        [JsonProperty("endIndex")]
        public int EndIndex { get; set; }
        [JsonProperty("pages")]
        public List<int> Pages { get; set; }
    }

    public class HadithData
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("data")]
        public List<Hadith> Data { get; set; }
    }

    public class Hadith : NotifyPropertyChanged
    {
        [JsonProperty("Number")]
        public int Number { get; set; }

        [JsonProperty("Arab")]
        public string Arabic { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        private Color rowColor;
        public Color RowColor
        {
            get
            {
                if (rowColor != ((Color)Application.Current.Resources["SelectedItem"]))
                {
                    if (Number % 2 == 0)
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
            set
            {
                rowColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(RowColor)));
            }
        }

        public double FontSizeArabic
        {
            get
            {
                return FontHelper.GetFontSizeArabic();
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

        public TextAlignment TextAlignment
        {
            get
            {
                return TextAlignment.Start;
            }
        }
    }

    public class HadithResultAPI : Narrator
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("items")]
        public List<Hadith> Items { get; set; }
    }

    public class HadithGroup : ObservableCollection<Hadith>
    {
        private bool isExpand = false;
        private string imageHeader = "expand.png";

        public int ID { get; set; }
        public string TitleHeader { get; set; }
        //public int NumberOfAyah { get; set; }

        public bool IsExpand
        {
            get
            {
                return isExpand;
            }
            set
            {
                isExpand = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpand)));
            }
        }

        public string ImageHeader
        {
            get
            {
                return imageHeader;
            }
            set
            {
                imageHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ImageHeader)));
            }
        }

        public string FoundResult
        {
            get
            {
                return $"{TitleHeader}";
            }
        }
        public ObservableCollection<Hadith> Hadiths { get; private set; }

        public HadithGroup(int id, string titleHeader, ObservableCollection<Hadith> hadiths) : base(hadiths)
        {
            this.ID = id;
            this.TitleHeader = titleHeader;
            //this.NumberOfAyah = numberOfAyah;
            Hadiths = hadiths;
        }

        public override string ToString()
        {
            return TitleHeader;
        }
    }
}
