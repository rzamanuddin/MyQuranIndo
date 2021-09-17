using MyQuranIndo.Databases;
using MyQuranIndo.Models.AsmaulHusna;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IAsmaulHusnaDataService
    {
        Task<List<AsmaulHusna>> GetAsmaulHusnasAsync(bool forceRefresh = false);
    }
    public class AsmaulHusnaDataService : IAsmaulHusnaDataService
    {
        private Database _database;


        public AsmaulHusnaDataService()
        {
            _database = new Database();
        }

        /// <summary>
        /// Get zikrs data from json
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<List<AsmaulHusna>> GetAsmaulHusnasAsync(bool forceRefresh = false)
        {
            List<AsmaulHusna> zikrs = null;

            try
            {
                zikrs = await _database.GetAsmaulHusnasAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return zikrs;
        }
    }
}
