using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SparkTask.DAL.ViewModels;
using SparkTask.Data;
using SparkTask.Models;

namespace SparkTask.Controllers
{
    [Authorize]

    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostingEnvironment _hostingEnvironment;
        public CategoriesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //await _context.Categories.Include(y => y.Products).FirstOrDefaultAsync(q => q.CategoryId == id);
            var category = await _context.Categories.Include(y => y.Products).FirstOrDefaultAsync(q => q.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriesEditViewModel model  )
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Category category = new Category
                {
                    Name = model.Name,
                    Content = model.Content,
                    Image = uniqueFileName,
                    Products=model.Products,
                };
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        private string UploadedFile(CategoriesEditViewModel model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            else
            {
                uniqueFileName = "100c4b49-f8ab-4272-988e-1739500fc52e_No-Photo-Available.jpg";
            }
            return uniqueFileName;
        }
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = await _context.Categories.Where(x => x.CategoryId == id).FirstOrDefaultAsync();

            CategoriesEditViewModel viewModel = new CategoriesEditViewModel
            {

                Name = category.Name,
                Content = category.Content,
                Products = category.Products,
                CategoryId = category.CategoryId,

            };
            //var category = await _context.Categories.FindAsync(id);
            //if (category == null)
            //{
            //    return NotFound();
            //}
            return View(viewModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriesEditViewModel model ,IFormFile file )
        {
            

            if (ModelState.IsValid)
            {

                if (id != model.CategoryId)
                {
                    return NotFound();
                }
                Category category = await _context.Categories.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
                category.Name = model.Name;
                category.Content = model.Content;
                if (model.Image != null)
                {
                    if (model.Image != null)
                    {
                        string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "images", model.Image.ToString());
                        System.IO.File.Delete(filepath);
                    }
                    category.Image = UploadedFile(model);
                }
                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetCat()
        {
            return View(await _context.Categories.ToListAsync());
            //return View(_context.Categories.ToListAsync());
        }
    }
}
