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
    public class PhotographesController : Controller
    {
        private H18_Proj_Eq07Entities1 db = new H18_Proj_Eq07Entities1();

        // GET: Photographes
        public ActionResult Index()
        {
            return View(db.Photographe.ToList());
        }

        // GET: Photographes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photographe photographe = db.Photographe.Find(id);
            if (photographe == null)
            {
                return HttpNotFound();
            }
            return View(photographe);
        }

        // GET: Photographes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photographes/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Photographe_ID,Nom,Prenom,Telephone,Courriel")] Photographe photographe)
        {
            if (ModelState.IsValid)
            {
                db.Photographe.Add(photographe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(photographe);
        }

        // GET: Photographes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photographe photographe = db.Photographe.Find(id);
            if (photographe == null)
            {
                return HttpNotFound();
            }
            return View(photographe);
        }

        // POST: Photographes/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Photographe_ID,Nom,Prenom,Telephone,Courriel")] Photographe photographe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photographe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photographe);
        }

        // GET: Photographes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photographe photographe = db.Photographe.Find(id);
            if (photographe == null)
            {
                return HttpNotFound();
            }
            return View(photographe);
        }

        // POST: Photographes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photographe photographe = db.Photographe.Find(id);
            db.Photographe.Remove(photographe);
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
