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
    public class CheckOutController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();

        [Route("checkout/{id?}")]
        public ActionResult Index(int? id)
        {
            var result = db.VisitPlaces.Where(p => p.ID == id).FirstOrDefault().PlaceCheckInfo;
            ViewData["availableUserInfo"] = db.AvailableUserVisits.Where(p => p.VisitID == id).FirstOrDefault();
            return View(result);
        }

        [Route("checkout")]
        [HttpPost]
        public ActionResult Index(Location input)
        {
            if (ModelState.IsValid)
            {
                var visitEntity = db.VisitPlaces.Where(p => p.ID == input.VisitID).FirstOrDefault();
                if (visitEntity.PlaceCheckInfo != null)
                {
                    visitEntity.PlaceCheckInfo.CheckOutDate = DateTime.Now;
                    visitEntity.PlaceCheckInfo.CheckoutLatitude = input.Latitude;
                    visitEntity.PlaceCheckInfo.CheckoutLongitude = input.Longitude;
                    db.Entry(visitEntity);
                    db.SaveChanges();
                }  
            }
            return RedirectToAction("/" + input.VisitID);
        }

        [Route("available")]
        [HttpPost]
        public ActionResult Available(AvailableUserVisit input)
        { 
            var availableEntity = db.AvailableUserVisits.Where(p => p.VisitID == input.VisitID).FirstOrDefault();
            if (availableEntity != null)
            { 
                availableEntity.Status = input.Status; 
                db.Entry(availableEntity);
                db.SaveChanges();
            }
            else
            {
                input.CreateDate = DateTime.Now;
                input.UserID = Convert.ToInt32(Session["UserId"]);
                db.AvailableUserVisits.Add(input);
                db.SaveChanges();
            } 
            return Redirect("/Home");
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