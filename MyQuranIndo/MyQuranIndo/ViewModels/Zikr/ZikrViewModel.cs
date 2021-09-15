using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Zikr
{
    [QueryProperty(nameof(ZikrTime), nameof(ZikrTime))]
    public class ZikrViewModel : BaseViewModel
    {
        public ObservableCollection<Models.Zikrs.Zikr> Zikrs { get; }
        
        public ICommand LoadCommand { get; }

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

        public ZikrViewModel()
        {            
            Title = "Dzikir Pagi (Beta)";
            Zikrs = new ObservableCollection<Models.Zikrs.Zikr>();
            LoadCommand = new Command(async () => await ExecuteLoadCommand());
        }

        private async Task ExecuteLoadCommand()
        {
            try
            {
                Zikrs.Clear();
                IEnumerable<Models.Zikrs.Zikr> zikrs = await ZikrDataService.GetZikrsDataService();

                if (this.ZikrTime == "0")
                {
                    zikrs = zikrs.Where(q => q.Time.Trim().ToLower() == "pagi" || string.IsNullOrEmpty(q.Time));
                    Title = "Dzikir Pagi (Beta)";
                }
                else if (this.ZikrTime == "1")
                {
                    zikrs = zikrs.Where(q => q.Time.Trim().ToLower() == "petang" || string.IsNullOrEmpty(q.Time));
                    Title = "Dzikir Petang (Beta)";
                }
                var zikrsMorning = zikrs.ToList();
                for (int i = 0; i < zikrsMorning.Count; i++)
                {
                    var zm = zikrsMorning[i];
                    zm.RowID = i + 1;
                    // Ayat Kursi
                    if (zm.ID == 1)
                    {
                        var ayatKursi = await AyahDataService.GetAsync(2, 255);
                        zm.ArabicLatin = ayatKursi.TextIndo;
                        zm.Arabic = ayatKursi.ReadText;
                    } // Al Ikhlas - An Naas
                    else if(zm.ID == 2) {
                        var alIkhlas = await AyahDataService.GetAsync(112);
                        var alFalaq = await AyahDataService.GetAsync(113);
                        var anNaas = await AyahDataService.GetAsync(114);
                        var bismillah = await AyahDataService.GetAsync(1, 1);
                        
                        zm.Arabic = "";
                        zm.TranslateID = "";
                        zm.ArabicLatin = "";

                        zm.Arabic += $"{bismillah.ReadText}\n";
                        zm.TranslateID += $"{bismillah.TranslateIndo}\n";
                        zm.ArabicLatin += $"{bismillah.TextIndo}\n";
                        for (int ayah = 0; ayah < alIkhlas.Count; ayah++)
                        {
                            zm.Arabic += $"{alIkhlas[ayah].ReadText}  ";
                            zm.TranslateID += $"{alIkhlas[ayah].TranslateIndo} ";
                            zm.ArabicLatin += $"{alIkhlas[ayah].TextIndo} ";
                        }
                        zm.Arabic += "\n";
                        zm.TranslateID += "\n";
                        zm.ArabicLatin += "\n";

                        zm.Arabic += $"{bismillah.ReadText}\n";
                        zm.TranslateID += $"{bismillah.TranslateIndo}\n";
                        zm.ArabicLatin += $"{bismillah.TextIndo}\n";
                        for (int ayah = 0; ayah < alFalaq.Count; ayah++)
                        {
                            zm.Arabic += $"{alFalaq[ayah]}  ";
                            zm.TranslateID += $"{alFalaq[ayah].TranslateIndo} ";
                            zm.ArabicLatin += $"{alFalaq[ayah].TextIndo} ";
                        }
                        zm.Arabic += "\n";
                        zm.TranslateID += "\n";
                        zm.ArabicLatin += "\n";

                        zm.Arabic += $"{bismillah.ReadText}\n";
                        zm.TranslateID += $"{bismillah.TranslateIndo}\n";
                        zm.ArabicLatin += $"{bismillah.TextIndo}\n";
                        for (int ayah = 0; ayah < anNaas.Count; ayah++)
                        {
                            zm.Arabic += $"{anNaas[ayah]}  ";
                            zm.TranslateID += $"{anNaas[ayah].TranslateIndo} ";
                            zm.ArabicLatin += $"{anNaas[ayah].TextIndo} ";
                        }
                    }

                    Zikrs.Add(zm);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_GET_ZIKR
                    + Environment.NewLine + ex.Message, Message.MSG_OK); ;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
