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
            TempData["itemid"] = id;
            return PartialView("_Details", db.products.Find(id));
        }

        public ActionResult Update(product product)
        {
          
            if (product.ID > 0)
            {
                var productdata = (from a in db.products
                                   where a.ID == product.ID
                                   select a).FirstOrDefault();

                if(productdata != null)
                {
                    productdata.prd_name = product.prd_name;
                    productdata.prd_desc = product.prd_desc;
                    productdata.prd_quantity = product.prd_quantity;
                    productdata.price = product.price;
                }
              
             //   var update = db.Entry(product).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json("Update Sucess", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Update Failed", JsonRequestBehavior.AllowGet);
            }
        
        }




        [HttpPost]
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