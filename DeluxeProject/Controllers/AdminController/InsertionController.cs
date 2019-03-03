using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeluxeModel;

namespace DeluxeProject.Controllers.AdminController
{
    public class InsertionController : Controller
    {
        DeluxeShoppingEntities db = new DeluxeShoppingEntities();
     

        [HttpPost]
        public JsonResult InsertBrand(string name)
        {
           var x = db.insertbrand(name);
            if(x != 0)

            {
                return Json("Added", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Not added", JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public JsonResult InsertCategory(string name)
        {
           var x = db.insertcategory(name);
            if(x != 0)
               
            {
                return Json("Added", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Not added", JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpPost]
        public JsonResult InsertSupplier(supplier supplier)
        {
           var y = db.insertsupplier(supplier.company_name, supplier.company_contact, supplier.company_mail, supplier.company_location);
         
            if (y != 0)
            {
                return Json("Added", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Not added", JsonRequestBehavior.AllowGet);
            }
       
        }

        [HttpPost]
        public JsonResult InsertProduct(product product)
        {
            Supplier_Details supplier_Details = new Supplier_Details();

            supplier_Details.supplier_id = product.supplier_id;
            supplier_Details.supplier_prd_date = DateTime.Now;
            supplier_Details.supplier_prd_desc = product.prd_desc;
            supplier_Details.supplier_product_categroy = product.Categ_id;
            supplier_Details.supplier_product_name = product.prd_name;
            supplier_Details.supplier_prd_price = product.price;
            var insertsupplierdetails = db.Supplier_Details.Add(supplier_Details);
            db.SaveChanges();

            if (product.fileupload != null )
            {
                string fileName = Path.GetFileNameWithoutExtension(product.fileupload.FileName);
                string extension = Path.GetExtension(product.fileupload.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                product.prd_img = fileName;
                product.fileupload.SaveAs(Path.Combine(Server.MapPath("~/App_File/Images"), fileName));
                db.products.Add(product);
                db.SaveChanges();
                return Json("Added", JsonRequestBehavior.AllowGet);
            }
         
            
          
            else
            {
                return Json("An Error Occured", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetAllSuppliers()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var allsuppliers = from a in db.suppliers
                               select a;
            return Json(allsuppliers , JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAllBrands()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var allsuppliers = from a in db.Brands
                               select a;
            return Json(allsuppliers, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetALLProducts()
        {
            return Json(db.products.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllCategories()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var allcategories = from a in db.Categories
                                select a;
            return Json(allcategories, JsonRequestBehavior.AllowGet);
        }



       
    }
}