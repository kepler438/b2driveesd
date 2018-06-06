using Area.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Area.Web.Attributes;

namespace Area.Web.Controllers
{
    [VerifyUser]
    public class VisitPlaceStockController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();

        [Route("visitplacestock/{id?}")]
        public ActionResult Index(int? id)
        {
            var categoryList = db.ProductCategories.Where(p => p.IsActive == true).ToList();
            categoryList.Add(new ProductCategory()
            { ID = 0, Name = "Lütfen Bir Kategori Seçiniz." });
            ViewData["productCategory"] = new SelectList(db.ProductCategories.Where(p => p.IsActive == true && p.ID != 2), "ID", "Name");
            //ViewData["productCategory"] = new SelectList(categoryList.OrderBy(p => p.ID), "ID", "Name");
            var productsalesList = db.ProductSales.Where(p => p.IsActive == true && p.VisitPlaceID == id && p.SaleType == 3).ToList();
            return View(productsalesList);
        }

        [Route("visitplacestock/getsubcategory/{id}")]
        public JsonResult GetSubCategory(int id)
        {
            var subcategories = db.ProductSubCategories.Where(p => p.CategoryID == id).ToList();
            return Json(new SelectList(subcategories, "ID", "Name"));
        }

        [Route("visitplacestock/getproduct/{id}")]
        public JsonResult GetProduct(int id)
        {
            var products = db.Products.Where(p => p.SubCategoryID == id).ToList();
            return Json(new SelectList(products, "ID", "Name"));
        }

        [Route("visitplacestock/create")]
        [HttpPost]
        public ActionResult Create(ProductSale productSale)
        {
            if (ModelState.IsValid)
            {
                productSale.CreateDate = DateTime.Now;
                productSale.IsActive = true;
                productSale.UserID = Convert.ToInt32(Session["UserId"]);//TODO: Sonradan buraya Session uzerinden userId cek
                db.ProductSales.Add(productSale);
                db.SaveChanges();
                return RedirectToAction("/" + productSale.VisitPlaceID);
            }
            return View(productSale);
        }

        [Route("visitplacestock/delete/{id}")]
        public ActionResult Delete(int id)
        {
            ProductSale productSale = db.ProductSales.Find(id);
            productSale.IsActive = false;
            db.Entry(productSale);
            db.SaveChanges();
            return RedirectToAction("/" + productSale.VisitPlaceID);
        }

        [Route("visitplacestock/EditProduct/{jsonData?}")]
        public ActionResult EditProduct(ProductSale jsonData)
        {
            var productEntity = db.ProductSales.Find(jsonData.ID);
            productEntity.ProductID = Convert.ToInt32(jsonData.ProductID);
            productEntity.Quantity = Convert.ToInt32(jsonData.Quantity);
            productEntity.Note = jsonData.Note; 
            db.Entry(productEntity);
            db.SaveChanges();
            return RedirectToAction("/" + jsonData.VisitPlaceID);
        }

        [HttpGet]
        [Route("visitplacestock/edit/{id}")]
        public ActionResult Edit(int id)
        {
            var product = db.ProductSales.Find(id);
            TempData["EditProduct"] = product;
            return RedirectToAction("/" + product.VisitPlaceID);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}