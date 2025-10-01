using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<GeneralResponse<IEnumerable<Product>>>> GetAll()
        {
            try
            {
                var products = await _productService.GetAllAsync();

                if (products == null || !products.Any())
                    return NotFound(
                        GeneralResponse<IEnumerable<Product>>.Fail("No products found")
                    );

                return Ok(GeneralResponse<IEnumerable<Product>>.Ok(products));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    GeneralResponse<IEnumerable<Product>>.Fail($"Server error: {ex.Message}")
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralResponse<Product>>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(GeneralResponse<Product>.Fail("Product ID is required"));

            try
            {
                var product = await _productService.GetByIdAsync(id);

                if (product == null)
                    return NotFound(GeneralResponse<Product>.Fail("Product not found"));

                return Ok(GeneralResponse<Product>.Ok(product));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    GeneralResponse<Product>.Fail($"Server error: {ex.Message}")
                );
            }
        }

        [HttpPost]
        public async Task<ActionResult<GeneralResponse<Product>>> Create([FromForm] ProductCreateUpdateDto dto)
        {
            if (dto == null)
                return BadRequest(GeneralResponse<Product>.Fail("Invalid product data"));

            try
            {
                var created = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById),
                    new { id = created.Id },
                    GeneralResponse<Product>.Ok(created, "Product created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    GeneralResponse<Product>.Fail($"Creation failed: {ex.Message}"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GeneralResponse<Product>>> Update(string id,[FromForm] ProductCreateUpdateDto dto)
        {
            if (dto == null)
                return BadRequest(GeneralResponse<Product>.Fail("Invalid product data"));

            try
            {
                var updated = await _productService.UpdateAsync(id, dto);
                if (updated == null)
                    return NotFound(GeneralResponse<Product>.Fail("Product not found"));

                return Ok(GeneralResponse<Product>.Ok(updated, "Product updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    GeneralResponse<Product>.Fail($"Update failed: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GeneralResponse<string>>> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(GeneralResponse<string>.Fail("Product ID is required"));

            try
            {
                var existing = await _productService.GetByIdAsync(id);
                if (existing == null)
                    return NotFound(GeneralResponse<string>.Fail("Product not found"));

                await _productService.DeleteAsync(id);
                return Ok(GeneralResponse<string>.Ok(id, "Product deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    GeneralResponse<string>.Fail($"Deletion failed: {ex.Message}")
                );
            }
        }
    }
}
