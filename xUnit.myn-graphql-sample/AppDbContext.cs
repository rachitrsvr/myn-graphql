using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using myn_graphql_sample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xUnit.myn_graphql_sample
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


        // Saves changes made to the database.
        public override int SaveChanges()
        {
            try
            {
                ChangeTracker.DetectChanges();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
