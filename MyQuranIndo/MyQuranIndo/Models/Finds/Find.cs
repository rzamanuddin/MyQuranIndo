using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Models.Themes;
using MyQuranIndo.References;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Finds
{
    public class Find : BaseGroupModel
    {
        private int theme = Preferences.Get(Setting.THEME, 0);
        private Color rowColor;
        public Ayah Ayah { get; set; }
        //public List<FindGroup> FindGroups { get; private set; } = new List<FindGroup>();
    }

    public class FindGroup : ObservableCollection<Find>
    {
        private bool isExpand = false;
        private string imageHeader = "expand.png";

        public int SurahID { get; set; }
        public string SurahName { get; set; }
        public int NumberOfAyah { get; set; }

        public bool IsExpand {
            get {
                return isExpand;
            }
            set
            {
                //if (value)
                //{
                //    ImageHeader = "collapse.png";
                //}
                //else
                //{
                //    ImageHeader = "expand.png";
                //}

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
                return String.Format("{0} ({1} ayat)", SurahName, NumberOfAyah);
            }
        }
        public ObservableCollection<Find> Finds { get; private set; }

        public FindGroup(int surahID, string surahName, int numberOfAyah, ObservableCollection<Find> finds) : base(finds)
        {
            if (Preferences.Get(MyQuranIndo.References.Setting.THEME, 0) == (int)ThemeStyle.Black)
            {
                this.imageHeader = "expand_black.png";
            }
            else
            {
                this.imageHeader = "expand.png";
            }

            this.SurahID = surahID;
            this.SurahName = surahName;
            this.NumberOfAyah = numberOfAyah;
            Finds = finds;
        }

        public override string ToString()
        {
            return SurahName;
        }
    }
}
