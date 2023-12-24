using MyQuranIndo.Configuration;
using MyQuranIndo.Databases;
using MyQuranIndo.Models.PrayerSchedules;
using MyQuranIndo.Models.Zikrs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyQuranIndo.Services
{
    public interface IPrayDataService
    {
        Task<IEnumerable<PrayData>> GetPraysAsync(bool forceRefresh = false);
    }

    public class PrayDataService : IPrayDataService
    {
        private Database _database;
        private HttpClient _client;

        public PrayDataService()
        {
            _database = new Database();
            _client = new HttpClient();
        }

        public async Task<IEnumerable<PrayData>> GetPraysAsync(bool forceRefresh = false)
        {
            List<PrayData> prays = null;

            try
            {
                var current = Connectivity.NetworkAccess;

                // If internet connection is not available
                if (current != NetworkAccess.Internet)
                {
                    prays = await _database.GetPrayAsync();
                    return prays;
                }

                string url = Path.Combine(AppSetting.GetUrlDoa());

                var tokenSource = new CancellationTokenSource(3000);

                HttpResponseMessage response = await _client.GetAsync(url, tokenSource.Token);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    prays = JsonConvert.DeserializeObject<List<PrayData>>(content);
                }
                else
                {
                    prays = await _database.GetPrayAsync();
                }
            }
            catch(OperationCanceledException)
            {
                prays = await _database.GetPrayAsync();
                return prays;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return prays;
        }
    }
}
