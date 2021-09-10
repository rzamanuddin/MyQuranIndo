using MyQuranIndo.Messages;
using MyQuranIndo.Views.About;
using MyQuranIndo.Views.Home;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyQuranIndo.ViewModels.About
{
    public class AboutViewModel : BaseViewModel
    {
        private string emailText;
        private string currentVersion;
        public ICommand MailCommand { get; }

        public string EmailText
        {
            get => emailText;
            set => SetProperty(ref emailText, value);
        }

        public string CurrentVersion
        {
            get => currentVersion;
            set => SetProperty(ref currentVersion, value);
        }

        public AboutViewModel()
        {
            Title = "Info Aplikasi";
            MailCommand = new Command(async () => await OnSendEmail());
            VersionTracking.Track();
            CurrentVersion = VersionTracking.CurrentVersion;
        }

        private async Task OnSendEmail()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EmailText))
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_WARNING, "Silahkan isi saran / kritik.", "Ok");
                    return;
                }
                List<string> to = new List<string>();
                to.Add("rzamanuddin@gmail.com");

                var message = new EmailMessage
                {
                    Subject = "Saran / Kritik MyQuran Indonesia Free",
                    Body = EmailText,
                    To = to
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
                EmailText = "";
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, "Perangkat tidak support"
                    + Environment.NewLine + fbsEx.Message, "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, ex.Message, "Ok");
            }
        }
    }
}