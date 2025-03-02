using CoinDesk.Models;
using Microsoft.EntityFrameworkCore;

namespace CoinDesk.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Currency> Currencies { get; set; }
    }
}