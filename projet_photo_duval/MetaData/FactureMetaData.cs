using projet_photo_duval.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projet_photo_duval.MetaData
{
    [MetadataType(typeof(FactureMetaData))]
    public partial class Facture
    {
        internal sealed class FactureMetaData
        {
            public int Facture_ID { get; set; }

            [Display(Name = "Seance")]
            [Required(ErrorMessage = "Ce champ est obligatoire.")]
            public int Seance_ID { get; set; }

            [DefaultValue(0)]
            [Range(0, 99999.99)]
            public decimal Prix { get; set; }

            [Display(Name ="État du paiement")]
            [Range(0,1)]
            public bool EstPayee { get; set; }

            [Display(Name = "Date de la facturation")]
            public DateTime DateFacturation { get; set; }
        }
    }
}