using Mediporta.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mediporta.Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
    }
}
