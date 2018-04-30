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
    public class AgentsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Agents
        public ActionResult Index(string ordreTri,string chaineFiltre, string filtreCourantNom, int? page, string MessageError)
        {
            ViewBag.TriNom = string.IsNullOrEmpty(ordreTri) ? "nom_desc" : "";
            ViewBag.TriPartenaire = ordreTri == "part" ? "part_desc" : "part";
            ViewBag.MessageError = "";

            if (chaineFiltre != null)
            {
                page = 1;
            }
            else
            {
                chaineFiltre = filtreCourantNom;
            }

            ViewBag.triCourant = ordreTri;
            ViewBag.filtreCourantNom = chaineFiltre;

            var agent = unitOfWork.AgentRepository.Get();

            if (!string.IsNullOrEmpty(chaineFiltre))
            {
                try
                {
                    agent = agent.Where(x => x.Nom.ToLower().StartsWith(chaineFiltre.ToLower()));
                }
                catch (Exception e)
                {
                    ViewBag.MessageError = "La valeur entrée n'est pas valide";
                }
            }

            switch (ordreTri)
            {
                case "nom_desc":
                    agent = agent.OrderByDescending(x => x.Nom);
                    break;
                case "part_desc":
                    agent = agent.OrderByDescending(x => x.EstPartenaire);
                    break;
                case "part":
                    agent = agent.OrderBy(x => x.EstPartenaire);
                    break;
                default:
                    agent = agent.OrderBy(x => x.Nom);
                    break;
            }

            int pageNo = page ?? 1;
            int taillePage = 5;

            return View(agent.ToPagedList(pageNo, taillePage));
            //return View(db.Agent.ToList());

        }


        // GET: Agents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = unitOfWork.AgentRepository.GetByID(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // GET: Agents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agents/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Agent_ID,Nom,Prenom,Telephone,Courriel,EstPartenaire")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.AgentRepository.Insert(agent);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(agent);
        }

        // GET: Agents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = unitOfWork.AgentRepository.GetByID(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // POST: Agents/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Agent_ID,Nom,Prenom,Telephone,Courriel,EstPartenaire")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.AgentRepository.Update(agent);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(agent);
        }

        // GET: Agents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = unitOfWork.AgentRepository.GetByID(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agent agent = unitOfWork.AgentRepository.GetByID(id);
            unitOfWork.AgentRepository.Delete(id);
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
