using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Models.PrayerSchedules
{
    public class APILocationResult : APIResult
    {
        [JsonProperty("data")]
        public List<Location> Locations { get; set; } 
    }
}
