using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeluxeProject.Controllers.UserController
{
    public class UserActionsController : Controller
    {
       public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }
        
        public ActionResult Product()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            return View();
        }
    }
}