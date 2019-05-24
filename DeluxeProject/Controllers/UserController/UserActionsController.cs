using System;
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
            FaceBookConnect.API_Key = "2362878207096245";
            FaceBookConnect.API_Secret = "ee048380218cf439e4c09605d01b2b93";

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



        [HttpPost]
        public EmptyResult Login()
        {
            FaceBookConnect.Authorize("email", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, "UserActions/SignUp/"));
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult showitem(int id)
        {
            var selecteditem = (from a in db.products
                                where a.ID == id
                                select a).ToList();

            Session["itemid"] = id;
            var _relateditems = (from a in db.products
                                 where a.ID == id ||
                                 a.Categ_id == (from cat in db.products where cat.ID == id select cat.Categ_id).FirstOrDefault()
                                 select a).ToList();

            ViewBag.HoldingItems = _relateditems;

            return View(selecteditem);

        }

        public ActionResult Checkout()
        {
            return View();
        }
 
        
        public ActionResult RemoveItemfromcart(int id)
        {
            var order_id = (from a in db.orders
                                 select a.ID).Max() + 1;

            var delete_item = (from a in db.shopping_cart_details
                               where a.order_id == order_id && 
                               a.item_id == id
                               select a).FirstOrDefault();

            db.shopping_cart_details.Remove(delete_item);
            db.SaveChanges();

            return Json("Removed" , JsonRequestBehavior.AllowGet);
            
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
                var finalqty = prods.prd_quantity - int.Parse(TempData["amount"].ToString()) ;
                order_Details.order_date = DateTime.Now;
                order_Details.order_id = getid;
                db.order_details.Add(order_Details);
                db.SaveChanges();
              
            }

            return Json("Order Made Correctly", JsonRequestBehavior.AllowGet);



        }

       
        public JsonResult AddtoCart(int id , int amount)
        {
            Session["iding"] = id;
            TempData["amount"] = amount;
        
            shopping_cart_details shopping_Cart_Details = new shopping_cart_details();
            int last_order_id;
            var checking = (from a in db.orders
                            select a.ID).Count();

            var itemprice = (from a in db.products
                             where a.ID == id
                             select a.price).FirstOrDefault();
            int item_price = Convert.ToInt32(itemprice);
            double totalfees = item_price * amount;
            TempData["totalcalc"] = totalfees;
            if (checking == 0)
                {
                    last_order_id = checking + 1;

                  var cart_icon = (from a in db.shopping_cart_details
                                     where a.order_id == last_order_id
                                     select a.order_id).Count();
                     Session["itemcount"] = cart_icon;
                     shopping_Cart_Details.item_id = id;
                     shopping_Cart_Details.item_price = itemprice;
                     shopping_Cart_Details.item_amount = amount;
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
                    shopping_Cart_Details.item_price = totalfees.ToString();
                    shopping_Cart_Details.item_amount = amount;
                    shopping_Cart_Details.order_id = last_order_id;
                    db.shopping_cart_details.Add(shopping_Cart_Details);
                    db.SaveChanges();
                }
                
            return Json("Added to cart" , JsonRequestBehavior.AllowGet);

        }


       
        public ActionResult CheckOutPage()
        {

            var itemscounted = (from a in db.shopping_cart_details
                                select a).Count();
            ViewBag.itemCounted = itemscounted;

            return View();
        }

        public ActionResult CartPage()
        {
            int item_id = Convert.ToInt32(Session["iding"]);
            var order_id_ = (from a in db.orders
                             select a.ID).Max() + 1;

            var checkout = (from a in db.shopping_cart_details
                            where a.order_id == order_id_
                            join b in db.products on a.item_id equals b.ID
                            join c in db.suppliers on b.supplier_id equals c.ID
                            select b).ToList();

            var itemsum = (from a in db.shopping_cart_details.AsEnumerable()
                           where a.order_id == order_id_
                           join b in db.products on a.item_id equals b.ID
                           select a).Sum(w => Convert.ToDecimal(w.item_price));

            var itemamount = (from a in db.shopping_cart_details
                              where a.order_id == order_id_
                              join b in db.products on a.item_id equals b.ID
                              select a.item_amount).ToList();


            ViewBag.amount_ = itemamount;
            ViewBag.TotalSum = itemsum;
            TempData["totalprice"] = itemsum;

           
            return View(checkout);
        }
        public ActionResult shoppingcartitem()
        {
            int item_id = Convert.ToInt32(Session["iding"]);
            var order_id_ = (from a in db.orders
                             select a.ID).Max() + 1;

            var checkout = (from a in db.shopping_cart_details
                            where a.order_id == order_id_
                            join b in db.products on a.item_id equals b.ID
                            join c in db.suppliers on b.supplier_id equals c.ID
                            select b).ToList();

            var itemsum = (from a in db.shopping_cart_details.AsEnumerable()
                           where a.order_id == order_id_
                           join b in db.products on a.item_id equals b.ID
                           select a).Sum(w => Convert.ToDecimal(w.item_price));

            var itemamount = (from a in db.shopping_cart_details
                              where a.order_id == order_id_
                              join b in db.products on a.item_id equals b.ID
                              select a.item_amount).ToList();

            ViewBag.amount_ = itemamount;
            ViewBag.TotalSum = itemsum;
            TempData["totalprice"] = itemsum;

            return View(checkout);

        }

    
        

        public ActionResult simple()
        {
            var order_id = (from a in db.orders
                            select a.ID).Count() + 1;

            var last_id = (from a in db.shopping_cart_details
                           where a.order_id == order_id
                           select a.order_id).Count();

            var itemscounted = (from a in db.shopping_cart_details
                                select a).Count();
            ViewBag.itemCounted = itemscounted;


     

            Session["itemcount"] = last_id;
            return View(db.products.ToList());
        }
      
        
        public ActionResult Login(string username , string password)
        {
            var validation = (from a in db.users
                              where a.username == username && a.passowrds == password
                              select a).Any();

            var fullname = (from a in db.users
                               where a.username == username && a.passowrds == password
                               select a.name).FirstOrDefault();
            if (validation != true)
            {
                return Json("Not Valid username or password", JsonRequestBehavior.AllowGet);
            }
            else
            {
                Session["username"] = username;
                Session["fullname"] = fullname;
                return Json("Valid Data entered", JsonRequestBehavior.AllowGet);

            }
          
        }
        

        public ActionResult Logout()
        {
          Session.Clear();
          return  RedirectToAction("Simple", "UserActions","/Controllers/User");
        }
    }
}