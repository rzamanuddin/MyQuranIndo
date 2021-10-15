using MyQuranIndo.Databases;
using MyQuranIndo.Models.Zikrs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IPrayDataService
    {
        Task<IEnumerable<PrayData>> GetPraysAsync(bool forceRefresh = false);
    }

    public class PrayDataService : IPrayDataService
    {
        private Database _database;

        public PrayDataService()
        {
            _database = new Database();
        }

        public async Task<IEnumerable<PrayData>> GetPraysAsync(bool forceRefresh = false)
        {
            List<PrayData> prays = null;

            try
            {
                prays = await _database.GetPrayAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return prays;
        }
    }
}
