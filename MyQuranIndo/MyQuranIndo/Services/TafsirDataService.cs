using MyQuranIndo.Databases;
using MyQuranIndo.Models.Qurans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface ITafsirDataService
    {
        Task<List<Tafsir>> GetAsync(bool forceRefresh = false);
        Task<List<Tafsir>> GetAsync(int surahID, bool forceRefresh = false);
        Task<Tafsir> GetAsync(int surahID, int ayahID, bool forceRefresh = false);
    }
    public class TafsirDataService : ITafsirDataService
    {
        private DatabaseManager _database;

        public TafsirDataService()
        {
            _database = new DatabaseManager();
        }

        public async Task<Tafsir> GetAsync(int surahID, int ayahID, bool forceRefresh = false)
        {
            Tafsir tafsir = null;

            try
            {
                tafsir = await _database.GetTafsirAsync(surahID, ayahID);
                return tafsir;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Tafsir>> GetAsync(bool forceRefresh = false)
        {
            List<Tafsir> tafsirs = null;

            try
            {
                tafsirs = await _database.GetTafsirAsync();
                return tafsirs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Tafsir>> GetAsync(int surahID, bool forceRefresh = false)
        {
            List<Tafsir> tafsirs = null;

            try
            {
                tafsirs = await _database.GetTafsirAsync(surahID);
                return tafsirs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
