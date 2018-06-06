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
    public class VisitPlaceInfoController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();

        [Route("visitplaceinfo/{id?}")]
        public ActionResult Index(int? id)
        {
            int userID = Convert.ToInt32(Session["UserId"]); //TODO: buraya session uzerınden user gelecek
            var visitplacaInfo = db.VisitPlaceInfoes.Where(p => p.IsActive == true && p.UserID == userID && p.VisitPlaceID == id).FirstOrDefault();
            return View(visitplacaInfo == null ? new VisitPlaceInfo() : visitplacaInfo);
        }

        [Route("visitplaceinfo/create")]
        [HttpPost]
        public ActionResult Create(VisitPlaceInfo visitplaceInfo)
        {
            var visitplaceInfoEntity = db.VisitPlaceInfoes.Where(p => p.ID == visitplaceInfo.ID).FirstOrDefault();
            if (visitplaceInfoEntity == null)
            {
                visitplaceInfo.CreateDate = DateTime.Now;
                visitplaceInfo.IsActive = true;
                visitplaceInfo.UserID = Convert.ToInt32(Session["UserId"]);//TODO: Sonradan buraya Session uzerinden userId cek
                db.VisitPlaceInfoes.Add(visitplaceInfo);
                db.SaveChanges();
                return Redirect("/VisitPlaceStock/" + visitplaceInfo.VisitPlaceID);
            }
            else
            {
                visitplaceInfoEntity.PlaceCapacity = visitplaceInfo.PlaceCapacity;
                visitplaceInfoEntity.PeopleInterviewed = visitplaceInfo.PeopleInterviewed;
                visitplaceInfoEntity.PeopleSurvey = visitplaceInfo.PeopleSurvey;
                visitplaceInfoEntity.PeopleToTasted = visitplaceInfo.PeopleToTasted;
                visitplaceInfoEntity.PlaceNegativeComment = visitplaceInfo.PlaceNegativeComment;
                visitplaceInfoEntity.PlacePositiveComment = visitplaceInfo.PlacePositiveComment;
                visitplaceInfoEntity.CustomerNegativeComment = visitplaceInfo.CustomerNegativeComment;
                visitplaceInfoEntity.CustomerPositiveComment = visitplaceInfo.CustomerPositiveComment;
                visitplaceInfoEntity.PlaceSaleProduct = visitplaceInfo.PlaceSaleProduct;
                db.Entry(visitplaceInfoEntity);
                db.SaveChanges();
                return Redirect("/VisitPlaceStock/" + visitplaceInfo.VisitPlaceID);
            } 
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