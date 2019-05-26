using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeluxeModel;
using System.Data;

namespace DeluxeProject.Controllers
{
    public class DemoController : Controller
    {
        DeluxeShoppingEntities db = new DeluxeShoppingEntities();
        
        #region Product
        public ActionResult Details(int id)
        {
            TempData["itemid"] = id;
            return PartialView("_Details", db.products.Find(id));
        }

        public ActionResult Update(product product)
        {

            if (product.ID > 0)
            {
                var productdata = (from a in db.products
                                   where a.ID == product.ID
                                   select a).FirstOrDefault();

                if (productdata != null)
                {
                    productdata.prd_name = product.prd_name;
                    productdata.prd_desc = product.prd_desc;
                    productdata.prd_quantity = product.prd_quantity;
                    productdata.price = product.price;
                }

                //   var update = db.Entry(product).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json("Update Sucess", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Update Failed", JsonRequestBehavior.AllowGet);
            }

        }




        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {
            var deleteproduct = (from a in db.products
                                 where a.ID == id
                                 select a).FirstOrDefault();

            db.products.Remove(deleteproduct);
            db.SaveChanges();
            return Json("Deletion is Done");
        }
        #endregion
        
        #region Brand
        public ActionResult BrandDetails(int id)
        {
            TempData["itemid"] = id;
            return PartialView("_BrandDetails", db.Brands.Find(id));
        }


        public ActionResult BrandUpdate(Brand brand)
        {

            if (brand.ID > 0)
            {
                var branddata = (from a in db.Brands
                                 where a.ID == brand.ID
                                 select a).FirstOrDefault();

                if (branddata != null)
                {
                    branddata.Brnd_name = brand.Brnd_name;

                }

                //   var update = db.Entry(product).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json("Update Sucess", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Update Failed", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteBrand(int id)
        {
            var deletebrand = (from a in db.Brands
                               where a.ID == id
                               select a).FirstOrDefault();

            db.Brands.Remove(deletebrand);
            db.SaveChanges();
            return Json("Deletion is Done");
        }
        #endregion

        #region Catgory
        public ActionResult CategoryDetails(int id)
        {
            return PartialView("_CategroyDetails", db.Categories.Find(id));
        }

        public ActionResult CategoryUpdate(Category category)
        {

            if (category.ID > 0)
            {
                var categorydata = (from a in db.Categories
                                 where a.ID == category.ID
                                 select a).FirstOrDefault();

                if (categorydata != null)
                {
                    categorydata.Category_name = category.Category_name;

                }

              

                db.SaveChanges();

                return Json("Update Sucess", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Update Failed", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            var deletecatg = (from a in db.Categories
                               where a.ID == id
                               select a).FirstOrDefault();

            db.Categories.Remove(deletecatg);
            db.SaveChanges();
            return Json("Deletion is Done");
        }

        #endregion

        #region Supplier
        public ActionResult SupplierDetails(int id)
        {
            return PartialView("_SupplierDetails", db.suppliers.Find(id));
        }

        public ActionResult UpdateSupplier(supplier supplier)
        {

            if (supplier.ID > 0)
            {
                var supplierdata = (from a in db.suppliers
                                   where a.ID == supplier.ID
                                   select a).FirstOrDefault();

                if (supplierdata != null)
                {
                    supplierdata.company_name = supplier.company_name;
                    supplierdata.company_contact = supplier.company_contact;
                    supplierdata.company_mail = supplier.company_mail;
                    supplierdata.company_location = supplier.company_location;
                }
                
                db.SaveChanges();

                return Json("Update Sucess", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Update Failed", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteSupplier(int id)
        {
            var deletesupplier = (from a in db.suppliers
                               where a.ID == id
                               select a).FirstOrDefault();

            db.suppliers.Remove(deletesupplier);
            db.SaveChanges();
            return Json("Deletion is Done");
        }

        #endregion

        #region order
        public ActionResult UpdateCart(int id , int amount )
        {
            var orderid = 27;

           if(id >0)
            {
                var update = (from a in db.shopping_cart_details
                              where a.item_id == id && a.order_id == orderid
                              select a).FirstOrDefault();
                if(update != null)
                {
                    update.item_amount = amount;
                }

                db.SaveChanges();

                return Json("Update made Successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Update failed", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveOrder(string payment_type)
        {
            order order = new order();
            order_details order_Details = new order_details();

            var getid = (from a in db.orders
                         select a.ID).Count();
            if (getid == 0)
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
                var finalqty = prods.prd_quantity - int.Parse(TempData["amount"].ToString());
                order_Details.order_date = DateTime.Now;
                order_Details.order_id = getid;
                db.order_details.Add(order_Details);
                db.SaveChanges();

            }

            return Json("Order Made Correctly", JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region Users

        #endregion


    }
}
