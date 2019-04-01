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
    }
}