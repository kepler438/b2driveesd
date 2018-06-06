using Area.Data;
using Area.Data.CustomEntity;
using Area.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Area.Web.Controllers
{
    [VerifyUser]
    public class SPPlaceController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();

        [Route("spplace/{id?}")]
        public ActionResult Index(int? id)
        {
            var result = db.SupervisorVisitPlaces.Where(p => p.VisitPlaceID == id).Include(v => v.VisitPlace).Include(v=>v.Place);
            return View(result);
        }


        [Route("spplace/detail/{id?}")]
        public ActionResult Detail(int? id)
        {
            int userID = Convert.ToInt32(Session["UserId"]);
            var result = db.ProductRecivedDelivereds.Include(x => x.Product.ProductSubCategory.ProductCategory).Where(p => p.IsActive == true && p.SupervisorVisitPlaceID == id).ToList();
            ViewData["supervisorVisitPlace"] = db.SupervisorVisitPlaces.Where(p => p.ID == id).FirstOrDefault();
            ViewData["ProductCategory"] = new SelectList(db.ProductCategories.Where(p => p.IsActive == true), "ID", "Name");
            ViewData["visitPlacePhotoList"] = db.PlacePhotoes.Where(p => p.UserID == userID && p.IsActive == true).ToList();
            return View(result);
        }

        [Route("spplace/deliveryproduct/{id?}")]
        public ActionResult DeliveryProduct(int? id)
        { 
            return View();
        }

        [Route("spplace/inspection/{id?}")]
        public ActionResult Inspection(int? id)
        {
            int userID = Convert.ToInt32(Session["UserId"]);
            var result = db.ProductRecivedDelivereds.Include(x => x.Product.ProductSubCategory.ProductCategory).Where(p => p.IsActive == true && p.SupervisorVisitPlaceID == id).ToList();
            ViewData["supervisorVisitPlace"] = db.SupervisorVisitPlaces.Where(p => p.ID == id).FirstOrDefault(); 
            ViewData["visitPlacePhotoList"] = db.PlacePhotoes.Where(p => p.UserID == userID && p.IsActive == true && p.SupervisorVisitPlaceID == id).ToList();
            return View(result);
        }

        [Route("spplace/inspectioncomment/{id?}")]
        public ActionResult InspectionComment(int? id)
        {
            int userID = Convert.ToInt32(Session["UserId"]); 
            ViewData["supervisorVisitPlace"] = db.SupervisorVisitPlaces.Where(p => p.ID == id).FirstOrDefault();

            var evalVM = new Evaluation(); 
            foreach (var item in db.RaitingQuestions.Where(p => p.IsActive == true))
            {
                evalVM.Questions.Add(new Question()
                {
                    ID = item.ID,
                    QuestionText = item.Name,
                    Answers = new List<Answer>()
                            {
                                new Answer() { ID = 1 , AnswerText = "İyi"},
                                new Answer() { ID = 2 , AnswerText = "Orta"},
                                new Answer() { ID = 3 , AnswerText = "Yetersiz"},
                           }, 
                });
            } 
            return View(evalVM);
        }

        [Route("spplace/placeownercomment/{id?}")]
        public ActionResult PlaceOwnerComment(int? id)
        {
            int userID = Convert.ToInt32(Session["UserId"]);
            ViewData["supervisorVisitPlace"] = db.SupervisorVisitPlaces.Where(p => p.ID == id).FirstOrDefault();

            var evalVM = new Evaluation();
            foreach (var item in db.RaitingQuestions.Where(p => p.IsActive == true))
            {
                evalVM.Questions.Add(new Question()
                {
                    ID = item.ID,
                    QuestionText = item.Name,
                    Answers = new List<Answer>()
                            {
                                new Answer() { ID = 1 , AnswerText = "İyi"},
                                new Answer() { ID = 2 , AnswerText = "Orta"},
                                new Answer() { ID = 3 , AnswerText = "Yetersiz"},
                           },
                });
            }
            return View(evalVM);
        }

        [Route("spplace/inspectioninfo/{id?}")]
        public ActionResult InspectionInfo(int? id)
        { 
            var result = db.SupervisorVisitPlaceInfoes.Where(p => p.SupervisorVisitPlaceID == id).FirstOrDefault();
            return View(result);
        }

        [Route("spplace/penetration/{id?}")]
        public ActionResult Penetration(int? id)
        {
            var result = db.SupervisorVisitPlacePenetrations.Where(p => p.SupervisorVisitPlaceID == id).ToList();
            ViewData["ProductCategory"] = new SelectList(db.ProductCategories.Where(p => p.IsActive == true && p.ID != 2), "ID", "Name");
            return View(result);
        }

        [Route("spplace/GetSubCategories/{categoryid?}")]
        public JsonResult GetSubCategories(string categoryid)
        {
            int catId = string.IsNullOrEmpty(categoryid) ? 0 : Convert.ToInt32(categoryid);
            var list = db.ProductSubCategories.Where(x => x.CategoryID == catId).ToList();
            return Json(new SelectList(list, "ID", "Name"));
        }
        [Route("spplace/GetProducts/{subcategoryid?}")]
        public JsonResult GetProducts(string subcategoryid)
        {
            int subCatId = string.IsNullOrEmpty(subcategoryid) ? 0 : Convert.ToInt32(subcategoryid);
            var list = db.Products.Where(x => x.SubCategoryID == subCatId).ToList();
            return Json(new SelectList(list, "ID", "Name"));
        }

        [Route("spplace/create")]
        [HttpPost]
        public ActionResult Create(ProductRecivedDelivered receivedproduct)
        {
            if (ModelState.IsValid)
            {
                receivedproduct.CreateDate = DateTime.Now;
                receivedproduct.IsActive = true;
                receivedproduct.UserID = Convert.ToInt32(Session["UserId"]);//TODO: Sonradan buraya Session uzerinden userId cek
                db.ProductRecivedDelivereds.Add(receivedproduct);
                db.SaveChanges();
                return RedirectToAction("/Detail/" + receivedproduct.SupervisorVisitPlaceID);
            }
            return View("/Detail/" + receivedproduct.SupervisorVisitPlaceID);
        }

        [Route("spplace/delete/{id}")]
        public ActionResult Delete(int id)
        {
            ProductRecivedDelivered product = db.ProductRecivedDelivereds.Find(id);
            product.IsActive = false;
            db.Entry(product);
            db.SaveChanges();
            return RedirectToAction("/Detail/" + product.SupervisorVisitPlaceID);
        }

        [Route("spplace/checkin")]
        [HttpPost]
        public ActionResult Checkin(Location input)
        {
            if (ModelState.IsValid)
            {
                var visitEntity = db.SupervisorVisitPlaces.Where(p => p.ID == input.ID).FirstOrDefault();
                visitEntity.CheckinDate = DateTime.Now;
                visitEntity.CheckinLatitude = input.Latitude;
                visitEntity.CheckinLongitude = input.Longitude;
                db.Entry(visitEntity);
                db.SaveChanges();
            }
            return RedirectToAction("/Detail/" + input.ID);
        }


        [Route("spplace/{id?}")]
        [HttpPost]
        public ActionResult SaveImages(int? id)
        {
            if (Request.Files.Count > 0)
            {
                using (B2DriveForPostEntities db = new B2DriveForPostEntities())
                {
                    var spPlacesEntity =db.SupervisorVisitPlaces.Find(id);
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
                            pp.SupervisorVisitPlaceID = id.HasValue ? id.Value : 0;
                            pp.VisitPlaceID = spPlacesEntity.VisitPlaceID;
                            db.PlacePhotoes.Add(pp);
                            db.SaveChanges();
                        }
                    }
                }
            } 
            return RedirectToAction("/inspection/" + id);
        }

        [Route("savecomment")]
        [HttpPost]
        public ActionResult SaveComment(Evaluation input)
        {
            db.SupervisorVisitPlaceComments.RemoveRange(db.SupervisorVisitPlaceComments.Where(x => x.SupervisorVisitPlaceID == input.visitplaceID));
            db.SaveChanges();
            foreach (var item in input.Questions)
            {
                SupervisorVisitPlaceComment comm = new SupervisorVisitPlaceComment();
                comm.RaitingID = item.ID;
                comm.RaitingStar = item.SelectedAnswer;
                comm.CreateDate = DateTime.Now;
                comm.SupervisorVisitPlaceID = input.visitplaceID;
                db.SupervisorVisitPlaceComments.Add(comm);
                db.SaveChanges(); 
            }
            var comment = db.SupervisorVisitPlaces.Find(input.visitplaceID);
            comment.PlaceNegativeComment = input.PlaceNegativeComment;
            comment.PlacePositiveComment = input.PlacePositiveComment;
            db.Entry(comment);
            db.SaveChanges(); 
            return Redirect("spplace/placeownercomment/"+input.visitplaceID);
        }

        [Route("saveownercomment")]
        [HttpPost]
        public ActionResult SaveOwnerComment(Evaluation input)
        {
            db.SupervisorVisitPlaceComments.RemoveRange(db.SupervisorVisitPlaceComments.Where(x => x.SupervisorVisitPlaceID == input.visitplaceID && x.PlaceOwner == true));
            db.SaveChanges();
            foreach (var item in input.Questions)
            {
                SupervisorVisitPlaceComment comm = new SupervisorVisitPlaceComment();
                comm.RaitingID = item.ID;
                comm.RaitingStar = item.SelectedAnswer;
                comm.CreateDate = DateTime.Now;
                comm.PlaceOwner = true;
                comm.SupervisorVisitPlaceID = input.visitplaceID;
                db.SupervisorVisitPlaceComments.Add(comm);
                db.SaveChanges();
            }
            var comment = db.SupervisorVisitPlaces.Find(input.visitplaceID);
            comment.PlaceNegativeComment = input.PlaceNegativeComment;
            comment.PlacePositiveComment = input.PlacePositiveComment;
            db.Entry(comment);
            db.SaveChanges();
            return Redirect("spplace/inspectioninfo/" + input.visitplaceID);
        }

        [Route("saveinspectioninfo")]
        [HttpPost]
        public ActionResult SaveInspectionInfo(SupervisorVisitPlaceInfo input)
        {
            if (input.ID > 0)
            {
                var spinfo = db.SupervisorVisitPlaceInfoes.Find(input.ID);
                spinfo.SpvEntryTime = input.SpvEntryTime;
                spinfo.SpvOutTime = input.SpvOutTime;
                spinfo.TotalCoverCount = input.TotalCoverCount;
                spinfo.PointPersonCount = input.PointPersonCount;
                spinfo.PointInformed = input.PointInformed;
                spinfo.TrueSystem = input.TrueSystem;
                db.Entry(spinfo);
                db.SaveChanges();
            }
            else
            {
                input.CreateDate = DateTime.Now;
                input.IsActive = true;
                db.SupervisorVisitPlaceInfoes.Add(input);
                db.SaveChanges();
            }
            
            return Redirect("sphome");
        }


        [Route("savepenetration")]
        [HttpPost]
        public ActionResult SavePenetration(SupervisorVisitPlacePenetration input)
        { 
                input.CreateDate = DateTime.Now;
                input.IsActive = true;
                db.SupervisorVisitPlacePenetrations.Add(input);
                db.SaveChanges();

            return Redirect("spplace/penetration/" + input.SupervisorVisitPlaceID);
        }


        [Route("spplace/editproductNew/{jsonData?}")]
        [HttpPost]
        public ActionResult EditProductNew(ProductRecivedDelivered jsonData)
        {
            var productReceivedEntity = db.ProductRecivedDelivereds.Find(jsonData.ID);
            productReceivedEntity.ProductID = Convert.ToInt32(jsonData.ProductID);
            productReceivedEntity.Quantity = Convert.ToInt32(jsonData.Quantity);
            productReceivedEntity.Note = jsonData.Note;
            db.Entry(productReceivedEntity);
            db.SaveChanges();
            return RedirectToAction("/Detail/" + jsonData.SupervisorVisitPlaceID);
        }

        [HttpGet]
        [Route("spplace/edit/{id}")]
        public ActionResult Edit(int id)
        {
            var product = db.ProductRecivedDelivereds.Find(id);
            TempData["EditProduct"] = product;
            return RedirectToAction("/Detail/" + product.SupervisorVisitPlaceID);
        }

        [Route("spplace/editPenetrationProduct/{jsonData?}")]
        [HttpPost]
        public ActionResult EditPenetrationProduct(SupervisorVisitPlacePenetration jsonData)
        {
            var productReceivedEntity = db.SupervisorVisitPlacePenetrations.Find(jsonData.ID);
            productReceivedEntity.ProductID = Convert.ToInt32(jsonData.ProductID);
            productReceivedEntity.Price = jsonData.Price;
            db.Entry(productReceivedEntity);
            db.SaveChanges();
            return RedirectToAction("/penetration/" + jsonData.SupervisorVisitPlaceID);
        }

        [HttpGet]
        [Route("spplace/editPenetration/{id}")]
        public ActionResult EditPenetration(int id)
        {
            var product = db.SupervisorVisitPlacePenetrations.Find(id);
            TempData["EditProductPenetration"] = product;
            return RedirectToAction("/penetration/" + product.SupervisorVisitPlaceID);
        }

        public class Location
        {
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public int ID { get; set; }
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