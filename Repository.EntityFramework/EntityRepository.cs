using System;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repository.EntityFramework.Config;

namespace Repository.EntityFramework
{
    public class EntityRepository : DbContext
    {
        private string connectionString = string.Empty;
        private bool useLazyLoading;
        public EntityRepository(string connectionString = "YourDeafaultConnectionString", bool useLazyLoading = true, bool ensureDeleted = false, bool ensureCreated = true, bool migrate = false)
        {
            this.connectionString = connectionString;
            this.useLazyLoading = useLazyLoading;

            if (ensureDeleted == true)
                Database.EnsureDeleted();
            
            if (ensureCreated == true)
                Database.EnsureCreated();
            
            if (migrate == true)
                Database.Migrate();
        }

        // Underneath create as many DbSet' as you have domain classes you wish to persist.
        // Each DbSet should have a Config file aplied in the method 'OnModelCreating'

        // Example:
        // public virtual DbSet<YourDomainClass> YourDomainClassInPlural { get; set; }


     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // To Lazy Load properties they require the keywork Virtual.
            // Making use of Lazy load means that the property only loads as it is about to be used,
            // which improves performance of the program
            optionsBuilder.UseLazyLoadingProxies(useLazyLoading).UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create a new class under Config, with the name of the domain class you wish to persist,
                // ending it in Config, to differnetiate it from the actual class
            
            // Example:
            // modelBuilder.ApplyConfiguration(new YourDomainClassConfig());
        }
    }
}

