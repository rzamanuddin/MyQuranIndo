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
using System.Threading;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IMP3Service
    {
        Task<byte[]> DownloadAsync(string url);
        Task DownloadSurahAsync(Surah surah);
        Stream GetStreamFromFile(string filename);
        Task PlayMurottal(int surahID, int ayahID, IToastService toastService = null, CancellationToken cancellationToken = default);

        void StopPlayer();

        bool IsPlaying();
        void Play(FileStream fs);
        void Pause();

        double GetCurrentPosition();
        double GetDuration();
        double GetBalance();
        // void Audio_PlaybackEnded(object sender, EventArgs e);
        ISimpleAudioPlayer Player { get; set; }
        bool Load(Stream audioStream);

        bool Load(string fileName);

        void Play();
        void Play(string fileName);

    }
    public class MP3Service : IMP3Service
    {
        // SINGLETON: Satu instance MP3Service di-share di seluruh aplikasi.
        // Sebelumnya DependencyService.Get<IMP3Service>() membuat instance baru setiap kali
        // dipanggil, menyebabkan ghost playback karena handler PlaybackEnded dari instance lama
        // masih ter-attach ke player yang sama tapi instance-nya sudah berbeda.
        private static MP3Service _instance;
        private static readonly object _lock = new object();

        public static MP3Service Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MP3Service();
                        }
                    }
                }
                return _instance;
            }
        }

        private ISimpleAudioPlayer player;

        // BUG FIX (performance): HttpClient sebelumnya dibuat baru (new HttpClient()) setiap kali
        // DownloadAsync dipanggil. Ini adalah anti-pattern yang terkenal di .NET karena setiap
        // instance HttpClient membuka socket/handler sendiri, dan pembuatan berulang bisa
        // menyebabkan socket exhaustion (SocketException: "too many open files" / port habis)
        // terutama saat DownloadSurahAsync memanggilnya puluhan-ratusan kali berturut-turut.
        // Solusi: gunakan satu instance static yang di-reuse untuk seluruh lifetime aplikasi.
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        // Batas jumlah download paralel agar tidak membanjiri koneksi/server saat DownloadSurahAsync
        // dipanggil untuk surat dengan banyak ayat (misal Al-Baqarah = 286 ayat).
        private const int MaxConcurrentDownloads = 4;

        public ISimpleAudioPlayer Player
        {
            get => this.player;
            set => this.player = value;
        }

        // Private constructor untuk singleton
        private MP3Service()
        {
            player = CrossSimpleAudioPlayer.Current;
        }


        public async Task<byte[]> DownloadAsync(string url)
        {
            byte[] result = null;

            try
            {
                // BUG FIX: HttpResponseMessage sekarang benar2 dibuang lewat using,
                // dan HttpClient di-reuse (lihat field _httpClient di atas).
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

        //public async Task PlayMurottal(int surahID, int ayahID, IToastService toastService = null)
        //{
        //    string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    string mp3Name = surahID.ToString("000") + ayahID.ToString("000") + ".mp3";
        //    string fileName = Path.Combine(filePath, ReciterHelper.GetReciter().ToString("000") + mp3Name);
        //    string url = ReciterHelper.GetReciterUrl();

        //    FileStream fs = null;

        //    byte[] mp3File = null;
        //    try
        //    {
        //        StopPlayer();
        //        if (!File.Exists(fileName))
        //        {
        //            if (toastService != null)
        //                toastService.Show("Sedang mengunduh murottal", false);

        //            // BUG FIX: sebelumnya menggunakan Path.Combine untuk menggabungkan URL.
        //            // Path.Combine memakai separator platform (bisa '\' di Windows) dan tidak
        //            // aman untuk membentuk URL. Sekarang digabung manual dengan '/'.
        //            mp3File = await DownloadAsync(CombineUrl(url, mp3Name));

        //            // BUG FIX: sebelumnya mp3File langsung ditulis ke file dan diputar tanpa
        //            // dicek null. Jika download gagal (mis. tidak ada koneksi internet atau
        //            // status HTTP bukan 200), DownloadAsync mengembalikan null, lalu
        //            // File.WriteAllBytes(fileName, null) akan melempar ArgumentNullException,
        //            // dan proses tetap lanjut mencoba memutar file yang sebenarnya gagal dibuat.
        //            // Sekarang: jika gagal, tampilkan pesan dan hentikan proses lebih awal.
        //            if (mp3File == null || mp3File.Length == 0)
        //            {
        //                if (toastService != null)
        //                    toastService.Show("Gagal mengunduh murottal", false);
        //                return;
        //            }

        //            File.WriteAllBytes(fileName, mp3File);

        //            // BUG FIX: sebelumnya toastService.Show dipanggil tanpa null check di sini,
        //            // padahal toastService adalah parameter opsional (default null). Jika method
        //            // dipanggil tanpa toastService, ini akan melempar NullReferenceException.
        //            if (toastService != null)
        //                toastService.Show("Mainkan Murottal", false);

        //            fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        }
        //        else
        //        {
        //            if (toastService != null)
        //                toastService.Show("Mainkan Murottal", false);
        //            fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        }

        //        try
        //        {
        //            player.Load(fs);
        //            player.Play();
        //            //player.PlaybackEnded += Player_PlaybackEnded;

        //        }
        //        catch (Exception ex)
        //        {
        //            await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 +
        //            Environment.NewLine + ex.Message, Message.MSG_OK);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 +
        //            Environment.NewLine + ex.Message, Message.MSG_OK);
        //    }
        //    finally
        //    {
        //        if (fs != null)
        //            fs.Dispose();
        //    }
        //}

        public async Task PlayMurottal(int surahID, int ayahID, IToastService toastService = null, CancellationToken cancellationToken = default)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string mp3Name = surahID.ToString("000") + ayahID.ToString("000") + ".mp3";
            string fileName = Path.Combine(filePath, ReciterHelper.GetReciter().ToString("000") + mp3Name);
            string url = ReciterHelper.GetReciterUrl();

            try
            {
                // Stop pemutaran sebelumnya
                StopPlayer();

                // Guard: Cek apakah task sudah dibatalkan sebelum lanjut
                cancellationToken.ThrowIfCancellationRequested();

                if (!File.Exists(fileName))
                {
                    if (toastService != null)
                        toastService.Show("Sedang mengunduh murottal", false);

                    byte[] mp3File = await DownloadAsync(CombineUrl(url, mp3Name));

                    // Guard: Cek pembatalan setelah download async selesai
                    cancellationToken.ThrowIfCancellationRequested();

                    if (mp3File == null || mp3File.Length == 0)
                    {
                        if (toastService != null)
                            toastService.Show("Gagal mengunduh murottal", false);
                        return;
                    }

                    File.WriteAllBytes(fileName, mp3File);
                }

                // Guard: Cek sekali lagi sebelum memuat audio ke player
                cancellationToken.ThrowIfCancellationRequested();

                // BUG FIX: player.Load(fileName) pada Android memanggil AssetManager.OpenFd()
                // yang hanya bisa membuka file dari folder assets APK, bukan filesystem biasa.
                // Solusi: buka file sebagai FileStream dan gunakan Load(Stream).
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    player.Load(fs);
                }

                // Cek pembatalan tepat sebelum Play()
                if (cancellationToken.IsCancellationRequested)
                {
                    player.Stop();
                    return;
                }

                player.Play();
            }
            catch (OperationCanceledException)
            {
                // Task dibatalkan karena user pindah surat / stop — abaikan exception ini
                StopPlayer();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Message.MSG_TITLE_ERROR, Message.MSG_FAIL_PLAY_MP3 +
                    Environment.NewLine + ex.Message, Message.MSG_OK);
            }
        }

        public async Task DownloadSurahAsync(Surah surah)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string url = ReciterHelper.GetReciterUrl();
            int reciter = ReciterHelper.GetReciter();

            // PERFORMANCE FIX: sebelumnya setiap ayat diunduh secara sequential (satu per satu,
            // menunggu ayat sebelumnya selesai total). Untuk surat panjang (contoh: Al-Baqarah,
            // 286 ayat) ini sangat lambat karena tidak memanfaatkan latency jaringan yang bisa
            // di-overlap. Sekarang diunduh paralel dengan batas MaxConcurrentDownloads memakai
            // SemaphoreSlim, supaya tetap cepat tapi tidak membanjiri koneksi/server.
            using (var semaphore = new SemaphoreSlim(MaxConcurrentDownloads))
            {
                var tasks = new List<Task>();

                for (int i = 1; i <= surah.NumberOfAyah; i++)
                {
                    int ayahNumber = i; // hindari closure capture bug pada variabel loop
                    tasks.Add(DownloadSingleAyahAsync(filePath, url, reciter, surah.ID, ayahNumber, semaphore));
                }

                await Task.WhenAll(tasks);
            }

            //toastService.Show($"Surat {surah.NameLatin} berhasil diunduh", false);
        }

        private async Task DownloadSingleAyahAsync(string filePath, string url, int reciter, int surahId, int ayahNumber, SemaphoreSlim semaphore)
        {
            string mp3Name = surahId.ToString("000") + ayahNumber.ToString("000") + ".mp3";
            string fileName = Path.Combine(filePath, reciter.ToString("000") + mp3Name);

            if (File.Exists(fileName))
                return;

            await semaphore.WaitAsync();
            try
            {
                byte[] mp3File = await DownloadAsync(CombineUrl(url, mp3Name));

                // BUG FIX: sama seperti di PlayMurottal, cek null/kosong sebelum menulis ke disk
                // supaya tidak melempar exception dan tidak menyisakan file kosong/korup.
                if (mp3File != null && mp3File.Length > 0)
                {
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
                semaphore.Release();
            }
        }

        // Helper untuk menggabungkan base URL dan nama file MP3 tanpa bergantung pada
        // separator path platform (lihat catatan BUG FIX di atas).
        private static string CombineUrl(string baseUrl, string fileName)
        {
            if (string.IsNullOrEmpty(baseUrl))
                return fileName;

            return baseUrl.TrimEnd('/') + "/" + fileName.TrimStart('/');
        }

        public void StopPlayer()
        {
            try
            {
                if (player != null)
                {
                    if (player.IsPlaying)
                    {
                        player.Stop();
                    }
                    //player?.Dispose();
                    //player = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"StopPlayer Exception: {ex.Message}");
            }
        }

        public bool IsPlaying()
        {
            return player.IsPlaying;
        }

        public void Play(FileStream fs)
        {
            player.Load(fs);
            player.Play();
            //Player.PlaybackEnded += Audio_PlaybackEnded;
        }

        public void Pause()
        {
            player.Pause();
        }

        public double GetCurrentPosition()
        {
            return player.CurrentPosition;
        }

        public double GetDuration()
        {
            return player.Duration;
        }

        public double GetBalance()
        {

            return player.Balance;
        }

        public bool Load(Stream audioStream)
        {
            return player.Load(audioStream);
        }

        public bool Load(string fileName)
        {
            return player.Load(fileName);
        }

        public void Play()
        {
            player.Play();
        }

        public void Play(string fileName)
        {
            player.Load(fileName);
            player.Play();
        }
    }
}