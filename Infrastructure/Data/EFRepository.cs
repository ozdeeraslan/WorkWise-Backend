using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly WorkWiseContext _db;

        public EFRepository(WorkWiseContext db)
        {
            _db = db;
        }

        public async Task<T> AddAsync(T entity)
        {
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public Task<T> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        Task<T> IRepository<T>.AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        Task IRepository<T>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<T>> IRepository<T>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<T> IRepository<T>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<T> IRepository<T>.UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
