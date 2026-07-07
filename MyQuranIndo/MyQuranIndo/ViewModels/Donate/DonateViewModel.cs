using MyQuranIndo.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.Donate
{
    public class DonateViewModel : BaseViewModel
    {
        public ICommand CopyRekeningCommand => new Command(async () => {
            await Clipboard.SetTextAsync("7165037715");
            // await App.Current.MainPage.DisplayAlert("Berhasil", "Nomor rekening disalin", "OK");
            ToastService.Show("Berhasil salin nomor rekening BCA");
        });
        public ICommand CopyRekeningBNICommand => new Command(async () => {
            await Clipboard.SetTextAsync("0707175048");
            // await App.Current.MainPage.DisplayAlert("Berhasil", "Nomor rekening disalin", "OK");
            ToastService.Show("Berhasil salin nomor rekening BNI");
        });
        public ICommand CopyEmailAddressCommand => new Command(async () => {
            await Clipboard.SetTextAsync("rzamanuddin@gmail.com");
            ToastService.Show("Berhasil salin alamat email");
        });
        public ICommand CopyRekeningPayPalCommand => new Command(async () => {
            await Clipboard.SetTextAsync("rzamanuddin@gmail.com");
            ToastService.Show("Berhasil salin akun PayPal");
        });

        public ICommand CopyEWalletCommand => new Command(async () => {
            await Clipboard.SetTextAsync("085793445020");
            //await App.Current.MainPage.DisplayAlert("Berhasil", "Nomor E-Wallet disalin", "OK");
            ToastService.Show("Berhasil salin nomor e-wallet");
        });

        public DonateViewModel()
        {
            Title = "Donasi";
            //MailCommand = new Command(async () => await OnSendEmail());
        }

        //public ICommand ContactAdminCommand => new Command(async () => {
        //    await Clipboard.SetTextAsync("1234567890");
        //    await App.Current.MainPage.DisplayAlert("Berhasil", "Nomor rekening disalin", "OK");
        //});
    }
}
