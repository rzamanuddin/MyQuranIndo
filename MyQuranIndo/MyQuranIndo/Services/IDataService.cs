using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyQuranIndo.Services
{
    public interface IDataService<T>
    {
        //Task<bool> AddAsync(T item);
        //Task<bool> UpdateAsync(T item);
        //Task<bool> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<Dictionary<int, T>> GetAsync(bool forceRefresh = false);
    }
}
