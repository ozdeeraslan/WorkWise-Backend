using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CompanyRepository : IRepository<Company>
    {
        private readonly WorkWiseContext _db;

        public CompanyRepository(WorkWiseContext db)
        {
            _db=db;
        }

        public async Task<Company> AddAsync(Company entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Company>> GetAllAsync()
        {
            var companies = await _db.Companies.ToListAsync();

            return companies;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            var company = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id);

            return company;
        }

        public Task<Company> UpdateAsync(Company entity)
        {
            throw new NotImplementedException();
        }
    }
}
