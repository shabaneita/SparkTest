using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkTask.Data;
using SparkTask.Models;

namespace SparkTask.Controllers.Apis
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Select(x=>new Product() { 
            
                Name=x.Name,
                Price=x.Price,
                Image=x.Image,
                Category=x.Category,
                CategoryId=x.CategoryId,
                Description=x.Description,
                ProductId=x.ProductId
            }).ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(y => y.Category).FirstOrDefaultAsync(q => q.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
     
    }
}
