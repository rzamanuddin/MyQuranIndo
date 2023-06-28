using MyQuranIndo.Configuration;
//using MyQuranIndo.Models.Prayers;
using MyQuranIndo.Models.PrayerSchedules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IPrayerScheduleDataService
    {
        //Task<PrayerSchedule> GetAsync(double latitude, double longitude, double? altitude, bool forceRefresh = false);
        Task<APIPrayerResult> GetAsync(APILocationResult locationsResult);
        Task<APILocationResult> GetLocationsAsync(string city);
    }

    public class PrayerScheduleDataService : IPrayerScheduleDataService
    {
        private HttpClient _client;

        public PrayerScheduleDataService()
        {
            _client = new HttpClient();
        }

        public async Task<APIPrayerResult> GetAsync(APILocationResult locationsResult)
        {
            if (locationsResult == null || locationsResult.Locations == null || locationsResult.Locations.Count == 0)
            {
                return null;
            }
            APIPrayerResult result = null;
            try
            {
                DateTime today = DateTime.Today;
                var location = locationsResult.Locations.FirstOrDefault();
                string url = Path.Combine(AppSetting.GetAPIUrlBase(), $"sholat/jadwal/{location.ID}/{today.Year}/{today.Month:00}/{today.Day:00}");                
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<APIPrayerResult>(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<APILocationResult> GetLocationsAsync(string city)
        {
            APILocationResult locationResults = null;
            try
            {
                var spilttedCity = city.Split(' ');
                foreach (var sc in spilttedCity)
                {
                    string url = Path.Combine(AppSetting.GetAPIUrlBase(), $"sholat/kota/cari/{sc}");
                    HttpResponseMessage response = await _client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        if (!content.ToLower().Trim().Contains("tidak ketemu"))
                        {
                            locationResults = JsonConvert.DeserializeObject<APILocationResult>(content);
                            return locationResults;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return locationResults;
        }

        //public async Task<PrayerSchedule> GetAsync(string city, bool forceRefresh = false)
        //public async Task<PrayerSchedule> GetAsync(double latitude, double longitude, double? altitude, bool forceRefresh = false)
        //{
        //    PrayerSchedule prayerSchedule = null;

        //    try
        //    {
        //        //string url = Path.Combine(AppSetting.GetAPIUrlBase(),
        //        //    $"?city={city}&date={System.DateTime.Now.ToString("yyyy-MM-dd")}&school=9"); // school = 9 Departemen kemajuan Islam, Malaysia (JAKIM)
        //        string url = Path.Combine(AppSetting.GetAPIUrlBase(),
        //            $"?latitude={latitude}&date={System.DateTime.Now.ToString("yyyy-MM-dd")}" +
        //            $"&longitude={longitude}&elevation={(altitude.HasValue ? altitude.Value : 0)}&school=9"); // school = 9 Departemen kemajuan Islam, Malaysia (JAKIM)
        //        HttpResponseMessage response = await _client.GetAsync(url);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string content = await response.Content.ReadAsStringAsync();
        //            var prayZone = JsonConvert.DeserializeObject<PrayZone>(content);
        //            prayerSchedule = prayZone.Results;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return prayerSchedule;
        //}
    }
}
