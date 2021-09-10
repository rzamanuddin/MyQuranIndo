using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using MyQuranIndo.Models.Qurans;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MyQuranIndo.Databases
{
    public class DatabaseManager
    {
        SQLiteAsyncConnection connection;

        public DatabaseManager()
        {
            connection = DependencyService.Get<IDBInterface>().CreateConnection();
        }

        public async Task<List<Surah>> GetSurahAsync()
        {
            return await connection.Table<Surah>().ToListAsync();
        }

        public async Task<JuzHeader> GetJuzHeaderAsync(int id)
        {
            string query = "SELECT * FROM JuzHeader WHERE ID=?";
            var juzHeader = await connection.FindWithQueryAsync<JuzHeader>(query, id);
            
            return juzHeader;
        }

        public async Task<List<JuzHeader>> GetJuzHeaderAsync()
        {
            // string query = "SELECT * FROM JuzHeader";
            var juzHeaders = await connection.Table<JuzHeader>().ToListAsync();

            return juzHeaders;
        }

        public async Task<List<JuzDetail>> GetJuzDetailAsync(int id)
        {
            string query = "SELECT * FROM VJuzDetail WHERE JuzID=?";
            var juzDetails = await connection.QueryAsync<JuzDetail>(query, id);
            
            return juzDetails;
        }

        public async Task<int> GetJuzDetailAsync(int surahID, int ayahID)
        {
            string query = "SELECT * FROM JuzDetail WHERE SurahID=? AND AyahID=?";
            var juzID = await connection.ExecuteScalarAsync<int>(query, surahID, ayahID);

            return juzID;
        }

        public async Task<Surah> GetSurahAsync(int id)
        {
            var surah = await connection.FindAsync<Surah>(id);
            return surah;
        }


        public async Task<List<Ayah>> GetAyahAsync()
        {
            return await connection.Table<Ayah>().ToListAsync();
        }

        public async Task<List<Ayah>> GetAyahAsync(int surahID)
        {
            var ayahs = await connection.QueryAsync<Ayah>("SELECT * FROM Ayah WHERE SurahID = ?", surahID.ToString());
            return ayahs;
        }
        public async Task<Ayah> GetAyahAsync(int surahID, int ayahID)
        {
            var ayah = await connection.FindWithQueryAsync<Ayah>($"SELECT * FROM Ayah WHERE SurahID={surahID} AND ID={ayahID}");
            return ayah;
        }

        public async Task<Tafsir> GetTafsir(int surahID, int ayahID)
        {
            var tafsir = await connection.FindWithQueryAsync<Tafsir>($"SELECT * FROM Tafsir WHERE SurahID={surahID} AND ID={ayahID}");
            return tafsir;
        }
    }
}
