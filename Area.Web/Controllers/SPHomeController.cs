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
    [RoutePrefix("sphome")]
    [VerifyUser]
    public class SPHomeController : Controller
    {
        // GET: SPHome
        public ActionResult Index()
        {
            using (B2DriveForPostEntities db = new B2DriveForPostEntities())
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var visitPlaces = db.VisitPlaces.Where(p => p.UserID == userId);
                return View(visitPlaces.ToList());
            }
        }
    }
}