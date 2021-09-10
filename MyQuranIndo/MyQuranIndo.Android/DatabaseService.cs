using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MyQuranIndo.Droid;
using MyQuranIndo.Databases;
using Android.Content.Res;
using System.Threading.Tasks;
using System.Reflection;
using MyQuranIndo.Models.Qurans;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: Dependency(typeof(DatabaseService))]
namespace MyQuranIndo.Droid
{
    public class DatabaseService : IDBInterface
    {
        public DatabaseService()
        {

        }

        public SQLiteAsyncConnection CreateConnection()
        {
            var sqliteFilename = "MyQuran.db";
            var configurationFile = "config.json";
            string documentsDirectoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsDirectoryPath, sqliteFilename);
            var pathConfig = Path.Combine(documentsDirectoryPath, configurationFile);

            // If version does not exists then copy the database to document directory path
            if (!File.Exists(pathConfig))
            {
                WriteFile(pathConfig, configurationFile);
                WriteFile(path, sqliteFilename);
            }
            else
            {
                string json = File.ReadAllText(pathConfig);
                var j = JObject.Parse(json);
                string oldVersion = j.Value<string>("DatabaseVersion");

                string newVersion = "";

                // Check version
                using (var sr = new StreamReader(Android.App.Application.Context.Assets.Open(configurationFile)))
                {
                    json = sr.ReadToEnd();

                    // Parse out the JSON
                    j = JObject.Parse(json);
                    newVersion = j.Value<string>("DatabaseVersion");
                }

                // Check version database, if different then copy to document folder path
                if (newVersion != oldVersion)
                {
                    WriteFile(pathConfig, configurationFile);
                    WriteFile(path, sqliteFilename);
                }
            }
            //var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var conn = new SQLiteAsyncConnection(path);

            return conn;
        }

        private void WriteFile(string path, string fileName)
        {
            using (var binaryReader = new BinaryReader(Android.App.Application.Context.Assets.Open(fileName)))
            {                
                using (var binaryWriter = new BinaryWriter(new FileStream(path, FileMode.Create)))
                {
                    byte[] buffer = new byte[2048];
                    int length = 0;
                    while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        binaryWriter.Write(buffer, 0, length);
                    }
                }
            }
        }

        void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            while (bytesRead >= 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }
    }
}