using MyQuranIndo.Configuration;
using MyQuranIndo.Databases;
using MyQuranIndo.Models.Zikrs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;
using MyQuranIndo.Models.Prayers;
using System.IO;

namespace MyQuranIndo.Services
{

    public interface IPrayerReadDataService
    {
        Task<IEnumerable<PrayerRead>> GetPrayerReads(bool forceRefresh = false);
    }
    public class PrayerDataService : IPrayerReadDataService
    {
        private Database _database;
        private HttpClient _client;

        public PrayerDataService()
        {
            _database = new Database();
            _client = new HttpClient();
        }

        public async Task<IEnumerable<PrayerRead>> GetPrayerReads(bool forceRefresh = false)
        {
            IEnumerable<PrayerRead> results;
            try
            {
                var current = Connectivity.NetworkAccess;

                // If internet connection is not available
                if (current != NetworkAccess.Internet)
                {
                    results = await _database.GetPrayerReadsAsync();
                    return results;
                }

                string url = Path.Combine(AppSetting.GetUrlPrayerRead());

                var tokenSource = new CancellationTokenSource(3000);

                HttpResponseMessage response = await _client.GetAsync(url, tokenSource.Token);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<List<PrayerRead>>(content);
                }
                else
                {
                    results = await _database.GetPrayerReadsAsync();
                }
            }
            catch (OperationCanceledException)
            {
                results = await _database.GetPrayerReadsAsync();
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;
        }
    }
}
