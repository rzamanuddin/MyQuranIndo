using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Models.PrayerSchedules
{
    public class Location
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("lokasi")]
        public string LocationName { get; set; }

        [JsonProperty("daerah")]
        public string LocationZone { get; set; }

        [JsonProperty("koordinat")]
        public Coordinat Coordinat { get; set; }

        [JsonProperty("jadwal")]
        public PrayerSchedule Schedule { get; set; }

    }
}
