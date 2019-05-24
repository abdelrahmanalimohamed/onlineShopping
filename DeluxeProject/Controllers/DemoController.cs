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


# region Users

#endregion


    }
}