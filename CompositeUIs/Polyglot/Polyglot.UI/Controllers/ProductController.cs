using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Polyglot.Products.Models;

namespace Polyglot.UI.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Categories()
        {
            using (var context = new AdventureWorks2012Context())
            {
                var categories = context.ProductCategories.ToList();
                return PartialView(categories);
            }
        }

        public ActionResult Index(int productCategoryId)
        {
            using (var context = new AdventureWorks2012Context())
            {
                var category = context.ProductCategories
                    .Include("ProductSubcategories")
                    .Single(c => c.ProductCategoryID == productCategoryId);

                return View(category);
            }
        }

        public ActionResult List(int productSubcategoryId)
        {
            using (var context = new AdventureWorks2012Context())
            {
                var subcategory = context.ProductSubcategories
                    .Include("Products")
                    .Single(c => c.ProductSubcategoryID == productSubcategoryId);

                return View(subcategory);
            }
        }

        public ActionResult Show(int productId)
        {
            using (var context = new AdventureWorks2012Context())
            {
                var product = context.Products
                    .Include("ProductModel.ProductModelProductDescriptionCultures.ProductDescription")
                    .Single(c => c.ProductID == productId);

                return View(product);
            }
        }

        public ActionResult Widget(int productId)
        {
            using (var context = new AdventureWorks2012Context())
            {
                var product = context.Products
                    .Include("ProductModel.ProductModelProductDescriptionCultures.ProductDescription")
                    .Single(c => c.ProductID == productId);

                return PartialView(product);
            }
        }

    }
}
