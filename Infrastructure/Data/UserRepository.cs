using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository<AppUser>
    {
        private readonly WorkWiseContext _db;

        public UserRepository(WorkWiseContext db)
        {
            _db = db;
        }

        public async Task<AppUser> AddAsync(AppUser entity)
        {
            _db.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetByEmail(string email)
        {
            var mail = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

            return mail;
        }

        public Task<AppUser> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> UpdateAsync(AppUser entity)
        {
            throw new NotImplementedException();
        }

        Task IUserRepository<AppUser>.GetByEmail(string email)
        {
            return _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}