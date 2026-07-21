using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MyQuranIndo.Models.Hadiths
{
    public class Narrator : BaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("total")]
        public int TotalHadith { get; set; }
        public string Description
        {
            get => $"Total Hadis {TotalHadith}";
        }

        public string DisplaySubTitle
        {
            get
            {
                return String.Format($"H.R. {Name}, {TotalHadith} Hadis.");
            }
        }
    }
}
