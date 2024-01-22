using Android.Bluetooth.LE;
using MyQuranIndo.Configuration;
using MyQuranIndo.Messages;
using MyQuranIndo.Models.Hadiths;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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


        Task<IList<HadithCategory>> GetRootAsync(bool forceRefresh = false);
        Task<IEnumerable<HadithCategoryData>> GetCategoriesAsync(bool forceRefresh = false);
        Task<HadithCategoryDetailResultAPI> GetCategoryDetailAsync(int categoryId, int page = 1, int perPage = 20, bool forceRefresh = false);
        Task<HadithCategoryDetailContent> GetCategoryDetailContentAsync(int id, bool forceRefresh = false);
        Task<HadithCategory> GetCategoryAsync(int categoryId, bool forceRefresh = false);
    }

    public class HadithDataService : IHadithDataService
    {
        private HttpClient _client;

        public HadithDataService()
        {
            _client = new HttpClient();
        }

        public async Task<IList<HadithCategory>> GetRootAsync(bool forceRefresh = false)
        {
            var current = Connectivity.NetworkAccess;
            IList<HadithCategory> hcs = null;

            try
            {
                if (current != NetworkAccess.Internet)
                {
                    throw new Exception(Message.MSG_DEVICE_NO_CONNECTIVITY);
                }

                string url = Path.Combine(AppSetting.GetUrlHadithCategory(), "categories/roots/?language=id");
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                hcs = JsonConvert.DeserializeObject<IList<HadithCategory>>(content);

                foreach (var hc in hcs)
                {
                    if (!hc.ParentId.HasValue)
                    {
                        hc.ParentId = 0;
                    }
                }
                return hcs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<HadithCategoryData>> GetCategoriesAsync(bool forceRefresh = false)
        {
            var current = Connectivity.NetworkAccess;
            IList<HadithCategoryData> hcds = new List<HadithCategoryData>();

            try
            {
                if (current != NetworkAccess.Internet)
                {
                    throw new Exception(Message.MSG_DEVICE_NO_CONNECTIVITY);
                }

                string url = Path.Combine(AppSetting.GetUrlHadithCategory(), "categories/list/?language=id");
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<IEnumerable<HadithCategory>>(content);
                var hcRoots = results.Where(x => x.ParentId == null);
                foreach (var hcRoot in hcRoots)
                {
                    var hcd = new HadithCategoryData(); 
                    hcd.ID = hcRoot.ID;
                    hcd.Title = hcRoot.Title;
                    hcd.TotalHadith = hcRoot.TotalHadith;
                    hcds.Add(hcd);
                }
                foreach (var hcd in hcds)
                {
                    hcd.Data = results.Where(x => x.ParentId == hcd.ID).ToList();
                }

                return hcds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HadithCategory> GetCategoryAsync(int categoryId, bool forceRefresh = false)
        {
            try
            {
                var categories = await GetCategoriesAsync(forceRefresh);
                foreach (var category in categories)
                {
                    var result = category.Data.FirstOrDefault(x => x.ID == categoryId);
                    if (result != null)
                    {
                        return result;
                    }
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<HadithCategoryDetailResultAPI> GetCategoryDetailAsync(int categoryId, int page = 1, int perPage = 20, bool forceRefresh = false)
        {
            var current = Connectivity.NetworkAccess;
            try
            {
                if (current != NetworkAccess.Internet)
                {
                    throw new Exception(Message.MSG_DEVICE_NO_CONNECTIVITY);
                }

                string url = Path.Combine(AppSetting.GetUrlHadithCategory(), $"hadeeths/list/?language=id&category_id={categoryId}&page={page}&per_page={perPage}");
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<HadithCategoryDetailResultAPI>(content);

                foreach (var hcd in results.Data)
                {
                    var hcdContent = await GetCategoryDetailContentAsync(hcd.ID);
                    hcd.Content = hcdContent;
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HadithCategoryDetailContent> GetCategoryDetailContentAsync(int id, bool forceRefresh = false)
        {
            var current = Connectivity.NetworkAccess;
            try
            {
                if (current != NetworkAccess.Internet)
                {
                    throw new Exception(Message.MSG_DEVICE_NO_CONNECTIVITY);
                }

                string url = Path.Combine(AppSetting.GetUrlHadithCategory(), $"hadeeths/one/?language=id&id={id}");
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<HadithCategoryDetailContent>(content);

                if (result != null)
                {
                    url = Path.Combine(AppSetting.GetUrlHadithCategory(), $"hadeeths/one/?language=ar&id={id}");
                    response = await _client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    content = await response.Content.ReadAsStringAsync();
                    var resultAr = JsonConvert.DeserializeObject<HadithCategoryDetailContent>(content);
                    if (resultAr != null)
                    {
                        result.HadeethArabic = resultAr.Hadeeth;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
