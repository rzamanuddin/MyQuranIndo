using System;
using System.Collections.Generic;
using System.Text;
using Plugin.SimpleAudioPlayer;
using System.IO;
using System.Reflection;
using System.Net.Http;
using System.Net;
using MyQuranIndo.Configuration;
using MyQuranIndo.Messages;
using System.Threading.Tasks;
using Xamarin.Essentials;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Services;

namespace MyQuranIndo.Helpers
{
    public static class MP3Helper
    {
        public static ISimpleAudioPlayer Player = CrossSimpleAudioPlayer.Current;


        private static Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("MyQuranIndo." + filename);

            return stream;
        }

        private static async Task<byte[]> DownloadAsync(string url)
        {
            byte[] result = null;

            var _httpClient = new HttpClient(); //{ Timeout = TimeSpan.FromSeconds(15) };

            try
            {
                using (var httpResponse = await _httpClient.GetAsync(url))
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        result = await httpResponse.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await App .Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_DOWNLOAD_MP3 +
                    Environment.NewLine + ex.Message, Message.MSG_OK);
            }

            return result;
        }

        public static async Task PlayMurottal(int surahID, int ayahID, IToastService toastService)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string mp3Name = surahID.ToString("000") + ayahID.ToString("000") + ".mp3";
            string fileName = Path.Combine(filePath, ReciterHelper.GetReciter().ToString("000") + mp3Name);
            //string url = AppSetting.GetUrlMP3();
            string url = ReciterHelper.GetReciterUrl();            

            FileStream fs = null;

            byte[] mp3File = null;
            try
            {
                if (!File.Exists(fileName))
                {
                    toastService.Show("Sedang mengunduh murottal.", false);
                    mp3File = await DownloadAsync(Path.Combine(url, mp3Name));
                    File.WriteAllBytes(fileName, mp3File);
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                }
                else
                {
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                }

                try
                {
                    Player.Load(fs);
                    Player.Play();                    
                    //player.PlaybackEnded += Player_PlaybackEnded;
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 +
                    Environment.NewLine + ex.Message, Message.MSG_OK);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 +
                    Environment.NewLine + ex.Message, Message.MSG_OK);
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        public static void StopPlayer()
        {
            if (Player.IsPlaying)
                Player.Stop();
        }
    }
}
