using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projet_photo_duval.Models;
using Ionic.Zip;
using System.IO;

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
        //[HttpPost]
        //public ActionResult AddImage(Photo model, HttpPostedFileBase image1)
        //{
        //    if (image1 != null)
        //    {
        //        model.Photo1 = new byte[image1.ContentLength];
        //        model.Seance_ID = 1002;
        //        image1.InputStream.Read(model.Photo1, 0, image1.ContentLength);
        //    }
        //    db.Photo.Add(model);
        //    db.SaveChanges();

        //    return View(model);
        //}
        [HttpPost]
        public ActionResult AddImage(int id,IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.MessageError = null;
            ViewBag.Message = null;
            Boolean uploadValide = true;
            int cpt = 0;
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                    if (!fileExtension.Equals(".jpg") ||
                        fileExtension.Equals(".png") ||
                        fileExtension.Equals(".jpeg"))
                    {
                        ViewBag.MessageError = "Le fichier " + file.FileName + " n'est pas une image";
                        uploadValide = false;
                    }
                    else
                    {
                        Photo photo = new Photo();
                        photo.Photo1 = new byte[file.ContentLength];
                        photo.Seance_ID = id;

                        file.InputStream.Read(photo.Photo1, 0, file.ContentLength);
                        db.Photo.Add(photo);
                    }
                    cpt++;
                } 
            }
            if (cpt.Equals(0))
            {
                ViewBag.MessageError = "Aucune image n'a été sélectionnée";
                uploadValide = false;
            }
            else if (uploadValide)
            {
                db.SaveChanges();
                ViewBag.Message = "Les photos ont été correctemment téléversées";
            }
            return View();
        }


        private List<Photo> GetPhotoList() {
            List<Photo> lstPhoto = new List<Photo>();
            var query = db.Photo;
            return query.ToList();
        }
        [HttpGet]
        public void DownLoadPhoto(int id)
        {
            List<Photo> ObjPhotos = GetPhotoList();
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Photos");

                var PhotoBySeanceId = (from p in ObjPhotos
                                       where p.Seance_ID.Equals(id)
                                       select new { p.Nom, p.Photo1 }).ToList();
                string contentType = "image/jpg";
                int NB = 0;
                foreach (var p in PhotoBySeanceId)
                {
                    NB++;
                    zip.AddEntry("Photos/image"+NB+".jpg",p.Photo1);
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
            //    var PhotoBySeanceId = (from p in ObjPhotos
            //                           where p.Seance_ID.Equals(id)
            //                           select new { p.Nom, p.Photo1 }).ToList().FirstOrDefault();

            //string contentType = "image/jpg";
            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName ="test.jpeg",
            //    Inline = false
            //};
            //Response.AppendHeader("Content-Disposition", cd.ToString());
            //return File(PhotoBySeanceId.Photo1, contentType, PhotoBySeanceId.Nom);

        }
        [HttpGet]
        public PartialViewResult PhotosDetails()
        {
            List<Photo> PhotoList = GetPhotoList();

            return PartialView("PhotoDetails", PhotoList);

        }

        public ActionResult ShowImage()
        {
            var photo = db.Photo;
            return View(photo.ToList());
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
