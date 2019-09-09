﻿using InfructructureDataInterfaces.Models;
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
            db.Buyers.Remove(buyer);
            db.SaveChanges();
        }

        public RepoBuyer Get(int id)
        {
            RepoBuyer obj = db.Buyers.Find(id).FromBuyerToRepoBuyer();
           
            return db.Buyers.Find(id).FromBuyerToRepoBuyer();
        }

        public IEnumerable<RepoBuyer> GetAll()
        {
            return db.Buyers.Select(x=>x.FromBuyerToRepoBuyer()).ToList();
        }

        public void Update(RepoBuyer item)
        {
            throw new NotImplementedException();
        }
    }
}
