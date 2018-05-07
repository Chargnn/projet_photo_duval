using projet_photo_duval.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projet_photo_duval.MetaData
{
    [MetadataType(typeof(PhotoMetaData))]
    public partial class Photo
    {
        internal sealed class PhotoMetaData
        {
            //[Required(ErrorMessage ="Ce champ est obligatoire")]
            [MinLength(5,ErrorMessage = "Le commentaire doit comprendre entre 5 et 250 caractères")]
            [MaxLength(250,ErrorMessage = "Le commentaire doit comprendre entre 5 et 250 caractères")]
            public string Commentaire { get; set; }
        }
    }
}