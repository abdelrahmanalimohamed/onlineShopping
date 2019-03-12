﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Collections;
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
            customer customers = new customer();
           
            if (Request.QueryString["code"] == "access_denied")
            {
                ViewBag.Message = "user has denied access";
            }
            try
            { 
                  string code = Request.QueryString["code"];
                    if (!string.IsNullOrEmpty(code))
                    {
                        string data = FaceBookConnect.Fetch(code, "me?fields=name,email,address");
                        customers = new JavaScriptSerializer().Deserialize<customer>(data);

                        Session["faceboomail"] = customers.email;
                        Session["facebookname"] = customers.name;
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
        public JsonResult Saveuser(user user)
        {
            db.users.Add(user);
            db.SaveChanges();

            return Json("saved", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOrder(string payment_type)
        {
            order order = new order();
            order_details order_Details = new order_details();

          
              
                var getid = (from a in db.orders
                         select a.ID).Count();
               if(getid == 0)
                {
                    getid = 1;
                }
                else
                {
                    getid = (from a in db.orders
                             select a.ID).Max() + 1;
                }


                   var totalsum = (from a in db.shopping_cart_details
                                   where getid == a.order_id
                                   select a.item_amount).Sum();
           
                 //var userid = (from a in db.users
                 //             where a.name == Session["username"].ToString() || a.name == Session["facebookusername"].ToString()
                 //             select a.ID).FirstOrDefault();

                var payment_id = (from a in db.Payments
                                  where a.payment_type == payment_type
                                  select a.ID).FirstOrDefault();

               
                order.date_created = DateTime.Now;
                order.total_sum = double.Parse(TempData["totalprice"].ToString());
                order.amount = totalsum;
             //   order.user_id = userid;
                order.payment_id = payment_id;
                db.orders.Add(order);
                db.SaveChanges();


             var products = (from a in db.shopping_cart_details
                                where a.order_id == getid
                                join b in db.products on a.item_id equals b.ID
                                select b).ToList();

            var itemoncart = (from a in db.shopping_cart_details
                              where getid == a.order_id
                              select a).ToList();

            foreach (var prods in products)
           {
                foreach (var item in itemoncart)
                {
                    order_Details.order_prd_qty = item.item_amount.ToString();
                }
                order_Details.order_prd_name = prods.prd_name;
                order_Details.order_prd_price = prods.price;
                order_Details.prd_id = prods.ID;
               
                order_Details.order_date = DateTime.Now;
                order_Details.order_id = getid;
                db.order_details.Add(order_Details);
                db.SaveChanges();
              
            }












            return Json("Order Made Correctly", JsonRequestBehavior.AllowGet);



        }

        [HttpPost]
        public JsonResult AddtoCart(int id)
        {
            Session["iding"] = id;
            shopping_cart_details shopping_Cart_Details = new shopping_cart_details();
            int last_order_id;
            var checking = (from a in db.orders
                            select a.ID).Count();

            var itemprice = (from a in db.products
                             where a.ID == id
                             select a.price).FirstOrDefault();
            if (checking == 0)
                {
                    last_order_id = checking + 1;

                  var cart_icon = (from a in db.shopping_cart_details
                                     where a.order_id == last_order_id
                                     select a.order_id).Count();

             

                     Session["itemcount"] = cart_icon;
                     shopping_Cart_Details.item_id = id;
                     shopping_Cart_Details.item_price = itemprice;
                     shopping_Cart_Details.item_amount = 1;
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
                    Session["itemcount"] = cart_icon;
                    last_order_id = getmax + 1;
                    shopping_Cart_Details.item_id = id;
                    shopping_Cart_Details.item_price = itemprice;
                    shopping_Cart_Details.item_amount = 1;
                    shopping_Cart_Details.order_id = last_order_id;
                    db.shopping_cart_details.Add(shopping_Cart_Details);
                    db.SaveChanges();
                }
                
            return Json("Added to cart" , JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public JsonResult UpdateAmount(int id)
        //{
        //    var upodate = (from a in db.shopping_cart_details
        //                   where a.item_id == id
        //                   select a.item_amount);


        //}

        public ActionResult shoppingcartitem()
        {
            int item_id = Convert.ToInt32(Session["iding"]);
            var order_id_ = (from a in db.orders
                             select a.ID).Count() + 1;

            var checkout = (from a in db.shopping_cart_details
                            where a.order_id == order_id_
                            join b in db.products on a.item_id equals b.ID
                            select b).ToList();

            var itemsum = (from a in db.shopping_cart_details.AsEnumerable()
                           where a.order_id == order_id_
                           join b in db.products on a.item_id equals b.ID
                           select b).Sum(w => Convert.ToDecimal(w.price));

            ViewBag.TotalSum = itemsum;
            TempData["totalprice"] = itemsum;

            return View(checkout);

        }


        public ActionResult Home()
        {
            var order_id = (from a in db.orders
                            select a.ID).Count()+1;

            var last_id = (from a in db.shopping_cart_details
                           where a.order_id == order_id
                           select a.order_id).Count();

            Session["itemcount"] = last_id;
            return View(db.products.ToList());
        }

      
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            order order = new order();
            order.user_id = Convert.ToInt32(Session["userid"]);
         //   order.ordername = (from a in db.shopping_cart_details)
            return View();
        }

        public ActionResult Logout()
        {
          Session.Clear();
          return  RedirectToAction("Home", "UserActions","/Controllers/User");
        }
    }
}