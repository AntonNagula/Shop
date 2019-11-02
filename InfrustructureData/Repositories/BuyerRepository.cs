using InfructructureDataInterfaces.Models;
using InfructructureDataInterfaces.Repositories;
using InfrustructureData.Data;
using InfrustructureData.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using InfrustructureData.Mappers;
using Microsoft.EntityFrameworkCore;

namespace InfrustructureData.Repositories
{
    public class BuyerRepository : IRepositories<RepoBuyer>
    {
        private AutoContext db;

        public BuyerRepository(AutoContext db)
        {
            this.db = db;
        }
        public void Create(RepoBuyer item)
        {
            db.Buyers.Add(item.FromRepoBuyerToBuyer());
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Buyer buyer = db.Buyers.Find(id);
            List<BuyCar> b= db.BuyCars.Where(x=>x.BuyerId==id).ToList();
            if(b!=null)
                db.BuyCars.RemoveRange(b);
            db.Buyers.Remove(buyer);            
            db.SaveChanges();
        }       

        public RepoBuyer Get(int id)
        {            
            return db.Buyers.Find(id).FromBuyerToRepoBuyer();
        }

        public IEnumerable<RepoBuyer> GetAll()
        {
            return db.Buyers.Select(x=>x.FromBuyerToRepoBuyer()).ToList();
        }

        public void Update(RepoBuyer item)
        {
            int id = item.Id;
            Buyer b=db.Buyers.Find(id);
            b.TelegramID = item.TelegramID;
            b.Email = item.Email;
            b.Telephone = item.Telephone;
            db.Entry(b).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IEnumerable<RepoBuyer> GetRange(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(List<RepoBuyer> items)
        {            
            foreach (RepoBuyer b in items)
            {
                Buyer delete = db.Buyers.FirstOrDefault(x => x.Id == b.Id);
                db.Buyers.Remove(delete);
            }
            db.SaveChanges();
        }

        public void DeleteWasteEntities()
        {
            throw new NotImplementedException();
        }
    }
}
