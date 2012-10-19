using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order.Models
{
    public class OrderDetail
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public DateTime OrderDate { get; set; }
        public string FullName { get; set; }
    }
}