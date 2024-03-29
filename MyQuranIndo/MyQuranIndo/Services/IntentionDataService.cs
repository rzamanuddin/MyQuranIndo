﻿using Java.Util;
using MyQuranIndo.Configuration;
using MyQuranIndo.Databases;
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
    public interface IIntentionDataService
    {
        Task<IEnumerable<Intention>> GetIntentions(bool forceRefresh = false);
    }
    public class IntentionDataService : IIntentionDataService
    {
        private Database _database;
        private HttpClient _client;

        public IntentionDataService()
        {
            _database = new Database();
            _client = new HttpClient();
        }

        public async Task<IEnumerable<Intention>> GetIntentions(bool forceRefresh = false)
        {
            IEnumerable<Intention> results;
            try
            {
                var current = Connectivity.NetworkAccess;

                // If internet connection is not available
                if (current != NetworkAccess.Internet)
                {
                    results = await _database.GetIntentionsAsync();
                    return results;
                }

                string url = Path.Combine(AppSetting.GetUrlIntention());

                var tokenSource = new CancellationTokenSource(3000);

                HttpResponseMessage response = await _client.GetAsync(url, tokenSource.Token);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<List<Intention>>(content);
                }
                else
                {
                    results = await _database.GetIntentionsAsync();
                }
            }
            catch (OperationCanceledException)
            {
                results = await _database.GetIntentionsAsync();
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
