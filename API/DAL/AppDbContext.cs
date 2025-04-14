using API.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Collections.Generic;

namespace API.DAL
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// DbSet for User entity. Needed for EF Core to manage User data.
        /// </summary>
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? connStr = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { RoleId = 1, RoleName = "Admin" },
                new UserRole { RoleId = 2, RoleName = "User" }
            );

            base.OnModelCreating(modelBuilder);
        }

    }


}
