using MyQuranIndo.Databases;
using MyQuranIndo.Models.Zikrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IZikrDataService
    {
        Task<List<Zikr>> GetZikrsDataService(bool forceRefresh = false);
    }
    public class ZikrDataService : IZikrDataService
    {
        private Database _database;


        public ZikrDataService()
        {
            _database = new Database();
        }

        /// <summary>
        /// Get zikrs data from json
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<List<Zikr>> GetZikrsDataService(bool forceRefresh = false)
        {
            List<Zikr> zikrs = null;

            try
            {
                zikrs = await _database.GetZikrsAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return zikrs;
        }
    }
}
