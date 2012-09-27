using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Order.Models
{
    public class Menu
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int DayOfWeek { get; set; }
        [Required]
        public double Price { get; set; }
    }
}