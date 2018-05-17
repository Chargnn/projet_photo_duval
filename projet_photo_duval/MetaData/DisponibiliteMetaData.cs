using projet_photo_duval.Models;
using projet_photo_duval.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projet_photo_duval.MetaData
{
    [MetadataType(typeof(DisponibiliteMetaData))]
    public partial class Disponibilite
    {
        internal sealed class DisponibiliteMetaData
        {
            public int Disponibilite_ID { get; set; }

            public int Photographe_ID { get; set; }

            [Required(ErrorMessage = "La date de début de la disponibilité est requise")]
            [Display(Name ="Début de la disponibilité")]
            [DateAnt(ErrorMessage = "La date ne peut pas être dans le passé")]
      
            public DateTime DateDebutDisponibilite { get; set; }

            [Required(ErrorMessage = "La date de fin de la disponibilité est requise")]
            [Display(Name = "Fin de la disponibilité")]
            [DateGreaterThan("DateDebutDisponibilite", ErrorMessage = "La date de fin doit être supèrieure à la date de début!")]

            public DateTime? DateFinDisponibilite { get; set; }

        }
    }
}