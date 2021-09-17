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

namespace MyQuranIndo.ViewModels.AsmaulHusna
{
    public class AsmaulHusnaViewModel : BaseViewModel, IHasCollectionViewModel
    {
        public IHasCollectionView View { get; set; }
        private ICommand _searchCommand;

        public ObservableCollection<Models.AsmaulHusna.AsmaulHusna> AsmaulHusnas { get; }

        public ICommand LoadCommand { get; }
        public ICommand AsmaulHusnaOneTapped { get; }

        public AsmaulHusnaViewModel()
        {
            Title = "Asmaul Husna";
            AsmaulHusnas = new ObservableCollection<Models.AsmaulHusna.AsmaulHusna>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
            AsmaulHusnaOneTapped = new Command<Models.AsmaulHusna.AsmaulHusna>(OnAsmaulHusnaOneTapped);
        }

        private void ScrollToItem(int index, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(index - 1, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }
        private void ScrollToItem(Models.AsmaulHusna.AsmaulHusna asmaulHusna, bool isAnimated = false)
        {
            View.CollectionView.ScrollTo(asmaulHusna, -1, position: ScrollToPosition.Start, isAnimated); // don't forget check null
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                AsmaulHusnas.Clear();
                var ahs = await AsmaulHusnaDataService.GetAsmaulHusnasAsync();
                for (int i = 0; i < ahs.Count; i++)
                {
                    ahs[i].RowID = i + 1;
                    AsmaulHusnas.Add(ahs[i]);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_ASMAUL_HUSNA
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
                //if (Player.IsPlaying()) 
                //    await Player.Stop();
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
                        if (id > 0 && id <= AsmaulHusnas.Count)
                        {
                            ScrollToItem(id);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, warningMessage, Message.MSG_OK);
                        }
                    }
                    else
                    {
                        text = text.ToLower();
                        var s = AsmaulHusnas.FirstOrDefault(q => q.ArabicLatin.ToLower().Contains(text)
                                || q.TranslateID.ToLower().Contains(text));
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

        private bool IsValidTapped(Models.AsmaulHusna.AsmaulHusna ah)
        {
            if (ah == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///  On Asmaul Husna One Tapped
        /// </summary>
        /// <param name="ah"></param>
        private async void OnAsmaulHusnaOneTapped(Models.AsmaulHusna.AsmaulHusna ah)
        {
            if (IsValidTapped(ah))
            {
                //MP3Helper.StopPlayer();
                MP3Service.StopPlayer();
                var oldColor = ah.RowColor;
                ah.RowColor = (Color)Application.Current.Resources["SelectedItem"];

                string result = "";
                string errorMessage = "";
                string action = await ActionHelper.DisplayActionAsmaulHusnaAsync(Title);
                string copiedText = "";
                try
                {
                    switch (action)
                    {
                        case ActionHelper.ASMAUL_HUSNA_SHARE:
                            copiedText = ActionHelper.GetAsmaulHusnaToShare(ah, Title);
                            await ActionHelper.ShareAsmaulHusnaAsync(copiedText);
                            break;
                        case ActionHelper.ASMAUL_HUSNA_SHARE_ALL:
                            copiedText = ActionHelper.GetAsmaulHusnaAllToShare(AsmaulHusnas, Title);
                            await ActionHelper.ShareAsmaulHusnaAsync(copiedText);
                            break;
                        case ActionHelper.ASMAUL_HUSNA_COPY:
                            copiedText = ActionHelper.GetAsmaulHusnaToShare(ah, Title);
                            await Clipboard.SetTextAsync(copiedText);
                            result = ActionHelper.ASMAUL_HUSNA_COPY + " Berhasil.";
                            break;
                        case ActionHelper.ASMAUL_HUSNA_COPY_ALL:
                            copiedText = ActionHelper.GetAsmaulHusnaAllToShare(AsmaulHusnas, Title);
                            await Clipboard.SetTextAsync(copiedText);
                            result = ActionHelper.ASMAUL_HUSNA_COPY_ALL + " Berhasil.";
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
                    ah.RowColor = oldColor;
                }
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
