using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MyQuranIndo.Configuration
{
    public class AppSetting
    {
        public enum FormMode : short
        {
            List = 0,
            Add = 1,
            Edit = 2
        }

        private const string API_URL_BASE = "apiUrlBase";
        private const string API_URL_PLAY_STORE = "urlPlayStore";
        private const string APP_NAME = "applicationName";
        private const string API_URL_MP3 = "urlMP3";
        private const string API_URL_MP3_AL_AFASY = "urlMP3AlAfasy";
        private const string API_URL_MP3_AL_MATROUD = "urlMP3AlMatroud";
        //private const string API_URL_LOGIN = "apiLogin";
        //private const string API_URL_VOTE = "apiVote";

        private static string GetJsonSetting(string key)
        {            
            // Get the assembly this code is executing in
            var assembly = Assembly.GetExecutingAssembly();

            // Look up the resource names and find the one that ends with settings.json
            // Your resource names will generally be prefixed with the assembly's default namespace
            // so you can short circuit this with the known full name if you wish
            var resName = assembly.GetManifestResourceNames()
                ?.FirstOrDefault(r => r.EndsWith("settings.json", StringComparison.OrdinalIgnoreCase));

            // Load the resource file
            var file = assembly.GetManifestResourceStream(resName);

            // Stream reader to read the whole file
            var sr = new StreamReader(file);

            // Read the json from the file
            var json = sr.ReadToEnd();

            // Parse out the JSON
            var j = JObject.Parse(json);
            return j.Value<string>(key);
        }

        public static string GetAPIUrlBase()
        {
            return GetJsonSetting(API_URL_BASE);
        }

        public static string GetUrlPlayStore()
        {
            return GetJsonSetting(API_URL_PLAY_STORE);
        }

        public static string GetApplicationName()
        {
            return GetJsonSetting(APP_NAME);
        }

        public static string GetUrlMP3()
        {
            return GetJsonSetting(API_URL_MP3);
        }

        public static string GetUrlMP3AlAfasy()
        {
            return GetJsonSetting(API_URL_MP3_AL_AFASY);
        }

        public static string GetUrlMP3AlMatroud()
        {
            return GetJsonSetting(API_URL_MP3_AL_MATROUD);
        }

        //public static string GetAPICategories()
        //{
        //    return $"{GetAPIUrlBase() + GetJsonSetting(API_URL_CATEGORY)}";
        //}

        //public static string GetAPIVotings()
        //{
        //    return $"{GetAPIUrlBase() + GetJsonSetting(API_URL_VOTING)}";
        //}

        //public static string GetAPIUsers()
        //{
        //    return $"{GetAPIUrlBase() + GetJsonSetting(API_URL_USER)}";
        //}

        //public static string GetAPILogin()
        //{
        //    return $"{GetAPIUrlBase() + GetJsonSetting(API_URL_LOGIN)}";
        //}

        //public static string GetAPIVote()
        //{
        //    return $"{GetAPIUrlBase() + GetJsonSetting(API_URL_VOTE)}";
        //}
    }
}
