using System;
using projet_photo_duval.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;

namespace projet_photo_duval.DAL
{
    public class UnitOfWork : IDisposable
    {
        private H18_Proj_Eq07Entities1 context = new H18_Proj_Eq07Entities1();
        private GenericRepository<Photo> photoRepository;
        private GenericRepository<Seance> seanceRepository;
        private GenericRepository<Facture> factureRepository;
        private GenericRepository<Agent> agentRepository;
        private GenericRepository<Disponibilite> disponibiliteRepository;
        private GenericRepository<Photographe> photographeRepository;

        public GenericRepository<Photographe> PhotographeRepository
        {
            get
            {

                if (this.photographeRepository == null)
                {
                    this.photographeRepository = new GenericRepository<Photographe>(context);
                }
                return photographeRepository;
            }
        }

        public GenericRepository<Disponibilite> DisponibiliteRepository
        {
            get
            {

                if (this.disponibiliteRepository == null)
                {
                    this.disponibiliteRepository = new GenericRepository<Disponibilite>(context);
                }
                return disponibiliteRepository;
            }
        }

        public GenericRepository<Photo> PhotoRepository
        {
            get
            {

                if (this.photoRepository == null)
                {
                    this.photoRepository = new GenericRepository<Photo>(context);
                }
                return photoRepository;
            }
        }

        public GenericRepository<Seance> SeanceRepository
        {
            get
            {

                if (this.seanceRepository == null)
                {
                    this.seanceRepository = new GenericRepository<Seance>(context);
                }
                return seanceRepository;
            }
        }

        public GenericRepository<Facture> FactureRepository
        {
            get
            {

                if (this.factureRepository == null)
                {
                    this.factureRepository = new GenericRepository<Facture>(context);
                }
                return factureRepository;
            }
        }

        public GenericRepository<Agent> AgentRepository
        {
            get
            {

                if (this.agentRepository == null)
                {
                    this.agentRepository = new GenericRepository<Agent>(context);
                }
                return agentRepository;
            }
        }

        public void Save()
        {
            //context.SaveChanges();
            SaveChanges(context);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}