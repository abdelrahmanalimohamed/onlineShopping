using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeluxeModel;

namespace DeluxeProject.Controllers
{
    public class ProductsController : Controller
    {
        DeluxeShoppingEntities db = new DeluxeShoppingEntities();
        
        public ActionResult Products()
        {
            return View();
        }

       

        public ActionResult Brands()
        {
            return View();
        }

        
        public ActionResult GetProducts()
        {
            return View(db.products.ToList());
        }

        public ActionResult Category()
        {
            return View();
        }


        public ActionResult Supplier()
        {
            return View();
        }
    
    }
}