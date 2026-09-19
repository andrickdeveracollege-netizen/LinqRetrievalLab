using LinqRetrievalLab.Models.Data;
using LinqRetrievalLab.Models.Domain;
using LinqRetrievalLab.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqRetrievalLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CatalogController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Catalog/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                return await _context.Category
                    .Include(c => c.Product)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // GET: api/Catalog/categories/5
        [HttpGet("categories/{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                var category = await _context.Category
                    .Include(c => c.Product)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound();
                }

                return category;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // POST: api/Catalog/categories
        [HttpPost("categories")]
        public async Task<ActionResult<Category>> PostCategory(CategoryDTO dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest();
                }

                var category = new Category
                {
                    Name = dto.Name
                };

                _context.Category.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetCategory),
                    new { id = category.Id },
                    category);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT: api/Catalog/categories/5
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> PutCategory(
            int id,
            CategoryDTO dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest();
                }

                var category = await _context.Category.FindAsync(id);

                if (category == null)
                {
                    return NotFound();
                }

                category.Name = dto.Name;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }

                throw;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/Catalog/categories/5
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Category
                    .Include(c => c.Product)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound();
                }

                if (category.Product.Any())
                {
                    return BadRequest(
                        "Cannot delete category because it has products.");
                }

                _context.Category.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }

        // GET: api/Catalog/products
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                return await _context.Product
                    .Include(p => p.Category)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // GET: api/Catalog/products/5
        [HttpGet("products/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Product
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                return product;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // POST: api/Catalog/products
        [HttpPost("products")]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest();
                }

                var category = await _context.Category
                    .FindAsync(dto.CategoryId);

                if (category == null)
                {
                    return BadRequest("Category does not exist.");
                }

                var product = new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Stock = dto.Stock,
                    CategoryId = dto.CategoryId
                };

                _context.Product.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetProduct),
                    new { id = product.Id },
                    product);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT: api/Catalog/products/5
        [HttpPut("products/{id}")]
        public async Task<IActionResult> PutProduct(
            int id,
            ProductDTO dto)
        {
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                {
                    return BadRequest();
                }

                var product = await _context.Product.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                var category = await _context.Category
                    .FindAsync(dto.CategoryId);

                if (category == null)
                {
                    return BadRequest("Category does not exist.");
                }

                product.Name = dto.Name;
                product.Price = dto.Price;
                product.Stock = dto.Stock;
                product.CategoryId = dto.CategoryId;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }

                throw;
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/Catalog/products/5
        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Product.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // 3. Search products by name
        // GET: api/Catalog/products/Search?q=mouse
        [HttpGet("products/Search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(
            [FromQuery] string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    return BadRequest();
                }

                var pattern = $"%{q.Trim()}%";

                return await _context.Product
                    .Where(p => EF.Functions.Like(p.Name, pattern))
                    .Include(p => p.Category)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // 4. Products within price range
        // GET: api/Catalog/products/PriceRange?min=100&max=500
        [HttpGet("products/PriceRange")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRange(
            [FromQuery] decimal min,
            [FromQuery] decimal max)
        {
            try
            {
                if (min > max)
                {
                    return BadRequest();
                }

                return await _context.Product
                    .Where(p => p.Price >= min && p.Price <= max)
                    .Include(p => p.Category)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // 5. Sum of stock
        // GET: api/Catalog/products/StockSum
        [HttpGet("products/StockSum")]
        public async Task<ActionResult<int>> GetStockSum()
        {
            try
            {
                var sum = await _context.Product
                    .Select(p => p.Stock)
                    .DefaultIfEmpty(0)
                    .SumAsync();

                return Ok(sum);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // 6. Average price
        // GET: api/Catalog/products/AveragePrice
        [HttpGet("products/AveragePrice")]
        public async Task<ActionResult<decimal>> GetAveragePrice()
        {
            try
            {
                var average = await _context.Product
                    .Select(p => (decimal?)p.Price)
                    .AverageAsync();

                return Ok(average ?? 0);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // 7. Total number of products
        // GET: api/Catalog/products/Count
        [HttpGet("products/Count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.Product.CountAsync();

                return Ok(count);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}