using DataAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAPI.Infrastructure
{
    public class DotnetCurrencyProjectContext : DbContext
    {
        public DotnetCurrencyProjectContext(DbContextOptions<DotnetCurrencyProjectContext> options) : base(options)
        {
        }

        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
