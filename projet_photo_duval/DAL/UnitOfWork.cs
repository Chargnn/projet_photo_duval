using System;
using projet_photo_duval.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace projet_photo_duval.DAL
{
    public class UnitOfWork : IDisposable
    {
        private H18_Proj_Eq07Entities1 context = new H18_Proj_Eq07Entities1();
        private GenericRepository<Photo> photoRepository;
        private GenericRepository<Seance> seanceRepository;
        private GenericRepository<Facture> factureRepository;
        private GenericRepository<Agent> agentRepository;

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
            context.SaveChanges();
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
    }
}