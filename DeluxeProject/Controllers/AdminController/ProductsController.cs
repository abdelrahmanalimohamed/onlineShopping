using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeluxeProject.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Products()
        {
            return View();
        }


        public ActionResult Brands()
        {
            return View();
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