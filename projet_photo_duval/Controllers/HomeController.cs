using projet_photo_duval.DAL;
using projet_photo_duval.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using projet_photo_duval.ViewModels;

namespace projet_photo_duval.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetEvents()
        {
            using (H18_Proj_Eq07Entities1 dc = new H18_Proj_Eq07Entities1())
            {
                List<ViewModelDispo> events = dc.Disponibilite.Select(x => new ViewModelDispo { dispoID = x.Disponibilite_ID, DateDebutDisponibilite = x.DateDebutDisponibilite, DateFinDisponibilite = x.DateFinDisponibilite }).ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Disponibilite e)
        {
            var status = false;
            using (H18_Proj_Eq07Entities1 dc = new H18_Proj_Eq07Entities1())
            {
                if (e.Disponibilite_ID > 0)
                {
                    //Update the event
                    var v = dc.Disponibilite.Where(a => a.Disponibilite_ID == e.Disponibilite_ID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Photographe_ID = e.Photographe_ID;
                        v.DateDebutDisponibilite = e.DateDebutDisponibilite;
                        v.DateFinDisponibilite = e.DateFinDisponibilite;
                    }
                }
                else
                {
                    dc.Disponibilite.Add(e);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int disponibiliteID)
        {
            var status = false;
            using (H18_Proj_Eq07Entities1 dc = new H18_Proj_Eq07Entities1())
            {
                var v = dc.Disponibilite.Where(a => a.Disponibilite_ID == disponibiliteID).FirstOrDefault();
                if (v != null)
                {
                    dc.Disponibilite.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}