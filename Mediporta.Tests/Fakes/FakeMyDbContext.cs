using Mediporta.Database;
using Microsoft.EntityFrameworkCore;
namespace Mediporta.Tests.Fakes
{
    public class FakeMyDbContext : MyDbContext
    {
        public FakeMyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
    }
}