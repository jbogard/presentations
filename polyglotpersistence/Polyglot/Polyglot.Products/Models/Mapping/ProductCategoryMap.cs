using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Polyglot.Products.Models.Mapping
{
    public class ProductCategoryMap : EntityTypeConfiguration<ProductCategory>
    {
        public ProductCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductCategoryID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProductCategory", "Production");
            this.Property(t => t.ProductCategoryID).HasColumnName("ProductCategoryID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.rowguid).HasColumnName("rowguid");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
        }
    }
}
