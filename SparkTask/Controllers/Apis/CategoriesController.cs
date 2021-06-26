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
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {

            return await _context.Categories.Select(x => new Category() {
            
            CategoryId=x.CategoryId,
            Content=x.Content,
            Image=x.Image,
            Name=x.Name,
            Products=x.Products
            
            }).ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.Include(y => y.Products).FirstOrDefaultAsync(q => q.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        //https://localhost:44301/api/categories/categoryproduct/4
        //getproducts in specific catgory
        //api/SubCategories/categoryproduct/1
        [HttpGet("{id}")]
        [Route("categoryproduct/{id}")]

        public List<Product> getProductOfCategory(int id)
        {
            //id of category
            List<Product> products = new List<Product>();
            //getsubcategory of id
            var category = _context.Categories.FirstOrDefault(q => q.CategoryId == id);
            //all product in subcategory
            products = _context.Products.Where((a => a.CategoryId == category.CategoryId)).ToList();

            return products;
        }

    }
}
