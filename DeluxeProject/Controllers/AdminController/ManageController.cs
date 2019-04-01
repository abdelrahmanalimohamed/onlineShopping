using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeluxeModel;

namespace DeluxeProject.Controllers.AdminController
{
    public class ManageController : Controller
    {
        DeluxeShoppingEntities db = new DeluxeShoppingEntities();
      
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        public ActionResult FindItem(int id)
        {
            return PartialView("_ProductModel", db.Brands.Find(id));
        }
    
        
        [HttpDelete]
        public ActionResult DeleteProduct(int id)
        {
            var deleteproduct = (from a in db.products
                                 where a.ID == id
                                 select a).FirstOrDefault();

            db.products.Remove(deleteproduct);
            db.SaveChanges();
            return Json("");
        }



      
        public ActionResult showProduct(int id)
        {
            var product = (from a in db.products
                           where a.ID == id
                           select a).ToList();
            ViewBag.productshow = product;
            return Json(product , JsonRequestBehavior.AllowGet);
        }
    }
}