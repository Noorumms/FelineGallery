using Feline_Gallery_v1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Feline_Gallery_v1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ❌ MAKE SURE YOU DON'T HAVE ANY DbSet<> PROPERTIES HERE!
        // NO: public DbSet<Artwork> Artworks { get; set; }
        // NO: public DbSet<Category> Categories { get; set; }
        // etc...

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure ApplicationUser custom properties
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // ⭐ CRITICAL: Ignore ALL Dapper models
            builder.Ignore<Artwork>();
            builder.Ignore<Category>();
            builder.Ignore<Exhibition>();
            builder.Ignore<ExhibitionArtwork>();
            builder.Ignore<Artist>();
            builder.Ignore<Cart>();
            builder.Ignore<CartItem>();
            builder.Ignore<Order>();
            builder.Ignore<OrderItem>();
        }
    }
}