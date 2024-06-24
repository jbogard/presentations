using After.Model;
using Microsoft.EntityFrameworkCore;

namespace After.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OfferType>()
                .Property(e => e.ExpirationType)
                .HasConversion(
                    e => e.Value,
                    v => ExpirationType.FromValue(v)
                );
        }
    }
}