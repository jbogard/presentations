using System;
using System.Collections.Generic;

namespace AdventureWorksCosmos.Products.Models
{
    public partial class Product
    {
        public Product()
        {
            BillOfMaterialsComponent = new HashSet<BillOfMaterials>();
            BillOfMaterialsProductAssembly = new HashSet<BillOfMaterials>();
            ProductCostHistory = new HashSet<ProductCostHistory>();
            ProductInventory = new HashSet<ProductInventory>();
            ProductListPriceHistory = new HashSet<ProductListPriceHistory>();
            ProductProductPhoto = new HashSet<ProductProductPhoto>();
            ProductReview = new HashSet<ProductReview>();
            ProductVendor = new HashSet<ProductVendor>();
            PurchaseOrderDetail = new HashSet<PurchaseOrderDetail>();
            ShoppingCartItem = new HashSet<ShoppingCartItem>();
            SpecialOfferProduct = new HashSet<SpecialOfferProduct>();
            TransactionHistory = new HashSet<TransactionHistory>();
            WorkOrder = new HashSet<WorkOrder>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public bool? MakeFlag { get; set; }
        public bool? FinishedGoodsFlag { get; set; }
        public string Color { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Size { get; set; }
        public string SizeUnitMeasureCode { get; set; }
        public string WeightUnitMeasureCode { get; set; }
        public decimal? Weight { get; set; }
        public int DaysToManufacture { get; set; }
        public string ProductLine { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public int? ProductSubcategoryId { get; set; }
        public int? ProductModelId { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ProductModel ProductModel { get; set; }
        public ProductSubcategory ProductSubcategory { get; set; }
        public UnitMeasure SizeUnitMeasureCodeNavigation { get; set; }
        public UnitMeasure WeightUnitMeasureCodeNavigation { get; set; }
        public ICollection<BillOfMaterials> BillOfMaterialsComponent { get; set; }
        public ICollection<BillOfMaterials> BillOfMaterialsProductAssembly { get; set; }
        public ICollection<ProductCostHistory> ProductCostHistory { get; set; }
        public ICollection<ProductInventory> ProductInventory { get; set; }
        public ICollection<ProductListPriceHistory> ProductListPriceHistory { get; set; }
        public ICollection<ProductProductPhoto> ProductProductPhoto { get; set; }
        public ICollection<ProductReview> ProductReview { get; set; }
        public ICollection<ProductVendor> ProductVendor { get; set; }
        public ICollection<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItem { get; set; }
        public ICollection<SpecialOfferProduct> SpecialOfferProduct { get; set; }
        public ICollection<TransactionHistory> TransactionHistory { get; set; }
        public ICollection<WorkOrder> WorkOrder { get; set; }
    }
}
