using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Area.Data;
using Area.Data.Enums;
using Area.Web.Attributes;

namespace Area.Web.Controllers
{
    public class AdminPlaceControlController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();
        [VerifyUser]
        public ActionResult Index()
        {
            var visitPlaces = (db.VisitPlaces.Include(v => v.Place).Include(v => v.PlaceCheckInfo).Include(v => v.Region).Include(v => v.User));
            List<VisitPlace> resultPlace = new List<VisitPlace>();
            foreach (var item in visitPlaces)
            {
                if (item.User.Permissions.FirstOrDefault().Id == (int)EnumUserType.Personnel)
                {
                    resultPlace.Add(item);
                }
            }
            return View(resultPlace); 
        }

        public ActionResult Supervisor()
        {
            //return Redirect("/AdminPlaceControl/Supervisor"); 
            return View();
        }

        public ActionResult ApprovedVisit(int id)
        {
            VisitPlace visitPlace = db.VisitPlaces.Find(id);
            visitPlace.IsApproved = true;
            db.Entry(visitPlace);
            db.SaveChanges();
            Session["AdminControl"] = false;
            Session["UserId"] = Session["FirstAdminId"];
            Session["FirstAdminId"] = null;
            var user = db.Users.Where(p => p.ID == visitPlace.UserID).FirstOrDefault();
            int UserType = user.Permissions.First().Id;
            if (UserType == (int)EnumUserType.Supervisor)
            {
                return Redirect("/adminplacecontrol/supervisor/");
            }
            return RedirectToAction("Index"); 
        }

        public ActionResult GotoPlace(int id)
        {
            VisitPlace visitPlace = db.VisitPlaces.Find(id);
            Session["AdminControl"] = true;
            Session["FirstAdminId"] = Convert.ToInt32(Session["UserId"]);
            Session["UserId"] = visitPlace.UserID;
            var user = db.Users.Where(p => p.ID == visitPlace.UserID).FirstOrDefault();
            int UserType = user.Permissions.First().Id;
             if (UserType == (int)EnumUserType.Supervisor)
            {
                return Redirect("/spplace/inspectioninfo/" + id);
            }
           return Redirect("/UploadPhotos/" + id);
        }

        public ActionResult CancelApproved()
        {
            Session["AdminControl"] = false;
            int userID = (int)Session["UserId"];
            Session["UserId"] = Session["FirstAdminId"];
            Session["FirstAdminId"] = null;
            var user = db.Users.Where(p => p.ID == userID).FirstOrDefault();
            int UserType = user.Permissions.First().Id;
            if (UserType == (int)EnumUserType.Supervisor)
            {
                return Redirect("/adminplacecontrol/supervisor/");
            }
            return RedirectToAction("Index");
        }

    }
}