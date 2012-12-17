using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Polyglot.Products.Models.Mapping
{
    public class ProductProductPhotoMap : EntityTypeConfiguration<ProductProductPhoto>
    {
        public ProductProductPhotoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProductID, t.ProductPhotoID });

            // Properties
            this.Property(t => t.ProductID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductPhotoID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProductProductPhoto", "Production");
            this.Property(t => t.ProductID).HasColumnName("ProductID");
            this.Property(t => t.ProductPhotoID).HasColumnName("ProductPhotoID");
            this.Property(t => t.Primary).HasColumnName("Primary");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.ProductProductPhotoes)
                .HasForeignKey(d => d.ProductID);
            this.HasRequired(t => t.ProductPhoto)
                .WithMany(t => t.ProductProductPhotoes)
                .HasForeignKey(d => d.ProductPhotoID);

        }
    }
}
