using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projet_photo_duval.ViewModels
{
    public class ViewModelDetailsFacture
    {
        [Display(Name = "Nom de l'agent")]
        public string nomAgent { get; set; }
        [Display(Name = "Nom du photographe")]
        public string nomPhotographe { get; set; }
        [Display(Name = "Prix de la facture")]
        public decimal prix { get; set; }
        [Display(Name = "État de paiement de la facture")]
        public int estPayee { get; set; }
        [Display(Name = "Adresse où la séance a eu lieu")]
        public string adresse { get; set; }
        [Display(Name = "Date de la séance")]
        public DateTime date { get; set; }

    }
}