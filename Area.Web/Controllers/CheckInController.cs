using Area.Data;
using Area.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Area.Web.Controllers
{
    [VerifyUser]
    public class CheckInController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();

        [Route("checkin/{id?}")]
        public ActionResult Index(int? id)
        {
            var result = db.VisitPlaces.Where(p => p.ID == id).FirstOrDefault().PlaceCheckInfo;
            return View(result); 
        }

        [Route("checkin")]
        [HttpPost] 
        public ActionResult Index(Location input)
        {
            if (ModelState.IsValid)
            { 
                var visitEntity = db.VisitPlaces.Where(p => p.ID == input.VisitID).FirstOrDefault();
                var checkEntity = new PlaceCheckInfo
                {
                    CheckinDate = DateTime.Now,
                    CheckinLatitude = "41.077194",
                    CheckinLongitude = "29.009297", 
                    IsActive = true
                };
                visitEntity.PlaceCheckInfo = checkEntity;
                db.Entry(visitEntity);
                db.SaveChanges(); 
            }
            return RedirectToAction("/" + input.VisitID);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public class Location
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public int VisitID { get; set; }
        }
    } 
}