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
using projet_photo_duval.DAL;

namespace projet_photo_duval.Controllers
{
    public class SeancesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Seances
        public ActionResult Index(string ordreTri, string chaineFiltre, string dateFiltre, string filtreCourantNom, string filtreCourantDate,  int? page, string MessageError)
        {
            ViewBag.TriDate = string.IsNullOrEmpty(ordreTri) ? "date_desc" : "";
            ViewBag.TriStatut = ordreTri == "statut" ? "statut_desc" : "statut";
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

            var seances = unitOfWork.SeanceRepository.Get(includeProperties: "Agent,Photographe").Where(s => s.DateSeance.Day == DateTime.Now.Day);

            if (!string.IsNullOrEmpty(chaineFiltre))
            {
                if (seances.First().Photographe != null) { 
                    seances = seances.Where(seance => seance.Photographe.Nom.Contains(chaineFiltre) ||
                    seance.Photographe.Prenom.Contains(chaineFiltre));
                }
                else
                {
                    seances = seances.Where(seance => seance.Seance_ID == 0);
                }
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

                    seances = seances.Where(seance => seance.Photographe.Nom.ToLower().Contains(chaineFiltre.ToLower()) ||
                    seance.Photographe.Prenom.ToLower().Contains(chaineFiltre.ToLower()) && seance.DateSeance.Day == date.Day && seance.DateSeance.Month == date.Month);
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

        // GET: Seances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seance seance = unitOfWork.SeanceRepository.GetByID(id);
            if (seance == null)
            {
                return HttpNotFound();
            }
            return View(seance);
        }

        // GET: Seances/Create
        public ActionResult Create()
        {
            ViewBag.MessageError = "";
            ViewBag.Agent_ID = new SelectList(unitOfWork.AgentRepository.Get(), "Agent_ID", "Nom");
            return View();
        }

        // POST: Seances/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Seance_ID,Photographe_ID,Agent_ID,Adresse,DateSeance,Ville,Statut")] Seance seance)
        {
            seance.Photographe_ID = null;
            seance.Statut = "demandée";

            if (ModelState.IsValid && seance.DateSeance >= DateTime.Now)
            {
                unitOfWork.SeanceRepository.Insert(seance);
                unitOfWork.Save();
                return RedirectToAction("Index");
            } else
            {
                //if(seance.DateSeance > seance.DateFinSeance)
                //    ViewBag.MessageError = "La date choisi n'est pas valide. (La date de fin est plus tôt que la date de réservation)";
                //else
                //    ViewBag.MessageError = "La date choisi n'est pas valide. (Une date plus tard qu'aujourd'hui)";
            }

            ViewBag.Agent_ID = new SelectList(unitOfWork.AgentRepository.Get(), "Agent_ID", "Nom", seance.Agent_ID);
            return View(seance);
        }

        // GET: Seances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seance seance = unitOfWork.SeanceRepository.GetByID(id);
            if (seance == null)
            {
                return HttpNotFound();
            }
            ViewBag.Agent_ID = new SelectList(unitOfWork.AgentRepository.Get(), "Agent_ID", "Nom", seance.Agent_ID);
            ViewBag.Photographe_ID = new SelectList(unitOfWork.PhotoRepository.Get(), "Photographe_ID", "Nom", seance.Photographe_ID);
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
                unitOfWork.SeanceRepository.Update(seance);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Agent_ID = new SelectList(unitOfWork.AgentRepository.Get(), "Agent_ID", "Nom", seance.Agent_ID);
            ViewBag.Photographe_ID = new SelectList(unitOfWork.PhotoRepository.Get(), "Photographe_ID", "Nom", seance.Photographe_ID);
            return View(seance);
        }

        // GET: Seances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seance seance = unitOfWork.SeanceRepository.GetByID(id);
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
            Seance seance = unitOfWork.SeanceRepository.GetByID(id);
            unitOfWork.SeanceRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
