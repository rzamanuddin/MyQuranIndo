using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using MyQuranIndo.Models;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyQuranIndo.Models.Qurans;
using MyQuranIndo.Models.Zikrs;

namespace MyQuranIndo.Databases
{
    public class Database
    {
        public async Task<List<Zikr>> GetZikrsAsync()
        {
            // Get the assembly this code is executing in
            var assembly = Assembly.GetExecutingAssembly();

            // Look up the resource names and find the one that ends with settings.json
            // Your resource names will generally be prefixed with the assembly's default namespace
            // so you can short circuit this with the known full name if you wish
            var resName = assembly.GetManifestResourceNames().Where(q => q.StartsWith("MyQuranIndo.Databases"))
                .FirstOrDefault(q => q.EndsWith("Zikr.json"));

            // Load the resource file
            var file = assembly.GetManifestResourceStream(resName);

            string json = "";

            // Stream reader to read the whole file
            using (var sr = new StreamReader(file))
            {
                // Read the json from the file
                json = await sr.ReadToEndAsync();
            }

            // Parse out the JSON
            var zikrs = JsonConvert.DeserializeObject<ListOfZikr>(json);

            return zikrs.data;
        }

        //public async Task<Surah> GetSurahAsync(int id)
        //{
        //    // Get the assembly this code is executing in
        //    var assembly = Assembly.GetExecutingAssembly();

        //    // Look up the resource names and find the one that ends with settings.json
        //    // Your resource names will generally be prefixed with the assembly's default namespace
        //    // so you can short circuit this with the known full name if you wish
        //    var resName = assembly.GetManifestResourceNames().Where(q => q.StartsWith("MyQuranIndo.Databases"))
        //        .FirstOrDefault(q => q.EndsWith(id.ToString("000") + ".json"));

        //    // Load the resource file
        //    var file = assembly.GetManifestResourceStream(resName);

        //    string json = "";

        //    // Stream reader to read the whole file
        //    using (var sr = new StreamReader(file))
        //    {

        //        // Read the json from the file
        //        json = await sr.ReadToEndAsync();
        //    }

        //    // Parse out the JSON
        //    var surah = JsonConvert.DeserializeObject<Dictionary<int, Surah>>(json);

        //    return surah[id];
        //}

        //public async Task<Dictionary<int, Surah>> GetSurahAsync()
        //{
        //    // Get the assembly this code is executing in
        //    var assembly = Assembly.GetExecutingAssembly();

        //    var surah = new Dictionary<int, Surah>();

        //    //for (int i = 1; i <= 114; i++)
        //    //{
        //    //    // Look up the resource names and find the one that ends with settings.json
        //    // Your resource names will generally be prefixed with the assembly's default namespace
        //    // so you can short circuit this with the known full name if you wish
        //    //var resName = assembly.GetManifestResourceNames()
        //    //    ?.FirstOrDefault(r => r.EndsWith(i.ToString() + ".json", StringComparison.OrdinalIgnoreCase));

        //    var resourceNames = assembly.GetManifestResourceNames().Where(q => q.StartsWith("MyQuranIndo.Databases"));

        //    int i = 1;
        //    foreach (var resName in resourceNames)
        //    {
        //        // Load the resource file
        //        var file = assembly.GetManifestResourceStream(resName);

        //        string json = "";

        //        // Stream reader to read the whole file
        //        using (var sr = new StreamReader(file))
        //        {
        //            // Read the json from the file
        //            json = await sr.ReadToEndAsync();
        //        }

        //        // Parse out the JSON
        //        var s = JsonConvert.DeserializeObject<Dictionary<int, Surah>>(json)[i];

        //        // Add Bismillah on every surah except al fatihah
        //        //s.Text.Insert(0, new KeyValuePair<int, string>(0, "بِسْمِ اللّٰهِ الرَّحْمٰنِ الرَّحِيْمِ"));

        //        surah.Add(i, s);
        //        //}

        //        i++;
        //    }

        //    return surah;
        //}

        //public async Task<List<Surah>> GetSurahNewAsync()
        //{
        //    // Get the assembly this code is executing in
        //    var assembly = Assembly.GetExecutingAssembly();

        //    // Look up the resource names and find the one that ends with settings.json
        //    // Your resource names will generally be prefixed with the assembly's default namespace
        //    // so you can short circuit this with the known full name if you wish
        //    var resName = assembly.GetManifestResourceNames().Where(q => q.StartsWith("MyQuranIndo.Databases"))
        //        .FirstOrDefault(q => q.EndsWith("SurahNew.json"));

        //    // Load the resource file
        //    var file = assembly.GetManifestResourceStream(resName);

        //    string json = "";

