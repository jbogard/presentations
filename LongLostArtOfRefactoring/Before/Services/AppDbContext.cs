using Before.Model;
using Microsoft.EntityFrameworkCore;

namespace Before.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
    }
}