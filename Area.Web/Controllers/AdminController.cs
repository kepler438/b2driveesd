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
    public class AdminController : Controller
    {
        /* esad */
        public ActionResult Index()
        {
            using (B2DriveForPostEntities db = new B2DriveForPostEntities())
            {
                var visitPlaces = db.VisitPlaces.Where(p => p.CheckInfoID != null).Include(v => v.Place).Include(v => v.PlaceCheckInfo).Include(v => v.User);
                return View(visitPlaces.ToList());
            }
        }
    }
}