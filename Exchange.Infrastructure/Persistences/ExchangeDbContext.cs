using Exchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Infrastructure.Persistences
{
    public class ExchangeDbContext : DbContext
    {
        public DbSet<ConversionRecord> ConversionRecords { get; set; }
        public DbSet<Client> Clients { get; set; }

        public ExchangeDbContext(DbContextOptions<ExchangeDbContext> options) : base(options)
        {
        }
    }
}