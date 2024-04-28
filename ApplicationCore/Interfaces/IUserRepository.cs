using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);

        Task<List<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(string id);

        Task GetByEmail(string email);
    }
}
