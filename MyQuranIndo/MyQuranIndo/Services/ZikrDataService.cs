using Android.Runtime;
using MyQuranIndo.Configuration;
using MyQuranIndo.Databases;
using MyQuranIndo.Models.Prayers;
using MyQuranIndo.Models.Zikrs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyQuranIndo.Services
{
    public interface IZikrDataService
    {
        Task<List<Zikr>> GetZikrsAsync(bool forceRefresh = false);
    }
    public class ZikrDataService : IZikrDataService
    {
        private Database _database;
        private readonly HttpClient _client;

        public ZikrDataService()
        {
            _database = new Database();
            _client = new HttpClient();
        }

        /// <summary>
        /// Get zikrs data from json
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<List<Zikr>> GetZikrsAsync(bool forceRefresh = false)
        {
            List<Zikr> results = null;

            try
            {
                var current = Connectivity.NetworkAccess;

                // If internet connection is not available
                if (current != NetworkAccess.Internet)
                {
                    results = await _database.GetZikrsAsync();
                    return results;
                }

                string url = Path.Combine(AppSetting.GetUrlZikr());

                var tokenSource = new CancellationTokenSource(3000);

                HttpResponseMessage response = await _client.GetAsync(url, tokenSource.Token);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var zikrs = JsonConvert.DeserializeObject<ListOfZikr>(content);
                    results = zikrs.data;
                }
                else
                {
                    results = await _database.GetZikrsAsync();
                }
            }
            catch (OperationCanceledException)
            {
                results = await _database.GetZikrsAsync();
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
