using MyQuranIndo.Configuration;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Hadiths;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyQuranIndo.Services
{
    public interface IHadithDataService
    {
        Task<IList<Narrator>> GetNarratorsAsync(bool forceRefresh = false);
        Task<Narrator> GetNarratorAsync(string slug, bool forceRefresh = false);
        Task<HadithResultAPI> GetHadithResultAPIAsync(string slug, int page, int limit, bool forceRefresh = false);
    }

    public class HadithDataService : IHadithDataService
    {
        private HttpClient _client;

        public HadithDataService()
        {
            _client = new HttpClient();
        }

        public async Task<HadithResultAPI> GetHadithResultAPIAsync(string slug, int page, int limit, bool forceRefresh = false)
        {
            var current = Connectivity.NetworkAccess;
            HadithResultAPI result = null;

            try
            {
                if (current != NetworkAccess.Internet)
                {
                    throw new Exception(Message.MSG_DEVICE_NO_CONNECTIVITY);
                }

                string url = Path.Combine(AppSetting.GetUrlHadith(), $"{slug}?page={page}&limit={limit}");
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<HadithResultAPI>(content);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<List<HadithData>> GetHadithDataAsync(string slug, int page, int limit, bool forceRefresh = false)
        //{
        //    var current = Connectivity.NetworkAccess;
        //    HadithResultAPI result = null;

        //    try
        //    {
        //        if (current != NetworkAccess.Internet)
        //        {
        //            throw new Exception(Message.MSG_DEVICE_NO_CONNECTIVITY);
        //        }

        //        string url = Path.Combine(AppSetting.GetUrlHadith(), $"{slug}?page={page}&limit={limit}");
        //        var response = await _client.GetAsync(url);
        //        response.EnsureSuccessStatusCode();
        //        var content = await response.Content.ReadAsStringAsync();
        //        result = JsonConvert.DeserializeObject<HadithResultAPI>(content);
        //        var hadithData = new List<HadithData>();
        //        foreach (var item in result.Items)
        //        {
        //            hadithData.Add(new HadithData { ID = result.})
        //        }
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<Narrator> GetNarratorAsync(string slug, bool forceRefresh = false)
        {
            try
            {
                var result = (await GetNarratorsAsync(forceRefresh)).FirstOrDefault(q => q.Slug.Trim().ToLower() == slug.Trim().ToLower());
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<Narrator>> GetNarratorsAsync(bool forceRefresh = false)
        {
            var current = Connectivity.NetworkAccess;
            IList<Narrator> narrators = null;

            try
            {
                if (current != NetworkAccess.Internet)
                {
                    throw new Exception(Message.MSG_DEVICE_NO_CONNECTIVITY);
                }

                string url = AppSetting.GetUrlHadith();
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                narrators = JsonConvert.DeserializeObject<IList<Narrator>>(content);

                return narrators;
            }
            catch (Exception)
            {
                throw;
            }     

        }
    }
}
