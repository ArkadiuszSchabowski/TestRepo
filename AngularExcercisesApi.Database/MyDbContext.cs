using _076.AngularExcercisesAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace _076.AngularExcercisesAPI.Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<FlashCard> FlashCards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "User"
            },
            new Role
            {
                Id = 2,
                Name = "Manager"
            },
            new Role
            {
                Id = 3,
                Name = "Admin"
            });

            modelBuilder.Entity<User>().HasOne(u => u.Role)
                .WithMany(r  => r.Users)
                .HasForeignKey(u => u.RoleId);
        }
    }
}
