using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IManageProductServie
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addQuantity);
        Task AddViewcount(int productId);
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);
        Task<int> Delete(int productId);
        Task<int> AddImages(int productId, List<IFormFile> files);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImages(int imageId, string caption, bool isDefaul);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }
}
