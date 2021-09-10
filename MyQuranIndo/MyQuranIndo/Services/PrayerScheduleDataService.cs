using MyQuranIndo.Configuration;
using MyQuranIndo.Models.Prayers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IPrayerScheduleDataService
    {
        Task<PrayerSchedule> GetAsync(double latitude, double longitude, double? altitude, bool forceRefresh = false);
    }

    public class PrayerScheduleDataService : IPrayerScheduleDataService
    {
        private HttpClient _client;

        public PrayerScheduleDataService()
        {
            _client = new HttpClient();
        }

        //public async Task<PrayerSchedule> GetAsync(string city, bool forceRefresh = false)
        public async Task<PrayerSchedule> GetAsync(double latitude, double longitude, double? altitude, bool forceRefresh = false)
        {
            PrayerSchedule prayerSchedule = null;

            try
            {
                //string url = Path.Combine(AppSetting.GetAPIUrlBase(),
                //    $"?city={city}&date={System.DateTime.Now.ToString("yyyy-MM-dd")}&school=9"); // school = 9 Departemen kemajuan Islam, Malaysia (JAKIM)
                string url = Path.Combine(AppSetting.GetAPIUrlBase(),
                    $"?latitude={latitude}&date={System.DateTime.Now.ToString("yyyy-MM-dd")}" +
                    $"&longitude={longitude}&elevation={(altitude.HasValue ? altitude.Value : 0)}&school=9"); // school = 9 Departemen kemajuan Islam, Malaysia (JAKIM)

                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var prayZone = JsonConvert.DeserializeObject<PrayZone>(content);
                    prayerSchedule = prayZone.Results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return prayerSchedule;
        }
    }
}
