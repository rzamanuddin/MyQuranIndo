using MyQuranIndo.Messages;
using MyQuranIndo.Models.Helps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Help
{
    public class HelpViewModel : BaseViewModel
    {

        private string searchQuery;
        private string notFoundMessage;
        private bool isVisibleNotFoundMessage;
        private Color foundResultColor;
        private HelpContent selectedHelp;
        private bool isExpandAll = true;
        private bool isVisibleExpandAll = false;

        //public ObservableCollection<Ayah> Ayahs { get; }
        public ObservableCollection<HelpGroup> Helps { get; set; }
        public ObservableCollection<HelpGroup> HelpsCopied { get; set; }

        public string SearchQuery
        {
            get => searchQuery;
            set => SetProperty(ref searchQuery, value);
        }

        public string NotFoundMessage
        {
            get => notFoundMessage;
            set => SetProperty(ref notFoundMessage, value);
        }

        public bool IsVisibleNotFoundMessage
        {
            get => isVisibleNotFoundMessage;
            set => SetProperty(ref isVisibleNotFoundMessage, value);
        }

        public Color FoundResultColor
        {
            get => foundResultColor;
            set => SetProperty(ref foundResultColor, value);
        }

        public bool IsExpandAll
        {
            get => isExpandAll;
            set
            {
                SetProperty(ref isExpandAll, value);
            }
        }
        public bool IsVisibleExpandAll
        {
            get => isVisibleExpandAll;
            set
            {
                SetProperty(ref isVisibleExpandAll, value);
            }
        }

        public Command LoadCommand { get; }
        public Command<HelpGroup> HeaderTapped { get; }
        public Command ExpandAllCommand { get; set; }

        public ICommand ClearSearchCommand
        {
            get;
        }

        //public FindViewModel(bool emptyGroups = false)
        public HelpViewModel()
        {
            Title = "Petunjuk Penggunaan";
            Helps = new ObservableCollection<HelpGroup>();
            HelpsCopied = new ObservableCollection<HelpGroup>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            ClearSearchCommand = new Command(ClearSearch);
            HeaderTapped = new Command<HelpGroup>(OnHeaderTapped);
            ExpandAllCommand = new Command(OnExpandAllComand);
            FoundResultColor = Color.Black;
        }

        private void OnExpandAllComand()
        {
            if (IsExpandAll)
            {
                for (int i = 0; i < Helps.Count; i++)
                {
                    Helps[i].Clear();
                }
                IsExpandAll = false;
            }
            else
            {
                for (int i = 0; i < HelpsCopied.Count; i++)
                {
                    Helps.Add(HelpsCopied[i]);
                }
                IsExpandAll = true;
            }
        }

        private void OnHeaderTapped(HelpGroup helpGroup)
        {
            helpGroup.IsExpand = !helpGroup.IsExpand;

            if (helpGroup.IsExpand)
            {
                var index = Helps.IndexOf(helpGroup);
                var context = HelpsCopied[index];
                for (int i = 0; i < context.Count; i++)
                {
                    helpGroup.Add(context[i]);
                }
                helpGroup.ImageHeader = "collapse.png";
            }
            else
            {
                helpGroup.Clear();
                helpGroup.ImageHeader = "expand.png";
            }
        }

        private void ClearSearch()
        {
            if (Helps != null)
            {
                Helps.Clear();
            }

            if (HelpsCopied != null)
            {
                HelpsCopied.Clear();
            }
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                NotFoundMessage = "";
                IsVisibleNotFoundMessage = false;
                FoundResultColor = Color.Gray;

                IEnumerable<HelpHeader> helpsData = await HelpDataService.GetAsync(true);
                
                if (!String.IsNullOrWhiteSpace(SearchQuery))
                {
                    helpsData = helpsData.Where(q => q.Title.Trim().ToLower().Contains(SearchQuery.ToLower())
                        || q.Contents.Any(s => s.Subtitle.Trim().ToLower().Contains(SearchQuery.ToLower())
                        || q.Contents.Any(c => c.Content.Trim().ToLower().Contains(SearchQuery.ToLower()))
                        ));
                }

                Helps.Clear();
                HelpsCopied.Clear();
                ObservableCollection<HelpContent> helpList = new ObservableCollection<HelpContent>();

                //var surahGroup = ayahs.ToLookup(s => s.SurahID);
                var helpGroup = helpsData.ToList().ToLookup(q => q.ID);
                int index = 0;
                foreach (var helpContent in helpGroup)
                {
                    helpList = new ObservableCollection<HelpContent>();
                    var helpInGroup = helpsData.Where(q => q.ID == helpContent.Key).ToList();
                    for (int i = 0; i < helpInGroup.Count; i++)
                    {
                        for (int r = 0; r < helpInGroup[i].Contents.Count; r++)
                        {
                            helpList.Add(helpInGroup[i].Contents[r]);
                        }
                    }

                    Helps.Add(new HelpGroup(helpContent.Key, helpsData.ToList()[index].Title, helpList));
                    HelpsCopied.Add(new HelpGroup(helpContent.Key, helpsData.ToList()[index].Title, helpList));

                    index++;
                }

                if (Helps.Count == 0)
                {
                    NotFoundMessage = Message.MSG_NOT_FOUND_FIND;
                    IsVisibleNotFoundMessage = true;
                    FoundResultColor = Color.Red;
                }
                else
                {
                    FoundResultColor = Color.Black;
                }

                IsVisibleExpandAll = Helps.Count > 0;

                for (int i = 0; i < Helps.Count; i++)
                {
                    Helps[i].Clear();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_FIND
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool IsValidTapped(HelpContent helpContent)
        {
            if (helpContent == null)
            {
                return false;
            }

            // Dont show pop up menu at ta'awudz
            if (helpContent.ID == 0)
            {
                return false;
            }

            return true;
        }

        public void OnAppearing()
        {
            IsBusy = true;
            selectedHelp = null;
        }
    }
}
