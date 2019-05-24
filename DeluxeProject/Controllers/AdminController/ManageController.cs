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


        public ActionResult ManageUsers()
        {
            return View(db.users.ToList());
        }

        public ActionResult ManageBrands()
        {
            return View(db.Brands.ToList());
        }

        
        public ActionResult ManageCategory()
        {
            return View(db.Categories.ToList());
        }


        public ActionResult ManageSuppliers()
        {
            return View(db.suppliers.ToList());
        }
 



     
    }
}