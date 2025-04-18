using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
   public class Recipe : BaseEntity
   {
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } 
        public decimal Discount { get; set; } 
        public Category Category { get; set; } 
    
    }



}