        //    // Stream reader to read the whole file
        //    using (var sr = new StreamReader(file))
        //    {
        //        // Read the json from the file
        //        json = await sr.ReadToEndAsync();
        //    }

        //    // Parse out the JSON
        //    var surahNews = JsonConvert.DeserializeObject<List<Surah>>(json);

        //    return surahNews;
        //}

        //public async Task<List<Ayah>> GetAyahAsync()
        //{
        //    // Get the assembly this code is executing in
        //    var assembly = Assembly.GetExecutingAssembly();

        //    var ayahs = new List<Ayah>();
        //    for (int i = 1; i <= 114; i++)
        //    {
        //        var resName = assembly.GetManifestResourceNames()
        //            .FirstOrDefault(q => q.StartsWith("MyQuranIndo.Databases." + i.ToString("000") + ".json"));


        //        // Load the resource file
        //        var file = assembly.GetManifestResourceStream(resName);

        //        string json = "";

        //        // Stream reader to read the whole file
        //        using (var sr = new StreamReader(file))
        //        {
        //            // Read the json from the file
        //            json = await sr.ReadToEndAsync();
        //        }

        //        ayahs.AddRange(JsonConvert.DeserializeObject<List<Ayah>>(json));

        //    }

        //    return ayahs;
        //}

        //public async Task<List<Ayah>> GetAyahAsync(int surahID)
        //{
        //    // Get the assembly this code is executing in
        //    var assembly = Assembly.GetExecutingAssembly();

        //    // Look up the resource names and find the one that ends with settings.json
        //    // Your resource names will generally be prefixed with the assembly's default namespace
        //    // so you can short circuit this with the known full name if you wish
        //    var resName = assembly.GetManifestResourceNames().Where(q => q.StartsWith("MyQuranIndo.Databases"))
        //        .FirstOrDefault(q => q == "MyQuranIndo.Databases." + surahID.ToString("000") + ".json");

        //    // Load the resource file
        //    var file = assembly.GetManifestResourceStream(resName);

        //    string json = "";

        //    // Stream reader to read the whole file
        //    using (var sr = new StreamReader(file))
        //    {
        //        // Read the json from the file
        //        json = await sr.ReadToEndAsync();
        //    }
        //    // Parse out the JSON
        //    var ayahs = JsonConvert.DeserializeObject<List<Ayah>>(json);

        //    return ayahs;
        //}

        //public async Task<Tafsir> GetTafsirNew(int surahID, int ayahID)
        //{
        //    // Get the assembly this code is executing in
        //    var assembly = Assembly.GetExecutingAssembly();

        //    // Look up the resource names and find the one that ends with settings.json
        //    // Your resource names will generally be prefixed with the assembly's default namespace
        //    // so you can short circuit this with the known full name if you wish
        //    var resName = assembly.GetManifestResourceNames().Where(q => q.StartsWith("MyQuranIndo.Databases"))
        //        .FirstOrDefault(q => q.EndsWith("Tafsir" + surahID.ToString("000") + ".json"));

        //    // Load the resource file
        //    var file = assembly.GetManifestResourceStream(resName);

        //    string json = "";

        //    // Stream reader to read the whole file
        //    using (var sr = new StreamReader(file))
        //    {

        //        // Read the json from the file
        //        json = await sr.ReadToEndAsync();
        //    }

        //    // Parse out the JSON
        //    var tafsirs = JsonConvert.DeserializeObject<List<Tafsir>>(json);
        //    var tafsir = tafsirs.FirstOrDefault(q => q.SurahID == surahID && q.ID == ayahID);

        //    return tafsir;
        //}

        //public async Task<Surah> GetSurahAsync(int id)
        //{
        //    // Get the assembly this code is executing in
        //    var assembly = Assembly.GetExecutingAssembly();

        //    // Look up the resource names and find the one that ends with settings.json
        //    // Your resource names will generally be prefixed with the assembly's default namespace
        //    // so you can short circuit this with the known full name if you wish
        //    var resName = assembly.GetManifestResourceNames().Where(q => q.StartsWith("MyQuranIndo.Databases"))
        //        .FirstOrDefault(q => q.EndsWith(id.ToString("000") + ".json"));

        //    // Load the resource file
        //    var file = assembly.GetManifestResourceStream(resName);

        //    string json = "";

        //    // Stream reader to read the whole file
        //    using (var sr = new StreamReader(file))
        //    {

        //        // Read the json from the file
        //        json = await sr.ReadToEndAsync();
        //    }

        //    // Parse out the JSON
        //    var surah = JsonConvert.DeserializeObject<Dictionary<int, Surah>>(json);

        //    return surah[id];
        //}
    }
}
