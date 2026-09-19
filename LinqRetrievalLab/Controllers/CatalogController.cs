using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinqRetrievalLab.Data;
using LinqRetrievalLab.Models.Domain;
using LinqRetrievalLab.Models.Data;

namespace LinqRetrievalLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly LinqRetrievalLabContext _context;

        public CatalogController(LinqRetrievalLabContext context)
        {
            _context = context;
        }

        // GET: api/Catalog/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Category
                                 .Include(c => c.Products)
                                 .ToListAsync();
        }

        // GET: api/Catalog/categories/5
        [HttpGet("categories/{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Category
                                         .Include(c => c.Products)
                                         .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();
            return category;
        }

        // POST: api/Catalog/categories
        [HttpPost("categories")]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // PUT: api/Catalog/categories/5
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id) return BadRequest();

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Catalog/categories/5
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

            private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }


        // GET: api/Catalog/products
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Product
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }

        // GET: api/Catalog/products/5
        [HttpGet("products/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product
                                        .Include(p => p.Category)
                                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            return product;
        }

        // GET: api/Catalog/products/ByCategory/3
        [HttpGet("products/ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            return await _context.Product
                                 .Where(p => p.CategoryId == categoryId)
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }

        // GET: api/Catalog/products/Search?q=term
        [HttpGet("products/Search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return BadRequest("q is required");

            var pattern = $"%{q.Trim()}%";
            return await _context.Product
                                 .Where(p => EF.Functions.Like(p.Name, pattern))
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }

        // GET: api/Catalog/products/PriceRange?min=10&max=100
        [HttpGet("products/PriceRange")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPriceRange([FromQuery] decimal min, [FromQuery] decimal max)
        {
            return await _context.Product
                                 .Where(p => p.Price >= min && p.Price <= max)
                                 .Include(p => p.Category)
                                 .ToListAsync();
        }

        // GET: api/Catalog/products/StockSum
        [HttpGet("products/StockSum")]
        public async Task<ActionResult<int>> GetStockSum()
        {
            var any = await _context.Product.AnyAsync();
            if (!any) return Ok(0);
            var sum = await _context.Product.SumAsync(p => p.Stock);
            return Ok(sum);
        }

        // GET: api/Catalog/products/AveragePrice
        [HttpGet("products/AveragePrice")]
        public async Task<ActionResult<decimal>> GetAveragePrice()
        {
            var any = await _context.Product.AnyAsync();
            if (!any) return Ok(0m);
            var avg = await _context.Product.AverageAsync(p => p.Price);
            return Ok(decimal.Round(avg, 2));
        }

        // GET: api/Catalog/products/Count
        [HttpGet("products/Count")]
        public async Task<ActionResult<int>> GetCount()
        {
            var count = await _context.Product.CountAsync();
            return Ok(count);
        }

        // POST: api/Catalog/products
        [HttpPost("products")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/Catalog/products/5
        [HttpPut("products/{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Catalog/products/5
        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return NotFound();

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
