using MyQuranIndo.Databases;
using MyQuranIndo.Models;
using MyQuranIndo.Models.Qurans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MyQuranIndo.Services
{

    public interface ISurahDataService //: IDataService<Surah>
    {
        //Task<IEnumerable<Surah>> GetAsync(CategoryFilter categoryFilter, bool forceRefresh = false);
        //Task<Dictionary<int, Surah>> GetAsync(bool forceRefresh = false);
        Task<List<Surah>> GetSurahNewAsync(bool forceRefresh = false);
        Task<Surah> GetSurahAsync(int id);
    }
    public class SurahDataService : ISurahDataService
    {
        private DatabaseManager _database;

        public SurahDataService()
        {
            _database = new DatabaseManager();
        }
               
        //public async Task<Surah> GetAsync(int id)
        //{
        //    Surah surah = null;

        //    try
        //    {
        //        //HttpResponseMessage response = await _database.GetAsync(AppSetting.GetAPICategories() + "/" + id.ToString());
        //        surah = await _database.GetSurahAsync(id);
        //        //if (response.IsSuccessStatusCode)
        //        //{
        //        //    string content = await response.Content.ReadAsStringAsync();
        //        //    category = JsonConvert.DeserializeObject<Category>(content);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return surah;
        //}

        //public async Task<Dictionary<int, Surah>> GetAsync(bool forceRefresh = false)
        //{
        //    Dictionary<int, Surah> surah = null;

        //    try
        //    {
        //        surah = await _database.GetSurahAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return surah;
        //}

        public async Task<List<Surah>> GetSurahNewAsync(bool forceRefresh)
        {
            List<Surah> surahs = null;

            try
            {
                surahs = await _database.GetSurahAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return surahs;
        }

        public async Task<Surah> GetSurahAsync(int id)
        {
            Surah surah = null;

            try
            {
                var surahs = await _database.GetSurahAsync();
                surah = surahs.FirstOrDefault(q => q.ID == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return surah;
        }

        //public async Task<IEnumerable<Category>> GetAsync(CategoryFilter categoryFilter, bool forceRefresh = false)
        //{
        //    IEnumerable<Category> categories = await GetAsync(true);

        //    if (!string.IsNullOrWhiteSpace(categoryFilter.Name))
        //    {
        //        categories = categories.Where(q => q.Name.ToLower().Contains(categoryFilter.Name.ToLower()));
        //    }

        //    return categories;
        //}

    }

}
