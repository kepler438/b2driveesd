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
    [VerifyUser]
    public class ProductSubCategoriesController : Controller
    {
        private B2DriveForPostEntities db = new B2DriveForPostEntities();

        // GET: ProductSubCategories
        public ActionResult Index()
        {
            var productSubCategories = db.ProductSubCategories.Include(p => p.ProductCategory);
            return View(productSubCategories.ToList());
        }

        // GET: ProductSubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSubCategory productSubCategory = db.ProductSubCategories.Find(id);
            if (productSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(productSubCategory);
        }

        // GET: ProductSubCategories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "ID", "Name");
            return View();
        }

        // POST: ProductSubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,CategoryID,Name,CreateDate,IsActive")] ProductSubCategory productSubCategory)
        {
            if (ModelState.IsValid)
            {
                productSubCategory.CreateDate = DateTime.Now;
                db.ProductSubCategories.Add(productSubCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.ProductCategories, "ID", "Name", productSubCategory.CategoryID);
            return View(productSubCategory);
        }

        // GET: ProductSubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSubCategory productSubCategory = db.ProductSubCategories.Find(id);
            if (productSubCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "ID", "Name", productSubCategory.CategoryID);
            return View(productSubCategory);
        }

        // POST: ProductSubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,CategoryID,Name,CreateDate,IsActive")] ProductSubCategory productSubCategory)
        {
            if (ModelState.IsValid)
            {
                productSubCategory.CreateDate = DateTime.Now;
                db.Entry(productSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "ID", "Name", productSubCategory.CategoryID);
            return View(productSubCategory);
        }

        // GET: ProductSubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSubCategory productSubCategory = db.ProductSubCategories.Find(id);
            if (productSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(productSubCategory);
        }

        // POST: ProductSubCategories/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSubCategory productSubCategory = db.ProductSubCategories.Find(id);
            db.ProductSubCategories.Remove(productSubCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
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
