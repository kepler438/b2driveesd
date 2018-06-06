using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Area.Data;
using Area.Web.Attributes;

namespace Area.Web.Controllers
{
    [RoutePrefix("home")]
    [VerifyUser]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (B2DriveForPostEntities db = new B2DriveForPostEntities())
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var visitPlaces = db.VisitPlaces.Where(p => p.UserID == userId).Include(v => v.Place).Include(v => v.PlaceCheckInfo).Include(v => v.User).Include(v=>v.Region);
                return View(visitPlaces.ToList());
            }
        }
    }
}
