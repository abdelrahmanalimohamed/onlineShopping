﻿using System;
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
        List<int> selected_id = new List<int>();
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

        [HttpGet]
        public ActionResult showitem(int id)
        {
            var selecteditem = (from a in db.products
                                where a.ID == id
                                select a).ToList();
            
            Session["itemid"] = id;

            return View(selecteditem);
        }

        [HttpPost]
    
        public JsonResult AddtoCart(int id)
        {
            shopping_cart_details shopping_Cart_Details = new shopping_cart_details();
            int last_order_id = 0;
            var checking = (from a in db.orders
                            select a.ID).Count();

          
           if(checking == 0)
            {
                last_order_id = checking + 1;
                var cart_icon = (from a in db.shopping_cart_details
                                 where a.order_id == last_order_id
                                 select a.order_id).Count();
                shopping_Cart_Details.item_id = id;
                 shopping_Cart_Details.order_id = last_order_id;
                db.shopping_cart_details.Add(shopping_Cart_Details);
                db.SaveChanges();
            }
            else
            {
                var getmax = (from a in db.orders
                              select a.ID).Max();
                var cart_icon = (from a in db.shopping_cart_details
                                 where a.order_id == getmax
                                 select a.order_id).Count();
                last_order_id = getmax + 1;
                shopping_Cart_Details.item_id = id;
                shopping_Cart_Details.order_id = last_order_id;
                db.shopping_cart_details.Add(shopping_Cart_Details);
                db.SaveChanges();
            }
          
            return Json("Added to cart", JsonRequestBehavior.AllowGet);
        }


        public ActionResult Home()
        {
         
            return View(db.products.ToList());
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
          return  RedirectToAction("Home", "UserActions","/Controllers/User");
        }
    }
}