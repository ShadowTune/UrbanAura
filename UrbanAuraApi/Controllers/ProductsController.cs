using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UrbanAuraApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product> productRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            IReadOnlyList<Product> products = await productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Product? product = await productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            productRepository.Create(product);
            if (await productRepository.SaveChangesAsync())
            {
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
            {
                return BadRequest("Invalid Request");
            }
            productRepository.Update(product);
            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem updating product");
        }
        private bool ProductExists(int id)
        {
            return productRepository.Exists(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            Product? product = await productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return BadRequest("Product not found");
            }
            productRepository.Delete(product);
            if (await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
        {
            // IReadOnlyList<string> brands = await productRepository.GetProductBrandsAsync();
            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
        {
            // IReadOnlyList<string> types = await productRepository.GetProductTypesAsync();
            return Ok();
        }
    } 
}
