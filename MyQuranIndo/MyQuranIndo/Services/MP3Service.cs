using MyQuranIndo.Helpers;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Qurans;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IMP3Service
    {
        Task<byte[]> DownloadAsync(string url);
        Task DownloadSurahAsync(Surah surah);
        Stream GetStreamFromFile(string filename);
        Task PlayMurottal(int surahID, int ayahID, IToastService toastService = null);

        void StopPlayer();

        bool IsPlaying();
        void Play(FileStream fs);
        void Pause();
        double GetCurrentPosition();
        double GetDuration();
        double GetBalance();
       // void Audio_PlaybackEnded(object sender, EventArgs e);


    }
    public class MP3Service : IMP3Service
    {
        private ISimpleAudioPlayer Player;
        public MP3Service()
        {
            Player = CrossSimpleAudioPlayer.Current;
        }
        public async Task<byte[]> DownloadAsync(string url)
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
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_DOWNLOAD_MP3 +
                    Environment.NewLine + ex.Message, Message.MSG_OK);
            }

            return result;
        }

        public Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("MyQuranIndo." + filename);

            return stream;
        }

        public async Task PlayMurottal(int surahID, int ayahID, IToastService toastService = null)
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
                StopPlayer();
                if (!File.Exists(fileName))
                {
                    if (toastService != null)
                        toastService.Show("Sedang mengunduh murottal", false);
                    mp3File = await DownloadAsync(Path.Combine(url, mp3Name));
                    File.WriteAllBytes(fileName, mp3File);
                    toastService.Show("Mainkan Murottal", false);
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    
                }
                else
                {
                    if (toastService != null)
                        toastService.Show("Mainkan Murottal", false);
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

        public async Task DownloadSurahAsync(Surah surah)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            for (int i = 1; i <= surah.NumberOfAyah; i++)
            {
                string mp3Name = surah.ID.ToString("000") + i.ToString("000") + ".mp3";
                string fileName = Path.Combine(filePath, ReciterHelper.GetReciter().ToString("000") + mp3Name);
                //string url = AppSetting.GetUrlMP3();
                string url = ReciterHelper.GetReciterUrl();

                FileStream fs = null;

                byte[] mp3File = null;
                try
                {
                    if (!File.Exists(fileName))
                    {
                        //toastService.Show($"Sedang mengunduh surat {surah.NameLatin} Ayat {i}", false);
                        mp3File = await DownloadAsync(Path.Combine(url, mp3Name));
                        File.WriteAllBytes(fileName, mp3File);
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

                // TODO: play all
            }

            //toastService.Show($"Surat {surah.NameLatin} berhasil diunduh", false);
        }

        public void StopPlayer()
        {
            if (Player.IsPlaying)
                Player.Stop();
        }

        public bool IsPlaying()
        {
            return Player.IsPlaying;
        }

        public void Play(FileStream fs)
        {
            Player.Load(fs);
            Player.Play(); 
            //Player.PlaybackEnded += Audio_PlaybackEnded;
        }

        public void Pause()
        {
            Player.Pause();
        }

        public double GetCurrentPosition()
        {
            return Player.CurrentPosition;
        }

        public double GetDuration()
        {
            return Player.Duration;
        }

        public double GetBalance()
        {
            
            return Player.Balance;
        }

        //public void Audio_PlaybackEnded(object sender, EventArgs e)
        //{
            
        //    //Check if next playlist element exists
        //    //if (Playlist.IndexOf(PlaylistSelectedItem) + 1 < Playlist.Count)
        //    //{
        //        //PlaylistSelectedItem = Playlist[Playlist.IndexOf(PlaylistSelectedItem) + 1];
        //        // (Player.IsPlaying) Player.Stop();
        //        //Player.Load(GetStreamFromFile());
        //        Player.Play();
        //        Player.PlaybackEnded += Audio_PlaybackEnded;
        //    //}
        //    //else if (Player.IsPlaying) Player.Stop();
        //}
    }
}
