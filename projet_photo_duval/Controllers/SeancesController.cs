using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projet_photo_duval.Models;
using PagedList;

namespace projet_photo_duval.Controllers
{
    public class SeancesController : Controller
    {
        private H18_Proj_Eq07Entities1 db = new H18_Proj_Eq07Entities1();

        // GET: Seances
        public ActionResult Index(string ordreTri, string chaineFiltre, string dateFiltre, string filtreCourantNom, string filtreCourantDate,  int? page, string MessageError)
        {
            ViewBag.TriDate = string.IsNullOrEmpty(ordreTri) ? "date_desc" : "";
            ViewBag.TriStatut = string.IsNullOrEmpty(ordreTri) ? "statut_desc" : "";
            ViewBag.MessageError = "";

            if (chaineFiltre != null || dateFiltre != null)
            {
                page = 1;
            }
            else
            {
                chaineFiltre = filtreCourantNom;
                dateFiltre = filtreCourantDate;
            }

            ViewBag.triCourant = ordreTri;
            ViewBag.filtreCourantNom = chaineFiltre;
            ViewBag.filtreCourantDate = dateFiltre;

            var seances = db.Seance.Include(s => s.Agent).Include(s => s.Photographe);
            //.Where(s => s.DateSeance.Day == DateTime.Now.Day);

            if (!string.IsNullOrEmpty(chaineFiltre))
            {
                seances = seances.Where(seance => seance.Photographe.Nom.Contains(chaineFiltre) ||
                seance.Photographe.Prenom.Contains(chaineFiltre));
            }

            if (!string.IsNullOrEmpty(dateFiltre))
            {
                try
                {
                    DateTime date = DateTime.Parse(dateFiltre);

                    seances = seances.Where(seance => seance.DateSeance.Day == date.Day && seance.DateSeance.Month == date.Month);
                }
                catch (Exception e)
                {
                    ViewBag.MessageError = "La date entrée n'est pas valide";
                }               
            }

            if (!string.IsNullOrEmpty(chaineFiltre) && !string.IsNullOrEmpty(dateFiltre))
            {
                try
                {
                    DateTime date = DateTime.Parse(dateFiltre);

                    seances = seances.Where(seance => seance.Photographe.Nom.Contains(chaineFiltre) ||
                    seance.Photographe.Prenom.Contains(chaineFiltre) && seance.DateSeance.Day == date.Day && seance.DateSeance.Month == date.Month);
                } catch (Exception e)
                {
                    ViewBag.MessageError = "La date entrée n'est pas valide";
                }            
            }

            switch (ordreTri)
            {
                case "date_desc":
                    seances = seances.OrderByDescending(seance => seance.DateSeance);
                    break;
                case "statut_desc":
                    seances = seances.OrderByDescending(seance => seance.Statut);
                    break;
                case "statut":
                    seances = seances.OrderBy(seance => seance.Statut);
                    break;
                default:
                    seances = seances.OrderBy(seance => seance.DateSeance);
                    break;
            }

            int pageNo = page ?? 1;
            int taillePage = 5;

            return View(seances.ToPagedList(pageNo, taillePage));
        }

        /*public ActionResult Index()
        {
            var seance = db.Seance.Include(s => s.Agent).Include(s => s.Photographe);
            return View(seance.ToList());
        }*/

        // GET: Seances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seance seance = db.Seance.Find(id);
            if (seance == null)
            {
                return HttpNotFound();
            }
            return View(seance);
        }

        // GET: Seances/Create
        public ActionResult Create()
        {
            ViewBag.Agent_ID = new SelectList(db.Agent, "Agent_ID", "Nom");
            ViewBag.Photographe_ID = new SelectList(db.Photographe, "Photographe_ID", "Nom");
            return View();
        }

        // POST: Seances/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Photographe_ID,Agent_ID,Adresse,DateSeance,Ville,Statut,DateFinSeance")] Seance seance)
        {
            if (ModelState.IsValid)
            {
                db.Seance.Add(seance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Agent_ID = new SelectList(db.Agent, "Agent_ID", "Nom", seance.Agent_ID);
            ViewBag.Photographe_ID = new SelectList(db.Photographe, "Photographe_ID", "Nom", seance.Photographe_ID);

            return View(seance);
        }

        // GET: Seances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seance seance = db.Seance.Find(id);
            if (seance == null)
            {
                return HttpNotFound();
            }
            ViewBag.Agent_ID = new SelectList(db.Agent, "Agent_ID", "Nom", seance.Agent_ID);
            ViewBag.Photographe_ID = new SelectList(db.Photographe, "Photographe_ID", "Nom", seance.Photographe_ID);
            return View(seance);
        }

        // POST: Seances/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Seance_ID,Agent_ID,Adresse,DateSeance,Ville,Statut,DateFinSeance")] Seance seance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Agent_ID = new SelectList(db.Agent, "Agent_ID", "Nom", seance.Agent_ID);
            ViewBag.Photographe_ID = new SelectList(db.Photographe, "Photographe_ID", "Nom", seance.Photographe_ID);
            return View(seance);
        }

        // GET: Seances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seance seance = db.Seance.Find(id);
            if (seance == null)
            {
                return HttpNotFound();
            }
            return View(seance);
        }

        // POST: Seances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Seance seance = db.Seance.Find(id);
            db.Seance.Remove(seance);
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
