using MyQuranIndo.Databases;
using MyQuranIndo.Models.Qurans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IAyahDataService
    {
        Task<List<Ayah>> GetAsync(bool forceRefresh = false);
        Task<List<Ayah>> GetAsync(int surahID, bool forceRefresh = false);
        Task<Ayah> GetAsync(int surahID, int ayahID, bool forceRefresh = false);
    }

    public class AyahDataServices : IAyahDataService
    {
        private DatabaseManager _database;

        public AyahDataServices()
        {
            _database = new DatabaseManager();
        }

        public async Task<List<Ayah>> GetAsync(int surahID, bool forceRefresh = false)
        {
            List<Ayah> ayahs = null;

            try
            {
                ayahs = await _database.GetAyahAsync(surahID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ayahs;
        }

        public async Task<Ayah> GetAsync(int surahID, int ayahID, bool forceRefresh = false)
        {
            Ayah ayah = null;

            try
            {
                ayah = await _database.GetAyahAsync(surahID, ayahID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ayah;
        }

        public async Task<List<Ayah>> GetAsync(bool forceRefresh = false)
        {
            List<Ayah> ayahs = null;

            try
            {
                ayahs = await _database.GetAyahAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ayahs;
        }
    }
}
