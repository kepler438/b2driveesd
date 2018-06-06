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
    public class SPCarInfoController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities(); 
        [Route("spcarinfo/{id?}")]
        public ActionResult Index(int? id) 
        {
            var carinfo = db.VisitPlaceCarInfoes.Where(p => p.VisitPlaceID == id).Include(v => v.RentACar).FirstOrDefault(); 
            return View(carinfo);
        }

        [Route("checkinspcar")]
        [HttpPost]
        public ActionResult Index(Location input)
        {
            if (ModelState.IsValid)
            {
                var visitEntity = db.VisitPlaceCarInfoes.Where(p => p.VisitPlaceID == input.VisitID).FirstOrDefault();
                visitEntity.CheckinDate = DateTime.Now;
                visitEntity.CheckinLatitude = input.Latitude;
                visitEntity.CheckinLongitude = input.Longitude; 
                db.Entry(visitEntity);
                db.SaveChanges();
            }
            return RedirectToAction("/" + input.VisitID);
        }

        [Route("spcarinfo/checkoutpage/{id?}")]
        public ActionResult CheckOutPage(int? id)
        {
            var carinfo = db.VisitPlaceCarInfoes.Where(p => p.VisitPlaceID == id).FirstOrDefault();
            return View(carinfo); 
        }

        [Route("checkoutspcar")]
        [HttpPost]
        public ActionResult CheckoutSpCar(Location input)
        {
            var carinfo = db.VisitPlaceCarInfoes.Where(p => p.VisitPlaceID == input.VisitID).FirstOrDefault();
            carinfo.CheckoutDate = DateTime.Now;
            db.Entry(carinfo);
            db.SaveChanges();
            return Redirect("/sphome");
        }

        public class Location
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public int VisitID { get; set; }
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