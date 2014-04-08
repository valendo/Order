using Order.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.Models
{
    public class MenuItem
    {
        [CsvField(Name = "Name")]
        public string Name { get; set; }
        [CsvField(Name = "DayOfWeek")]
        public int DayOfWeek { get; set; }
        [CsvField(Name = "Price")]
        public double Price { get; set; }
    }
}