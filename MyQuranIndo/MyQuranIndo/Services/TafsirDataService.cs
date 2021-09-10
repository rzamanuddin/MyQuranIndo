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
        Task<Tafsir> GetAsync(int surahID, int ayahID);
    }
    public class TafsirDataService : ITafsirDataService
    {
        private DatabaseManager _database;

        public TafsirDataService()
        {
            _database = new DatabaseManager();
        }

        public async Task<Tafsir> GetAsync(int surahID, int ayahID)
        {
            Tafsir tafsir = null;

            try
            {
                tafsir = await _database.GetTafsir(surahID, ayahID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tafsir;
        }
    }
}
