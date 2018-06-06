using Area.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Area.Web.Controllers
{
    [VerifyUser]
    public class VehicleInfoController : Controller
    {
        // GET: VehicleInfo
        public ActionResult Index()
        {
            return View();
        }
    }
}