using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Order.Models;
using System.IO;
using CsvHelper;
using Order.Business;

namespace Order.Controllers
{
    public class CsvController : Controller
    {
        private OrderContext context = new OrderContext();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                file.SaveAs(path);
                //Stream reader will read test.csv file in current folder
                StreamReader sr = new StreamReader(path);
                //Csv reader reads the stream
                CsvReader csvread = new CsvReader(sr);

                //csvread will fetch all record in one go to the IEnumerable object record
                IEnumerable<MenuItem> records = csvread.GetRecords<MenuItem>();
                context.Database.ExecuteSqlCommand("delete from Menus");
                context.SaveChanges();
                foreach (MenuItem item in records)
                {
                    Menu m = new Menu();
                    m.Name = item.Name;
                    m.DayOfWeek = item.DayOfWeek;
                    m.Price = item.Price;
                    context.Menus.Add(m);
                    context.SaveChanges();
                }
                sr.Close();
                ViewBag.ImportSuccess = "Import CSV file successfully!";
            }
            
            return View();
        }

        public FileContentResult Export()
        {
            var menuList = new List<MenuItem>();
            foreach (var item in context.Menus.ToList())
            {
                MenuItem m = new MenuItem();
                m.Name = item.Name;
                m.DayOfWeek = item.DayOfWeek;
                m.Price = item.Price;
                menuList.Add(m);
            }
            string result = Utility.GetCsv(menuList);
            return File(new System.Text.UTF8Encoding().GetBytes(result), "text/csv", "Menu.csv");

        }
    }
}
