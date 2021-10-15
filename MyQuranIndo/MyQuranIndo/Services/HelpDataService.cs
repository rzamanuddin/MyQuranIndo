using MyQuranIndo.Databases;
using MyQuranIndo.Models.Helps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IHelpDataService
    {
        Task<IEnumerable<HelpHeader>> GetAsync(bool forceRefresh = false);
    }

    public class HelpDataService : IHelpDataService
    {
        private Database _database;

        public HelpDataService()
        {
            _database = new Database();
        }
        public async Task<IEnumerable<HelpHeader>> GetAsync(bool forceRefresh = false)
        {
            var helps = new List<HelpHeader>();
            try
            {
                helps =  await  _database.GetHelpAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return helps;
        }
    }
}
