using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class WorkWiseContext : IdentityDbContext<AppUser>
    {
		public WorkWiseContext(DbContextOptions<WorkWiseContext> options) : base(options)
		{

		}

		public DbSet<Company> Companies { get; set; }

        public DbSet<Leave> Leaves { get; set; }

		public DbSet<Expense> Expenses { get; set; }

		public DbSet<AdvancePayment> AdvancePayments { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<AppUser>().OwnsOne(x => x.PersonalDetail).Property(x => x.Wage).HasPrecision(18, 2); ;

			builder.Entity<AdvancePayment>().Property(x => x.Amount).HasPrecision(18, 2);
			builder.Entity<Expense>().Property(x => x.Amount).HasPrecision(18, 2);

			
        }

	}
}
