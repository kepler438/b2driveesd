using Area.Data;
using Area.Web.Attributes;
using Area.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Area.Web.Controllers
{
    [VerifyUser]
    public class ReceivedProductsController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();
        // GET: ReceivedProducts
        [Route("receivedproducts/{id?}")]
        public ActionResult Index(int? id)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            var result = db.ProductRecivedDelivereds.Include(x => x.Product.ProductSubCategory.ProductCategory).Where(p => p.VisitPlaceID == id && p.UserID == userId && p.IsActive == true).ToList();
            ViewData["ProductCategory"] = new SelectList(db.ProductCategories.Where(p => p.IsActive == true), "ID", "Name");
            return View(result);
        }

        [Route("receivedproducts/GetSubCategories/{categoryid?}")]
        public JsonResult GetSubCategories(string categoryid)
        {
            int catId = string.IsNullOrEmpty(categoryid) ? 0 : Convert.ToInt32(categoryid);
            var list = db.ProductSubCategories.Where(x => x.CategoryID == catId).ToList();
            return Json(new SelectList(list, "ID", "Name"));
        }

        [Route("receivedproducts/GetProducts/{subcategoryid?}")]
        public JsonResult GetProducts(string subcategoryid)
        {
            int subCatId = string.IsNullOrEmpty(subcategoryid) ? 0 : Convert.ToInt32(subcategoryid);
            var list = db.Products.Where(x => x.SubCategoryID == subCatId).ToList();
            return Json(new SelectList(list, "ID", "Name"));
        }

        [Route("receivedproducts/SaveReceivedProducts/{jsonData?}")]
        public ActionResult SaveReceivedProducts(ReceivedProductEntity jsonData)
        {
            ProductRecivedDelivered entity = new ProductRecivedDelivered();
            entity.UserID = Convert.ToInt32(Session["UserId"]);
            entity.ProductID = Convert.ToInt32(jsonData.Product);
            entity.VisitPlaceID = Convert.ToInt32(jsonData.VisitPlaceID);
            entity.Quantity = Convert.ToInt32(jsonData.Quantity); //TODO: Patlamaması için veri kontrolü
            entity.Note = jsonData.Note;
            entity.CreateDate = DateTime.Now;
            entity.IsActive = true;
            db.ProductRecivedDelivereds.Add(entity);
            db.SaveChanges();
            return RedirectToAction("/" + jsonData.VisitPlaceID);
        }

        [Route("receivedproducts/EditReceivedProducts/{jsonData?}")]
        public ActionResult EditReceivedProducts(ReceivedProductEntity jsonData)
        {
            var productReceivedEntity = db.ProductRecivedDelivereds.Find(jsonData.ID); 
            productReceivedEntity.ProductID = Convert.ToInt32(jsonData.Product);
            productReceivedEntity.Quantity = Convert.ToInt32(jsonData.Quantity);
            productReceivedEntity.Note = jsonData.Note;
            db.Entry(productReceivedEntity);
            db.SaveChanges(); 
            return RedirectToAction("/" + jsonData.VisitPlaceID);
        }
         
        [Route("receivedproducts/delete/{id}")]
        public ActionResult Delete(int id)
        {
            ProductRecivedDelivered product = db.ProductRecivedDelivereds.Find(id);
            product.IsActive = false;
            db.Entry(product);
            db.SaveChanges();
            return RedirectToAction("/" + product.VisitPlaceID);
        }

        [HttpGet]
        [Route("receivedproducts/edit/{id}")]
        public ActionResult Edit(int id)
        {
            var product = db.ProductRecivedDelivereds.Find(id);
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