﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace projet_photo_duval.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class H18_Proj_Eq07Entities1 : DbContext
    {
        public H18_Proj_Eq07Entities1()
            : base("name=H18_Proj_Eq07Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Agent> Agent { get; set; }
        public virtual DbSet<Facture> Facture { get; set; }
        public virtual DbSet<Photographe> Photographe { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Seance> Seance { get; set; }
    }
}
