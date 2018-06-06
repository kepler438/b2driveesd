using Area.Data;
using Area.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Area.Web.Controllers
{
    [VerifyUser]
    public class UploadPhotosController : Controller
    {
        // GET: Photo
        [Route("uploadphotos/{id?}")]
        public ActionResult Index(int? id)
        {
            using (B2DriveForPostEntities db = new B2DriveForPostEntities())
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var result = db.PlacePhotoes.Where(p => p.VisitPlaceID == id && p.UserID == userId && p.IsActive == true).ToList();
                return View(result);
            } 
        }
        [Route("uploadphotos/{id?}")]
        [HttpPost]
        public ActionResult SaveImages(int? id)
        {
            if (Request.Files.Count > 0)
            {
                using (B2DriveForPostEntities db = new B2DriveForPostEntities())
                {
                    PlacePhoto pp;
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(Guid.NewGuid().ToString().Substring(0, 5) + "_" + file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Dosyalar/"), fileName);
                            file.SaveAs(path);
                            pp = new PlacePhoto();
                            pp.CreateDate = DateTime.Now;
                            pp.IsActive = true;
                            pp.PhotoUrl = "/Dosyalar/" + fileName;
                            pp.UserID = Convert.ToInt32(Session["UserId"]);
                            pp.VisitPlaceID = id.HasValue ? id.Value : 0;
                            db.PlacePhotoes.Add(pp);
                            db.SaveChanges();
                        }
                    }
                }
            }


            return RedirectToAction("Index");
        }
         
        public ActionResult Delete(int id)
        {
            using (B2DriveForPostEntities db = new B2DriveForPostEntities())
            {
                var result = db.PlacePhotoes.Where(p => p.ID == id).FirstOrDefault();
                db.PlacePhotoes.Remove(result);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}