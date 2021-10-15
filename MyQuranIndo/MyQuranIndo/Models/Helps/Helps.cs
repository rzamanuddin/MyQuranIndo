using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Helps
{
    public class HelpHeader
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public List<HelpContent> Contents { get; set; }
    }

    public class HelpContent : NotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }

        public bool IsVisibleSubtitle
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Subtitle))
                {
                    return false;
                }
                else
                {
                    return true;
                }
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
    }

    public class HelpGroup : ObservableCollection<HelpContent>
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
                //return String.Format("{0} ({1} ayat)", SurahName, NumberOfAyah);
                return $"{TitleHeader}";
            }
        }
        public ObservableCollection<HelpContent> Helps { get; private set; }

        public HelpGroup(int id, string titleHeader, ObservableCollection<HelpContent> helps) : base(helps)
        {
            this.ID = id;
            this.TitleHeader = titleHeader;
            Helps = helps;
        }

        public override string ToString()
        {
            return TitleHeader;
        }
    }
}
