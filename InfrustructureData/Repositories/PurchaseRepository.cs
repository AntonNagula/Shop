using InfructructureDataInterfaces.Models;
using InfructructureDataInterfaces.Repositories;
using InfrustructureData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using InfrustructureData.Mappers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InfrustructureData.DataModels;

namespace InfrustructureData.Repositories
{
    public class PurchaseRepository : PurchaseRepositories<RepoBuyCar>
    {
        private AutoContext db;

        public PurchaseRepository(AutoContext db)
        {
            this.db = db;
        }
        public void Create(RepoBuyCar item)
        {
            db.BuyCars.Add(item.FromRepoBuyCarToBuyCar());
            db.SaveChanges();
        }

        public IEnumerable<RepoBuyCar> GetAll()
        {
            return db.BuyCars.Include(x=>x.Car).Include(x=>x.Buyer).Select(x => x.FromBuyCarToRepoBuyCar()).ToList();
        }

        public void Update(RepoBuyCar item)
        {
            throw new NotImplementedException();
        }        

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public RepoBuyCar Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(RepoBuyCar id)
        {
            BuyCar obj=db.BuyCars.FirstOrDefault(x=>x.CarId==id.CarId && x.BuyerId==id.BuyerId);
            db.BuyCars.Remove(obj);
            db.SaveChanges();
        }

        public void DeleteRange(List<RepoBuyCar> items)
        {
            foreach (RepoBuyCar b in items)
            {
                BuyCar delete=db.BuyCars.FirstOrDefault(x=>x.CarId==b.CarId && x.BuyerId==b.BuyerId);
                db.BuyCars.Remove(delete);
            }
            db.SaveChanges();
        }
    }
}
