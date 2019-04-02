using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeluxeModel;

namespace DeluxeProject.Controllers
{
    public class DemoController : Controller
    {
        DeluxeShoppingEntities db = new DeluxeShoppingEntities();

        public ActionResult Demo()
        {
            return View(db.products.ToList());
        }

        public ActionResult Details(int id)
        {
            return PartialView("_Details", db.products.Find(id));
        }

        [HttpPost]
        public ActionResult Details(product product)
        {
            return null;
        }

        [HttpDelete]
        public ActionResult DeleteProduct(int id)
        {
            var deleteproduct = (from a in db.products
                                 where a.ID == id
                                 select a).FirstOrDefault();

            db.products.Remove(deleteproduct);
            db.SaveChanges();
            return Json("Deletion is Done");
        }
    }
}