using InfructructureDataInterfaces.Models;
using InfructructureDataInterfaces.Repositories;
using InfrustructureData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrustructureData.Data
{
    public class UnitOfWork :IUnitOfWork,IDisposable
    {
        private AutoContext db;
        private CarRepository dbCars;
        private BuyerRepository dbBuyers;
        private PurchaseRepository dbBuyCars;
        private BrandRepository dbBrands;
        private SpeachRepository dbSpeaches;
        private MessageRepository dbMessages;
        public UnitOfWork(AutoContext st)
        {
            db = st;
        }

        public IRepositories<RepoMessage> Messages
        {
            get
            {
                if (dbMessages == null)
                    dbMessages = new MessageRepository(db);
                return dbMessages;
            }
        }

        public IRepositories<RepoSpeach> Speaches
        {
            get
            {
                if (dbSpeaches == null)
                    dbSpeaches = new SpeachRepository(db);
                return dbSpeaches;
            }
        }
        public IRepositories<RepoBrands> Brands
        {
            get
            {
                if (dbBrands == null)
                    dbBrands = new BrandRepository(db);
                return dbBrands;
            }
        }

        public IRepositories<RepoCar> Cars
        {
            get
            {
                if (dbCars == null)
                    dbCars = new CarRepository(db);
                return dbCars;
            }
        }
        public IRepositories<RepoBuyer> Buyers
        {
            get
            {
                if (dbBuyers == null)
                    dbBuyers = new BuyerRepository(db);
                return dbBuyers;
            }
        }

        public PurchaseRepositories<RepoBuyCar> BuyCars
        {
            get
            {
                if (dbBuyCars == null)
                    dbBuyCars = new PurchaseRepository(db);
                return dbBuyCars;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
