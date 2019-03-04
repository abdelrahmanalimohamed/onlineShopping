using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ASPSnippets.FaceBookAPI;
using DeluxeModel;
using DeluxeProject.Models;

namespace DeluxeProject.Controllers.UserController
{
    public class UserActionsController : Controller
    {
        DeluxeShoppingEntities db = new DeluxeShoppingEntities();
        UserChecking Checking = new UserChecking();
       public ActionResult SignUp()
        {
            FaceBookConnect.API_Key = "761905080858164";
            FaceBookConnect.API_Secret = "962d097a2ef51220b74329509a6f61fa";

            user user = new user();
            customers customers = new customers();
           
            if (Request.QueryString["code"] == "access_denied")
            {
                ViewBag.Message = "user has denied access";
            }
            try
            { 
                  string code = Request.QueryString["code"];
                    if (!string.IsNullOrEmpty(code))
                    {
                        string data = FaceBookConnect.Fetch(code, "me?fields=name,email");
                        customers = new JavaScriptSerializer().Deserialize<customers>(data);

                        Session["mail"] = customers.email;
                        Session["username"] = customers.name;
                   var validate = Checking.CheckInUser(customers.email);
                    if(validate == true)
                    {
                        ViewBag.validationMSg = "Already Existed";
                    }
                    else
                    {
                        user.emails = customers.email;
                        user.name = customers.name;
                        db.users.Add(user);
                        db.SaveChanges();
                    }
                        
                    }
        
            }
           catch(Exception ex)
            {
                ViewBag.ErrorMSG = ex;
            }
            return View(user);
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

        public ActionResult Logout()
        {
            Session.Clear();
          return  RedirectToAction("Home", "UserAction");
        }
    }
}