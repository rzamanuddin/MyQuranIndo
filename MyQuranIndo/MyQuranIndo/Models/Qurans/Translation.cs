using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MyQuranIndo.Models.Qurans
{
    public class Translation
    {
        [JsonProperty("id")]
        public TranslationID ID { get; set; }
    }

    public class TranslationID
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("text")]
        public Dictionary<int, string> Text { get; set; }
    }
}
