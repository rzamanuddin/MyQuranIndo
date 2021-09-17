using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Zikr
{
    public class PrayViewModel : BaseViewModel, IHasCollectionViewModel
    {
        private ICommand _searchCommand;

        public ObservableCollection<Models.Zikrs.Pray> Prays { get; }

        public ICommand LoadCommand { get; }

        public ICommand PrayOneTapped { get; }

        private string zikrTime;
        public string ZikrTime
        {
            get
            {
                return zikrTime;
            }
            set
            {
                zikrTime = value;
            }
        }
        public double FontSizeArabic
        {
            get
            {
                return FontHelper.GetFontSizeArabic();
            }
        }
                
        public IHasCollectionView View { get; set; }

        public PrayViewModel()
        {
            Title = "Kumpulan Do'a";
            Prays = new ObservableCollection<Models.Zikrs.Pray>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            //AyahOneTapped = new Command<Ayah>(OnAyahOneTapped);
            PrayOneTapped = new Command<Models.Zikrs.Pray>(OnPrayOneTapped);
        }

        private void ScrollToItem(int index, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(index, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }
        private void ScrollToItem(Models.Zikrs.Pray pray, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(pray, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                Prays.Clear();
                var prays = await PrayDataService.GetPraysAsync();

                for (int i = 0; i < prays.Count; i++)
                {
                    prays[i].RowID = i + 1;
                    Prays.Add(prays[i]);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_PRAY
                    + Environment.NewLine + ex.Message, Message.MSG_OK); ;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>(async (text) =>
        {
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                {
                    ScrollToItem(1);
                }
                else
                {
                    int id = 0;
                    int.TryParse(text, out id);
                    string warningMessage = Message.MSG_NOT_FOUND_KEY.Replace("<text>", text);
                    if (id > 0)
                    {
                        if (id > 0 && id <= Prays.Count)
                        {
                            ScrollToItem(id - 1);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        var s = Prays.FirstOrDefault(q => q.Title.ToLower().Contains(text));
                        if (s != null)
                        {
                            ScrollToItem(s);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET
                    + Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                IsBusy = false;
            }
        }));

        private bool IsValidTapped(Models.Zikrs.Pray pray)
        {
            if (pray == null)
            {
                return false;
            }

            // Dont show pop up menu at ta'awudz
            if (pray.ID == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// On Zikr tapped one
        /// </summary>
        /// <param name="pray"></param>
        private async void OnPrayOneTapped(Models.Zikrs.Pray pray)
        {
            if (IsValidTapped(pray))
            {
                //MP3Helper.StopPlayer();
                MP3Service.StopPlayer();
                var oldColor = pray.RowColor;
                pray.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                string result = "";
                string errorMessage = "";
                //var surah = await SurahDataService.GetSurahAsync(zikr.SurahID);
                string action = await ActionHelper.DisplayActionPrayAsync(pray.TitleAndNumber);
                string zikrCopied = "";
                try
                {
                    switch (action)
                    {
                        case ActionHelper.PRAY_SHARE:
                            zikrCopied = ActionHelper.GetPrayToShare(pray, this.Title);
                            await ActionHelper.ShareZikrAsync(zikrCopied);
                            break;
                        case ActionHelper.PRAY_SHARE_ALL:
                            zikrCopied = ActionHelper.GetPraysAllToShare(Prays, this.Title);
                            await ActionHelper.ShareZikrAsync(zikrCopied);
                            break;
                        case ActionHelper.PRAY_COPY:
                            zikrCopied = ActionHelper.GetPrayToShare(pray, this.Title);
                            await Clipboard.SetTextAsync(zikrCopied);
                            result = ActionHelper.PRAY_COPY + " Berhasil.";
                            break;
                        case ActionHelper.PRAY_COPY_ALL:
                            zikrCopied = ActionHelper.GetPraysAllToShare(Prays, this.Title);
                            await Clipboard.SetTextAsync(zikrCopied);
                            result = ActionHelper.PRAY_COPY + " Berhasil.";
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        ToastService.Show(result);
                    }
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, errorMessage, Message.MSG_OK);
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_POP_UP_MENU + Environment.NewLine
                        + ex.Message, Message.MSG_OK);
                }
                finally
                {
                    pray.RowColor = oldColor;
                }
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
