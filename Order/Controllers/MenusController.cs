using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Order.Models;

namespace Order.Controllers
{   
    public class MenusController : Controller
    {
        private OrderContext context = new OrderContext();

        //
        // GET: /Menus/

        public ViewResult Index()
        {
            return View(context.Menus.ToList());
        }

        //
        // GET: /Menus/Details/5

        public ViewResult Details(int id)
        {
            Menu menu = context.Menus.Single(x => x.ID == id);
            return View(menu);
        }

        //
        // GET: /Menus/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Menus/Create

        [HttpPost]
        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                context.Menus.Add(menu);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(menu);
        }
        
        //
        // GET: /Menus/Edit/5
 
        public ActionResult Edit(int id)
        {
            Menu menu = context.Menus.Single(x => x.ID == id);
            return View(menu);
        }

        //
        // POST: /Menus/Edit/5

        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                context.Entry(menu).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        //
        // GET: /Menus/Delete/5
 
        public ActionResult Delete(int id)
        {
            Menu menu = context.Menus.Single(x => x.ID == id);
            return View(menu);
        }

        //
        // POST: /Menus/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = context.Menus.Single(x => x.ID == id);
            context.Menus.Remove(menu);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}