using projet_photo_duval.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projet_photo_duval.MetaData
{
    [MetadataType(typeof(SeanceMetaData))]
    public partial class Seance
    {
        internal sealed class SeanceMetaData
        {
            public int Seance_ID { get; set; }
            public int? Photographe_ID { get; set; }
            public int Agent_ID { get; set; }

            [Required(ErrorMessage = "L'adresse est requise")]
            public string Adresse { get; set; }

            [Required(ErrorMessage = "La date est requise")]
            public DateTime DateSeance { get; set; }

            [Required(ErrorMessage = "La ville est requise")]
            public string Ville { get; set; }
            public string Statut { get; set; }
            public DateTime? DateFinSeance { get; set; }
        }
    }
}