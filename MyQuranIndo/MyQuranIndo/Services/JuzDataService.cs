using MyQuranIndo.Databases;
using MyQuranIndo.Models.Qurans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IJuzDataService
    {
        Task<List<JuzHeader>> GetJuzHeaderAsync(bool forceRefresh = false);
        Task<JuzHeader> GetJuzHeaderAsync(int id, bool forceRefresh = false);
        Task<List<JuzDetail>> GetJuzDetailAsync(int id, bool forceRefresh = false);
        Task<int> GetJuzIDAsync(int surahID, int ayahID, bool forceRefresh = false);
    }
    public class JuzDataService : IJuzDataService
    {
        private DatabaseManager _database;

        public JuzDataService()
        {
            _database = new DatabaseManager();
        }

        public async Task<List<JuzDetail>> GetJuzDetailAsync(int id, bool forceRefresh = false)
        {
            List<JuzDetail> juzDetails = new List<JuzDetail>();
            try
            {
                juzDetails = await _database.GetJuzDetailAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return juzDetails;
        }

        public async Task<List<JuzHeader>> GetJuzHeaderAsync(bool forceRefresh = false)
        {
            List<JuzHeader> juzHeaders = new List<JuzHeader>();
            try
            {
                juzHeaders = await _database.GetJuzHeaderAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return juzHeaders;
        }

        public async Task<JuzHeader> GetJuzHeaderAsync(int id, bool forceRefresh = false)
        {
            var juzHeader = new JuzHeader();
            try
            {
                juzHeader = await _database.GetJuzHeaderAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return juzHeader;
        }

        public async Task<int> GetJuzIDAsync(int surahID, int ayahID, bool forceRefresh = false)
        {
            var juzID = 0;
            try
            {
                juzID = await _database.GetJuzDetailAsync(surahID, ayahID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return juzID;
        }
    }
}
