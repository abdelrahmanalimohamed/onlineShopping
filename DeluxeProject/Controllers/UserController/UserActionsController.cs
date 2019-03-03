using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ASPSnippets.FaceBookAPI;
using DeluxeModel;

namespace DeluxeProject.Controllers.UserController
{
    public class UserActionsController : Controller
    {

       public ActionResult SignUp()
        {
            FaceBookConnect.API_Key = "824415404557890";
            FaceBookConnect.API_Secret = "8f48f5360b2600fb2744af16d903c28e";
            user user = new user();
           
            if (Request.QueryString["code"] == "access_denied")
            {
                ViewBag.Message = "user has denied access";
            }
            else
            {
                string code = Request.QueryString["code"];
                if (!string.IsNullOrEmpty(code))
                {
                    string data = FaceBookConnect.Fetch(code, "me?fields=name,email");
                    user = new JavaScriptSerializer().Deserialize<user>(data);
                   
                }
            }
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