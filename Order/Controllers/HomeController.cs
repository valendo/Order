using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Order.Models;

namespace Order.Controllers
{
    public class HomeController : Controller
    {
        private OrderContext context = new OrderContext();

        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult Menus()
        {
            int currentDay = (int)DateTime.Now.DayOfWeek;
            var list = context.Menus.Where(t => t.DayOfWeek == currentDay).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectedMenus()
        {
            DateTime today = DateTime.Now;
            var list = (from o in context.OrderDetails
                        join m in context.Menus on o.MenuID equals m.ID
                        where o.OrderDate.Day == today.Day && o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year
                        group m by m.ID into g
                        select new { MenuID = g.Key, Name = g.Select(x => x.Name).Distinct(), Price = g.Select(x => x.Price).Distinct(), Count = g.Select(x => x.ID).Count() }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Order(string MenuID, string FullName)
        {
            OrderDetail od = new OrderDetail();
            od.MenuID = int.Parse(MenuID);
            od.OrderDate = DateTime.Now;
            od.FullName = FullName;
            context.OrderDetails.Add(od);
            context.SaveChanges();

            DateTime today = DateTime.Now;
            var list = (from o in context.OrderDetails
                        join m in context.Menus on o.MenuID equals m.ID
                        where o.OrderDate.Day == today.Day && o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year
                        group m by m.ID into g
                        select new { MenuID = g.Key, Name = g.Select(x => x.Name).Distinct(), Price = g.Select(x => x.Price).Distinct(), Count = g.Select(x => x.ID).Count() }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OrderDetail()
        {
            DateTime today = DateTime.Now;
            var list = (from o in context.OrderDetails
                        join m in context.Menus on o.MenuID equals m.ID
                        where o.OrderDate.Day == today.Day && o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year
                        select new {o.ID, m.Name, o.FullName, m.Price }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Remove(string MenuID)
        //{
        //    DateTime today = DateTime.Now;

        //    int menuId = int.Parse(MenuID);
        //    var list_delete = context.OrderDetails.Where(t => t.MenuID == menuId && t.OrderDate.Day == today.Day && t.OrderDate.Month == today.Month && t.OrderDate.Year == today.Year).ToList();
        //    foreach (OrderDetail item in list_delete)
        //    {
        //        context.OrderDetails.Remove(item);
        //    }
        //    context.SaveChanges();
            
        //    var list = (from o in context.OrderDetails
        //                join m in context.Menus on o.MenuID equals m.ID
        //                where o.OrderDate.Day == today.Day && o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year
        //                group m by m.ID into g
        //                select new { MenuID = g.Key, Name = g.Select(x => x.Name).Distinct(), Price = g.Select(x => x.Price).Distinct(), Count = g.Select(x => x.ID).Count() }).ToList();

        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult Remove(int ID)
        {
            DateTime today = DateTime.Now;
            var list_delete = context.OrderDetails.Where(t => t.ID == ID).ToList();
            foreach (OrderDetail item in list_delete)
            {
                context.OrderDetails.Remove(item);
            }
            context.SaveChanges();

            var list = (from o in context.OrderDetails
                        join m in context.Menus on o.MenuID equals m.ID
                        where o.OrderDate.Day == today.Day && o.OrderDate.Month == today.Month && o.OrderDate.Year == today.Year
                        group m by m.ID into g
                        select new { MenuID = g.Key, Name = g.Select(x => x.Name).Distinct(), Price = g.Select(x => x.Price).Distinct(), Count = g.Select(x => x.ID).Count() }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            return View();
        }

        //public ActionResult Select()
        //{
        //    return View();
        //}
    }
}
