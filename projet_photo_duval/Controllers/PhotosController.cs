using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projet_photo_duval.Models;

namespace projet_photo_duval.Controllers
{
    public class PhotosController : Controller
    {
        private H18_Proj_Eq07Entities1 db = new H18_Proj_Eq07Entities1();



        public ActionResult AddImage()
        {
            Photo photo = new Photo();
            return View(photo);
        }
        [HttpPost]
        public ActionResult AddImage(Photo model, HttpPostedFileBase image1)
        {
            if (image1 != null)
            {
                model.Photo1 = new byte[image1.ContentLength];
                image1.InputStream.Read(model.Photo1, 0, image1.ContentLength);
            }
            db.Photo.Add(model);
            db.SaveChanges();
            return View(model);
        }


        // GET: Photos
        public ActionResult Index()
        {
            var photo = db.Photo.Include(p => p.Seance);
            return View(photo.ToList());
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            ViewBag.Seance_ID = new SelectList(db.Seance, "Seance_ID", "Adresse");
            return View();
        }

        // POST: Photos/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Photo_ID,Photo1,Seance_ID")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Photo.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Seance_ID = new SelectList(db.Seance, "Seance_ID", "Adresse", photo.Seance_ID);
            return View(photo);
        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Seance_ID = new SelectList(db.Seance, "Seance_ID", "Adresse", photo.Seance_ID);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Photo_ID,Photo1,Seance_ID")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Seance_ID = new SelectList(db.Seance, "Seance_ID", "Adresse", photo.Seance_ID);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photo.Find(id);
            db.Photo.Remove(photo);
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
