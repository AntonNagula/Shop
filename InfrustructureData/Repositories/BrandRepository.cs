using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfructructureDataInterfaces.Repositories;
using InfructructureDataInterfaces.Models;
using InfrustructureData.Data;
using InfrustructureData.Mappers;
using Microsoft.EntityFrameworkCore;
using InfrustructureData.DataModels;

namespace InfrustructureData.Repositories
{
    public class BrandRepository : IRepositories<RepoBrands>
    {
        private AutoContext db;

        public BrandRepository(AutoContext db)
        {
            this.db = db;
        }
        public void Create(RepoBrands item)
        {
            db.Brands.Add(item.FromRepoBrandsToBrands());
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Brands brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
            db.SaveChanges();
        }        

        public RepoBrands Get(int id)
        {
            if (id != 0)
            {
                return db.Brands.Find(id).FromBrandsToRepoBrands();
            }
            else
            {
                return db.Brands.Last().FromBrandsToRepoBrands();
            }
        }

        public IEnumerable<RepoBrands> GetAll()
        {
            return db.Brands.Select(x => x.FromBrandsToRepoBrands()).ToList();
        }

        public void Update(RepoBrands item)
        {
            throw new NotImplementedException();
        }               

        public IEnumerable<RepoBrands> GetRange(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(List<RepoBrands> items)
        {
            foreach (RepoBrands b in items)
            {
                Brands delete = db.Brands.FirstOrDefault(x => x.Id == b.Id);
                db.Brands.Remove(delete);
            }
            db.SaveChanges();
        }

        public void DeleteWasteEntities()
        {
            throw new NotImplementedException();
        }
    }
}
