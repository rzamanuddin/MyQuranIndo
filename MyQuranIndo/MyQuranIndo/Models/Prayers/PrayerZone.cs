using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Globalization;
using Xamarin.Forms;
using System.ComponentModel;

namespace MyQuranIndo.Models.Prayers
{
    public class PrayerTime : NotifyPropertyChanged
    {
        private FontAttributes fontAttributes;
        public string Name { get; set; }
        public string Time { get; set; }
        public int Row { get; set; }

        public Color RowColor
        {
            get
            {
                if (Row % 2 == 0)
                {
                    return ((Color)Application.Current.Resources["RowColor"]);
                }
                else
                {
                    return Color.White;
                }
            }
        }

        public FontAttributes FontAttributes
        {
            get
            {
                return fontAttributes;
            }
            set
            {
                fontAttributes = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FontAttributes)));
            }
        }
    }
    public class PrayZone
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("results")]
        public PrayerSchedule Results { get; set; }
        //public Location Location { get; set; }
        //public Setting settings { get; set; }
    }
    public class PrayerSchedule
    {
        [JsonProperty("datetime")]
        public List<DateTime> DateTime { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("settings")]
        public Setting Setting { get; set; }
    }
    public class DateTime
    {
        public Times Times { get; set; }
        public Date Date { get; set; }
    }
    public class Times
    {
        //     "datetime":[
        //{
        //	"times":
        //	{
        //		"Imsak":"05:18","Sunrise":"06:39","Fajr":"05:28","Dhuhr":"12:33","Asr":"15:54","Sunset":"18:27","Maghrib":"18:38","Isha":"-","Midnight":"23:58"

        //             },
        //	"date":
        //	{
        //		"timestamp":1614470400,"gregorian":"2021-02-28","hijri":"1442-07-16"
        //	}
        //}],
        public string Imsak { get; set; }
        public string Sunrise { get; set; }
        public string Fajr { get; set; }
        public string Dhuhr { get; set; }
        public string Asr { get; set; }
        public string Sunset { get; set; }
        public string Maghrib { get; set; }
        public string Isha { get; set; }
        public string Midnight { get; set; }
    }

    public class Date
    {
        private string[] hijriMonths = new string[]
        {
            "Muharram",
            "Shafar",
            "Rabi'ul Awwal",
            "Rabi'ul Akhir",
            "Jumadil",
            "Jumadil Akhir",
            "Rajab",
            "Sya'ban",
            "Ramadhan",
            "Syawal",
            "Dzulqo'dah",
            "Dzulhijjah"
        };

        [JsonProperty("timestamp")]
        public int TimeStamp { get; set; }

        [JsonProperty("gregorian")]
        public System.DateTime Gregorian { get; set; }

        [JsonProperty("hijri")]
        public System.DateTime Hijri { get; set; }

        public string HijriDate
        {
            get
            {
                //var date = DateTime.Now.ToString("dd/MMM/yyyy", new CultureInfo("ar"));
                return $"{Hijri.Day} {hijriMonths[Hijri.Month - 1]} {Hijri.Year}H";
            }
        }
    }

    public class Location
    {
        //     "location":
        //{
        //	"latitude":21.416667938232425,"longitude":39.816665649414062,"elevation":333.0,"country":"","country_code":"SA","timezone":"Asia/Riyadh","local_offset":3.0
        //},
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("elevation")]
        public double Elevation { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("timezone")]
        public string TimeZone { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("local_offset")]
        public double LocalOffset { get; set; }
    }

    public class Setting
    {

        //"settings":
        //{
        //	"timeformat":"HH:mm","school":"Ithna Ashari","juristic":"Shafii","highlat":"None","fajr_angle":18.5,"isha_angle":90.0
        //}
        [JsonProperty("timeformat")]
        public string TimeFormat { get; set; }

        [JsonProperty("school")]
        public string School { get; set; }

        [JsonProperty("juristic")]
        public string Juristic { get; set; }

        [JsonProperty("highlat")]
        public string Highlat { get; set; }

        [JsonProperty("fajr_angle")]
        public double FajrAngle { get; set; }

        [JsonProperty("isha_angle")]
        public double IshaAngle { get; set; }
    }
}
