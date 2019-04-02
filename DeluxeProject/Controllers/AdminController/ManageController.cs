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
      
        public ActionResult ManageProducts()
        {
           
            return View(db.products.ToList());
        }

        public ActionResult FindItem(int id)
        {
            return PartialView("_Details", db.products.Find(id));
        }
    
        
 
        public ActionResult DeleteProduct(int id)
        {
            var deleteproduct = (from a in db.products
                                 where a.ID == id
                                 select a).FirstOrDefault();

            db.products.Remove(deleteproduct);
            db.SaveChanges();
            return Json("Deletion is Done",JsonRequestBehavior.AllowGet);
        }



     
    }
}