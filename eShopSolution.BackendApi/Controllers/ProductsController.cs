using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServie _productService;
        public ProductsController(IProductServie productService)
        {
            _productService = productService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _productService.GetAllPaging(request);
            return Ok(products);
        }

        //http:/localhost:port/product/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productService.GetById(productId, languageId);
            if (product == null)
            {
                return BadRequest("Cannot find productId");
            }
            return Ok(product);
        }

        [HttpPost]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.Create(request);
            //if (result == 0)
            //{
            //    return BadRequest();
            //}
            //var product = await _productService.GetById(productId, request.LanguageId);
            //var a  = CreatedAtAction(nameof(GetById), new { id = productId }, product);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var affetedResult = await _productService.Update(request);
        if (affetedResult == 0)
        {
            return BadRequest();
        }
        return Ok();
    }
    [HttpPatch("{productId}/{newPrice}")]
    public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
    {
        var isSuccessfull = await _productService.UpdatePrice(productId, newPrice);
        if (isSuccessfull)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> Delete(int productId)
    {
        var affetedResult = await _productService.Delete(productId);
        if (affetedResult == 0)
        {
            return BadRequest();
        }
        return Ok();
    }



    [HttpGet("{productId}/images/{imageId}")]
    public async Task<IActionResult> GetImageById(int productId, int imageId)
    {
        var image = await _productService.GetImageById(imageId);
        if (image == null)
        {
            return BadRequest("Cannot find productId");
        }
        return Ok(image);
    }

    [HttpPost("{productId}/images")]
    public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var imageId = await _productService.AddImage(productId, request);
        if (imageId == 0)
        {
            return BadRequest();
        }
        var image = await _productService.GetImageById(imageId);
        return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
    }

    [HttpPut("{productId}/images/{imageId}")]
    public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _productService.UpdateImage(imageId, request);
        if (result == 0)
        {
            return BadRequest();
        }
        return Ok();
    }
    [HttpDelete("{productId}/images/{imageId}")]
    public async Task<IActionResult> RemoveImage(int imageId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _productService.RemoveImage(imageId);
        if (result == 0)
        {
            return BadRequest();
        }
        return Ok();
    }
        [HttpPut("{id}/categories")]
        public async Task<IActionResult> RoleAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.CategoryAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
