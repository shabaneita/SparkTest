using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SparkTask.Models
{
 
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }


    }
}
