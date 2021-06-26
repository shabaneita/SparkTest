using Microsoft.AspNetCore.Http;
using SparkTask.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SparkTask.DAL.ViewModels
{
    public class CategoriesEditViewModel
    {
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }
        public string Content { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
