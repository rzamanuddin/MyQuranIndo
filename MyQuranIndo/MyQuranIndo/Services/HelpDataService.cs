using Java.Util;
using MyQuranIndo.Configuration;
using MyQuranIndo.Databases;
using MyQuranIndo.Models.Helps;
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
    public interface IHelpDataService
    {
        Task<IEnumerable<HelpHeader>> GetAsync(bool forceRefresh = false);
    }

    public class HelpDataService : IHelpDataService
    {
        private Database _database;
        private HttpClient _client;

        public HelpDataService()
        {
            _database = new Database();
            _client = new HttpClient();
        }

        public async Task<IEnumerable<HelpHeader>> GetAsync(bool forceRefresh = false)
        {
            var helps = new List<HelpHeader>();
            try
            {
                var current = Connectivity.NetworkAccess;

                // If internet connection is not available
                if (current != NetworkAccess.Internet)
                {

                    helps = await _database.GetHelpAsync();
                    return helps;
                }

                string url = Path.Combine(AppSetting.GetUrlHelp());

                var tokenSource = new CancellationTokenSource(3000);

                HttpResponseMessage response = await _client.GetAsync(url, tokenSource.Token);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    helps = JsonConvert.DeserializeObject<List<HelpHeader>>(content);
                }
                else
                {
                    helps = await _database.GetHelpAsync();
                }
            }
            catch (OperationCanceledException)
            {
                helps = await _database.GetHelpAsync();
                return helps;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return helps;
        }
    }
}
