using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Polyglot.Products.Models.Mapping;

namespace Polyglot.Products.Models
{
    public class AdventureWorks2012Context : DbContext
    {
        static AdventureWorks2012Context()
        {
            Database.SetInitializer<AdventureWorks2012Context>(null);
        }

		public AdventureWorks2012Context()
			: base("Name=AdventureWorks2012Context")
		{
		}

        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }
        public DbSet<ProductPhoto> ProductPhotoes { get; set; }
        public DbSet<ProductProductPhoto> ProductProductPhotoes { get; set; }
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; }
        public DbSet<UnitMeasure> UnitMeasures { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new CultureMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductCategoryMap());
            modelBuilder.Configurations.Add(new ProductDescriptionMap());
            modelBuilder.Configurations.Add(new ProductModelMap());
            modelBuilder.Configurations.Add(new ProductModelProductDescriptionCultureMap());
            modelBuilder.Configurations.Add(new ProductPhotoMap());
            modelBuilder.Configurations.Add(new ProductProductPhotoMap());
            modelBuilder.Configurations.Add(new ProductSubcategoryMap());
            modelBuilder.Configurations.Add(new UnitMeasureMap());
        }
    }
}
