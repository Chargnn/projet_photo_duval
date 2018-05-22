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
    public class DisponibilitesController : Controller
    {
        private H18_Proj_Eq07Entities1 db = new H18_Proj_Eq07Entities1();

        // GET: Disponibilites
        public ActionResult Index()
        {
            var disponibilite = db.Disponibilite.Include(d => d.Photographe);
            return View(disponibilite.ToList());
        }

        // GET: Disponibilites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponibilite disponibilite = db.Disponibilite.Find(id);
            if (disponibilite == null)
            {
                return HttpNotFound();
            }
            return View(disponibilite);
        }

        // GET: Disponibilites/Create
        public ActionResult Create(int photographeID)
        {
            ViewBag.Test = photographeID;
            ViewBag.Photographe_ID = new SelectList(db.Photographe.Where(p=>p.Photographe_ID==photographeID), "Photographe_ID", "Photographe_ID");
            return View();
        }

        // POST: Disponibilites/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Disponibilite_ID,Photographe_ID,DateDebutDisponibilite,DateFinDisponibilite")] Disponibilite disponibilite)
        {
            if (TryValidateModel(disponibilite))// ModelState.IsValid)


            {
                db.Disponibilite.Add(disponibilite);
                db.SaveChanges();
                return RedirectToAction("Create", new { photographeID = disponibilite.Photographe_ID });
            }
            ViewBag.Photographe_ID = new SelectList(db.Photographe, "Photographe_ID", "Nom", disponibilite.Photographe_ID);
            //return View(disponibilite);
            return RedirectToAction("Create", new { photographeID = disponibilite.Photographe_ID });
        }

        // GET: Disponibilites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponibilite disponibilite = db.Disponibilite.Find(id);
            if (disponibilite == null)
            {
                return HttpNotFound();
            }
            ViewBag.Photographe_ID = new SelectList(db.Photographe, "Photographe_ID", "Nom", disponibilite.Photographe_ID);
            return View(disponibilite);
        }

        // POST: Disponibilites/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public ActionResult Edit([Bind(Include = "Disponibilite_ID,Photographe_ID,DateDebutDisponibilite,DateFinDisponibilite")] Disponibilite disponibilite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disponibilite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Photographe_ID = new SelectList(db.Photographe, "Photographe_ID", "Nom", disponibilite.Photographe_ID);
            return View(disponibilite);
        }

        // GET: Disponibilites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponibilite disponibilite = db.Disponibilite.Find(id);
            if (disponibilite == null)
            {
                return HttpNotFound();
            }
            return View(disponibilite);
        }

        // POST: Disponibilites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Disponibilite disponibilite = db.Disponibilite.Find(id);
            db.Disponibilite.Remove(disponibilite);
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
        public ActionResult AddDisponibilite(int photographeID)
        {
            ViewBag.Test = photographeID;
            return View();
        }
    }
}
