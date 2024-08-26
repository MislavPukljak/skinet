using System.Drawing;
using Core;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository productRepository) : ControllerBase
    {
        private readonly IProductRepository _productRepository = productRepository;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
            string? brand,
            string? type,
            string? sort)
        {
            return Ok(await _productRepository.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _productRepository.AddProduct(product);

            if (await _productRepository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new {id = product.Id}, product);
            }

            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
            {
                return BadRequest("Cannot update this product");
            }

            _productRepository.UpdateProduct(product);

            if (await _productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            _productRepository.DeleteProduct(product);

            if (await _productRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting the product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await _productRepository.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await _productRepository.GetTypesAsync());
        }

        private bool ProductExists(int id)
        {
            return _productRepository.ProductExists(id);
        }
    }
}
