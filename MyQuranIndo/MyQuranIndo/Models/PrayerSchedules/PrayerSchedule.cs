using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Models.PrayerSchedules
{
    public class PrayerSchedule
    {
        [JsonProperty("tanggal")]
        public string LongDate { get; set; }

        [JsonProperty("tanggal_hijriah")]
        public string HijriDate 
        {              
            get 
            {
                HijriCalendar hijri = new HijriCalendar();
                return $"{hijri.GetDayOfMonth(ShortDate)}/{hijri.GetMonth(ShortDate)}/{hijri.GetYear(ShortDate)}";
            }
        }

        [JsonProperty("imsak")]
        public string Imsak { get; set; }

        [JsonProperty("subuh")]
        public string Subuh { get; set; }
        [JsonProperty("terbit")]
        public string Terbit { get; set; }

        [JsonProperty("dhuha")]
        public string Dhuha { get; set; }

        [JsonProperty("dzuhur")]
        public string Dzuhur { get; set; }

        [JsonProperty("ashar")]
        public string Ashar { get; set; }

        [JsonProperty("maghrib")]
        public string Maghrib { get; set; }

        [JsonProperty("isya")]
        public string Isya { get; set; }

        [JsonProperty("date")]
        public DateTime ShortDate { get; set; }

    }
}
