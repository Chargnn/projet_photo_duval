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
        public ActionResult Index()
        {
            ViewBag.MessageError = "";

            return View();
        }
        public ActionResult IndexSeancesAgent(string ordreTri, string chaineFiltre, string dateFiltre, string filtreCourantNom, string filtreCourantDate, int? page, string MessageError, int agentID)
        {
            return IndexSeances(ordreTri, chaineFiltre, ref dateFiltre, filtreCourantNom, filtreCourantDate, ref page, agentID, true);
        }
        public ActionResult IndexSeancesPhotographe(string ordreTri, string chaineFiltre, string dateFiltre, string filtreCourantNom, string filtreCourantDate, int? page, string MessageError, int photographeID)
        {
            return IndexSeances(ordreTri, chaineFiltre, ref dateFiltre, filtreCourantNom, filtreCourantDate, ref page, photographeID, false);
        }

        private ActionResult IndexSeances(string ordreTri, string chaineFiltre, ref string dateFiltre, string filtreCourantNom, string filtreCourantDate, ref int? page, int ID, bool agent)
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
            IEnumerable<Seance> seances;
            if (agent)
            {
                seances = unitOfWork.SeanceRepository.Get(includeProperties: "Agent,Photographe", filter: s => s.DateSeance.Year == DateTime.Now.Year && s.Agent_ID == ID);
            }
            else
            {
                seances = unitOfWork.SeanceRepository.Get(includeProperties: "Agent,Photographe", filter: s => s.DateSeance.Year == DateTime.Now.Year && s.Photographe_ID == ID);
            }

            if (!string.IsNullOrEmpty(chaineFiltre))
            {
                if (seances.First().Photographe != null)
                {
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
                    ViewBag.MessageError = "La date entrée n'est pas d'un format valide";
                }
            }

            if (!string.IsNullOrEmpty(chaineFiltre) && !string.IsNullOrEmpty(dateFiltre))
            {
                try
                {
                    DateTime date = DateTime.Parse(dateFiltre);

                    seances = seances.Where(seance => seance.Photographe.Nom.ToLower().Contains(chaineFiltre.ToLower()) ||
                    seance.Photographe.Prenom.ToLower().Contains(chaineFiltre.ToLower()) && seance.DateSeance.Day == date.Day && seance.DateSeance.Month == date.Month);
                }
                catch (Exception e)
                {
                    ViewBag.MessageError = "La date entrée n'est pas d'un format valide";
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

            decimal prix = (decimal)0.00;

            foreach (Seance s in seances)
            {
                if (s.Prix != null)
                    prix += (decimal)s.Prix;
            }
            if (agent)
            {
                ViewBag.Prix = prix;
                return View("IndexSeancesAgent", seances.ToPagedList(pageNo, taillePage));
            }
            else
            {
                return View("IndexSeancesPhotographe", seances.ToPagedList(pageNo, taillePage));
            }
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
            ViewBag.Disponibilites = new SelectList(unitOfWork.DisponibiliteRepository.Get(), "DateDebutDisponibilite", "Disponibilite");

            return View();
        }

        // POST: Seances/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Seance_ID,Photographe_ID,Agent_ID,Adresse,Disponibilite,Ville,Statut")] Seance seance)
        {
            seance.Photographe_ID = null;
            seance.Statut = "demandée";

            if (ModelState.IsValid && seance.DateSeance >= DateTime.Now)
            {
                unitOfWork.SeanceRepository.Insert(seance);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                if (seance.DateSeance > seance.DateFinSeance)
                    ViewBag.MessageError = "La date choisie n'est pas valide. (La date de fin est plus tôt que la date de réservation)";
                else
                    ViewBag.MessageError = "La date choisie n'est pas valide. (Une date plus tard que maintenant)";
            }

            ViewBag.Agent_ID = new SelectList(unitOfWork.AgentRepository.Get(), "Agent_ID", "Nom", seance.Agent_ID);
            //ViewBag.Disponibilites = new SelectList(unitOfWork.DisponibiliteRepository.Get(), "Disponibilites", "DateDebutDisponibilite");

            return View();
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

            ViewBag.Photographe_ID = new SelectList(unitOfWork.PhotographeRepository.Get(), "Photographe_ID", "Prenom", seance.Photographe_ID);

            return View(seance);
        }

        // POST: Seances/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Seance_ID,Agent_ID,Photographe_ID,Adresse,DateSeance,Ville,Statut,DateFinSeance")] Seance seance)
        {
            bool dateBonne = true;

            try
            {
                DateTime date = DateTime.Parse(seance.DateSeance.ToString());

                if (date < DateTime.Now)
                    dateBonne = false;
            }
            catch (Exception e)
            {
                ViewBag.MessageError = "La date entrée n'est pas d'un format valide";
                dateBonne = false;
            }

            if (ModelState.IsValid && dateBonne)
            {
                unitOfWork.SeanceRepository.Update(seance);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            ViewBag.Photographe_ID = new SelectList(unitOfWork.PhotographeRepository.Get(), "Photographe_ID", "Prenom", seance.Photographe_ID);

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

        // Partial view
        public PartialViewResult AvoirSeances(string ordreTri, string chaineFiltre, string dateFiltre, string statutFiltre, string filtreCourantNom, string filtreCourantDate, string filtreCourantStatut, int? page, string MessageError)
        {
            ViewBag.TriDate = string.IsNullOrEmpty(ordreTri) ? "date_desc" : "";
            ViewBag.TriStatut = ordreTri == "statut" ? "statut_desc" : "statut";
            ViewBag.MessageError = "";

            if (chaineFiltre != null || dateFiltre != null || statutFiltre != null)
            {
                page = 1;
            }
            else
            {
                chaineFiltre = filtreCourantNom;
                dateFiltre = filtreCourantDate;
                statutFiltre = filtreCourantStatut;
            }

            ViewBag.triCourant = ordreTri;
            ViewBag.filtreCourantNom = chaineFiltre;
            ViewBag.filtreCourantDate = dateFiltre;
            ViewBag.filtreCourantStatut = statutFiltre;

            var seances = unitOfWork.SeanceRepository.Get(includeProperties: "Agent,Photographe", filter: s => s.DateSeance.Day == DateTime.Now.Day);

            if (!string.IsNullOrEmpty(chaineFiltre))
            {
                var s = seances.Where(seance => seance.Photographe != null);

                seances = s.Where(seance => seance.Photographe.Nom.ToLower().Contains(chaineFiltre.ToLower()) ||
                seance.Photographe.Prenom.ToLower().Contains(chaineFiltre.ToLower()));
            }

            if (!string.IsNullOrEmpty(dateFiltre))
            {
                try
                {
                    DateTime date = DateTime.Parse(dateFiltre);

                    seances = unitOfWork.SeanceRepository.Get(includeProperties: "Agent,Photographe", filter: seance => seance.DateSeance.Day == date.Day && seance.DateSeance.Month == date.Month);
                }
                catch (Exception e)
                {
                    ViewBag.MessageError = "La date entrée n'est pas valide";
                }
            }

            if (!string.IsNullOrEmpty(statutFiltre))
            {
                seances = seances.Where(seance => seance.Statut == statutFiltre);
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

            return PartialView("ListeSeances", seances.ToPagedList(pageNo, taillePage));
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
        public ActionResult AccepterSeance(int idseance, int idphotographe)
        {
            Seance seance = unitOfWork.SeanceRepository.GetByID(idseance);
            seance.Statut = "confirmée";
            unitOfWork.SeanceRepository.Update(seance);
            unitOfWork.Save();

            return View("IndexDemandesSeances", unitOfWork.SeanceRepository.Get(filter: s => s.DateSeance.Year == DateTime.Now.Year && s.Photographe_ID == idphotographe && s.Statut == "demandée").ToPagedList(1,5));
        }

        public ActionResult IndexDemandesSeances(string ordreTri, string chaineFiltre, string dateFiltre, string filtreCourantNom, string filtreCourantDate, int? page, int photographeID)
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
            IEnumerable<Seance> seances;

            seances = unitOfWork.SeanceRepository.Get(includeProperties: "Agent,Photographe", filter: s => s.DateSeance.Year == DateTime.Now.Year && s.Photographe_ID == photographeID && s.Statut == "demandée" && s.DateSeance > DateTime.Today);


            if (!string.IsNullOrEmpty(chaineFiltre))
            {
                if (seances.First().Photographe != null)
                {
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
                    ViewBag.MessageError = "La date entrée n'est pas d'un format valide";
                }
            }

            if (!string.IsNullOrEmpty(chaineFiltre) && !string.IsNullOrEmpty(dateFiltre))
            {
                try
                {
                    DateTime date = DateTime.Parse(dateFiltre);

                    seances = seances.Where(seance => seance.Photographe.Nom.ToLower().Contains(chaineFiltre.ToLower()) ||
                    seance.Photographe.Prenom.ToLower().Contains(chaineFiltre.ToLower()) && seance.DateSeance.Day == date.Day && seance.DateSeance.Month == date.Month);
                }
                catch (Exception e)
                {
                    ViewBag.MessageError = "La date entrée n'est pas d'un format valide";
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

            decimal prix = (decimal)0.00;

            foreach (Seance s in seances)
            {
                if (s.Prix != null)
                    prix += (decimal)s.Prix;
            }

            return View("IndexDemandesSeances", seances.ToPagedList(pageNo, taillePage));

        }
    }
}
