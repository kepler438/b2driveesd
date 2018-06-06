using Area.Data;
using Area.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Area.Web.Controllers
{
    [VerifyUser]
    public class WareHouseController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();
        [Route("warehouse/{id?}")]
        public ActionResult Index(int? id)
        {
            var result = db.VisitPlaceWareHouses.Where(p => p.VisitPlaceID == id).Include(v => v.VisitPlace);
            return View(result);
        } 
        [Route("warehouse/detail/{id?}")]
        public ActionResult Detail(int? id)
        { 
            var result = db.ProductRecivedDelivereds.Include(x => x.Product.ProductSubCategory.ProductCategory).Where(p =>   p.IsActive == true && p.VisitPlaceWareHouseID == id && p.IsDeliveryWareHouse != true).ToList();
            ViewData["visitplaceWareHouse"] = db.VisitPlaceWareHouses.Where(p => p.ID == id).FirstOrDefault(); 
            ViewData["ProductCategory"] = new SelectList(db.ProductCategories.Where(p => p.IsActive == true), "ID", "Name");
            return View(result);
        }

        [Route("warehouse/productdelivery/{id?}")]
        public ActionResult ProductDelivery(int? id)
        {
            var result = db.ProductRecivedDelivereds.Include(x => x.Product.ProductSubCategory.ProductCategory).Where(p => p.IsActive == true && p.VisitPlaceWareHouseID == id && p.IsDeliveryWareHouse == true).ToList();
            ViewData["visitplaceWareHouse"] = db.VisitPlaceWareHouses.Where(p => p.ID == id).FirstOrDefault();
            ViewData["ProductCategory"] = new SelectList(db.ProductCategories.Where(p => p.IsActive == true), "ID", "Name");
            return View(result);
        }

        [Route("warehouse/GetSubCategories/{categoryid?}")]
        public JsonResult GetSubCategories(string categoryid)
        {
            int catId = string.IsNullOrEmpty(categoryid) ? 0 : Convert.ToInt32(categoryid);
            var list = db.ProductSubCategories.Where(x => x.CategoryID == catId).ToList();
            return Json(new SelectList(list, "ID", "Name"));
        }
        [Route("warehouse/GetProducts/{subcategoryid?}")]
        public JsonResult GetProducts(string subcategoryid)
        {
            int subCatId = string.IsNullOrEmpty(subcategoryid) ? 0 : Convert.ToInt32(subcategoryid);
            var list = db.Products.Where(x => x.SubCategoryID == subCatId).ToList();
            return Json(new SelectList(list, "ID", "Name"));
        }

        [Route("warehouse/create")]
        [HttpPost]
        public ActionResult Create(ProductRecivedDelivered receivedproduct)
        {
            if (ModelState.IsValid)
            {
                receivedproduct.CreateDate = DateTime.Now;
                receivedproduct.IsActive = true;
                receivedproduct.UserID = Convert.ToInt32(Session["UserId"]);//TODO: Sonradan buraya Session uzerinden userId cek
                db.ProductRecivedDelivereds.Add(receivedproduct);
                db.SaveChanges();
                return RedirectToAction("/Detail/" + receivedproduct.VisitPlaceWareHouseID);
            }
            return View("/Detail/" + receivedproduct.VisitPlaceWareHouseID);
        }

        [Route("warehouse/createdelivery")]
        [HttpPost]
        public ActionResult CreateDelivery(ProductRecivedDelivered receivedproduct)
        {
            if (ModelState.IsValid)
            {
                receivedproduct.CreateDate = DateTime.Now;
                receivedproduct.IsActive = true;
                receivedproduct.IsDeliveryWareHouse = true;
                receivedproduct.UserID = Convert.ToInt32(Session["UserId"]);//TODO: Sonradan buraya Session uzerinden userId cek
                db.ProductRecivedDelivereds.Add(receivedproduct);
                db.SaveChanges();
                return RedirectToAction("/ProductDelivery/" + receivedproduct.VisitPlaceWareHouseID);
            }
            return View("/ProductDelivery/" + receivedproduct.VisitPlaceWareHouseID);
        }

        [Route("warehouse/delete/{id}")]
        public ActionResult Delete(int id)
        {
            ProductRecivedDelivered product = db.ProductRecivedDelivereds.Find(id);
            product.IsActive = false;
            db.Entry(product);
            db.SaveChanges();
            return RedirectToAction("/Detail/" + product.VisitPlaceWareHouseID);
        }

        [Route("warehouse/deletedelivery/{id}")]
        public ActionResult DeleteDelivery(int id)
        {
            ProductRecivedDelivered product = db.ProductRecivedDelivereds.Find(id);
            product.IsActive = false;
            db.Entry(product);
            db.SaveChanges();
            return RedirectToAction("/ProductDelivery/" + product.VisitPlaceWareHouseID);
        }

        [Route("warehouse/checkin")]
        [HttpPost]
        public ActionResult Checkin(Location input)
        {
            if (ModelState.IsValid)
            {
                var visitEntity = db.VisitPlaceWareHouses.Where(p => p.ID == input.VisitPlaceWareHouseID).FirstOrDefault();
                visitEntity.CheckinDate = DateTime.Now;
                visitEntity.CheckinLatitude = input.Latitude;
                visitEntity.CheckinLongitude = input.Longitude; 
                db.Entry(visitEntity);
                db.SaveChanges();
            }
            return RedirectToAction("/Detail/" + input.VisitPlaceWareHouseID);
        }

        [Route("warehouse/checkout")]
        [HttpPost]
        public ActionResult Checkout(Location input)
        {
            if (ModelState.IsValid)
            {
                var visitEntity = db.VisitPlaceWareHouses.Where(p => p.ID == input.VisitPlaceWareHouseID).FirstOrDefault();
                visitEntity.CheckoutDate = DateTime.Now;
                visitEntity.CheckoutLatitude = input.Latitude;
                visitEntity.CheckoutLongitude = input.Longitude;
                db.Entry(visitEntity);
                db.SaveChanges();
            }
            return RedirectToAction("/ProductDelivery/" + input.VisitPlaceWareHouseID);
        }

        [Route("warehouse/EditReceivedProducts/{jsonData?}")]
        public ActionResult EditReceivedProducts(ProductRecivedDelivered jsonData)
        {
            var productReceivedEntity = db.ProductRecivedDelivereds.Find(jsonData.ID);
            productReceivedEntity.ProductID = Convert.ToInt32(jsonData.ProductID);
            productReceivedEntity.Quantity = Convert.ToInt32(jsonData.Quantity);
            productReceivedEntity.Note = jsonData.Note;
            db.Entry(productReceivedEntity);
            db.SaveChanges();
            return RedirectToAction("/Detail/" + jsonData.VisitPlaceWareHouseID);
        }

        [Route("warehouse/EditDeliveryProducts/{jsonData?}")]
        public ActionResult EditDeliveryProducts(ProductRecivedDelivered jsonData)
        {
            var productReceivedEntity = db.ProductRecivedDelivereds.Find(jsonData.ID);
            productReceivedEntity.ProductID = Convert.ToInt32(jsonData.ProductID);
            productReceivedEntity.Quantity = Convert.ToInt32(jsonData.Quantity);
            productReceivedEntity.Note = jsonData.Note;
            db.Entry(productReceivedEntity);
            db.SaveChanges();
            return RedirectToAction("/ProductDelivery/" + jsonData.VisitPlaceWareHouseID);
        }

        [HttpGet]
        [Route("warehouse/edit/{id}")]
        public ActionResult Edit(int id)
        {
            var product = db.ProductRecivedDelivereds.Find(id);
            TempData["EditProduct"] = product;
            return RedirectToAction("/Detail/" + product.VisitPlaceWareHouseID);
        }

        [HttpGet]
        [Route("warehouse/editdelivery/{id}")]
        public ActionResult EditDelivery(int id)
        {
            var product = db.ProductRecivedDelivereds.Find(id);
            TempData["EditProduct"] = product;
            return RedirectToAction("/ProductDelivery/" + product.VisitPlaceWareHouseID);
        }

        public class Location
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public int VisitPlaceWareHouseID { get; set; }
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