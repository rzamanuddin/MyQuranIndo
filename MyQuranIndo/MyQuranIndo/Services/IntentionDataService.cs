using MyQuranIndo.Databases;
using MyQuranIndo.Models.Zikrs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IIntentionDataService
    {
        Task<IEnumerable<Intention>> GetIntentions(bool forceRefresh = false);
    }
    public class IntentionDataService : IIntentionDataService
    {
        private Database _database;

        public IntentionDataService()
        {
            _database = new Database();
        }

        public async Task<IEnumerable<Intention>> GetIntentions(bool forceRefresh = false)
        {
            IEnumerable<Intention> results;
            try
            {
                results = await _database.GetIntentionsAsync();
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}
