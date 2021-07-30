using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Models
{
    public class ProductDetailViewModel
    {
        public CategoryVm Category { get; set; }
        public ProductVm Product { get; set; }
        public List<ProductVm> RelateProducts { get; set; }
        public List<ProductImageViewModel> ProductImage { get; set; }
    }
}
