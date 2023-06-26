using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Models.PrayerSchedules
{
    public class Coordinat
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }
        [JsonProperty("lon")]
        public double Longitude { get; set; }
        [JsonProperty("lintang")]
        public string Lintang { get; set; }
        [JsonProperty("bujur")]
        public string Bujur { get; set; }
    }
}
