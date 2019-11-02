using System.Collections.Generic;
using System.Linq;
using InfructructureDataInterfaces.Repositories;
using InfructructureDataInterfaces.Models;
using InfrustructureData.Data;
using InfrustructureData.Mappers;
using Microsoft.EntityFrameworkCore;
using InfrustructureData.DataModels;
using System;

namespace InfrustructureData.Repositories
{
    public class CarRepository : IRepositories<RepoCar>
    {
        private AutoContext db;

        public CarRepository(AutoContext db)
        {
            this.db = db;
        }
        public void Create(RepoCar item)
        {
            db.Cars.Add(item.FromRepoCarToCar());
            db.SaveChanges();
        }

        public void Delete(int id)
        {            
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();            
        }

        public void DeleteWasteEntities()
        {
            List<BuyCar> buycars = db.BuyCars.Include(x => x.Car).Where(x => x.Car.OwnerId == null || x.Car.Status != "Продается").ToList();
            db.BuyCars.RemoveRange(buycars);
            db.SaveChanges();
            List<Car> cars = db.Cars.Where(x => x.OwnerId == null || x.Status != "Продается").ToList();
            db.Cars.RemoveRange(cars);
            db.SaveChanges();
        }

        public void DeleteRange(List<RepoCar> items)
        {            
            foreach (RepoCar b in items)
            {
                Car delete = db.Cars.FirstOrDefault(x => x.Id == b.Id);
                db.Cars.Remove(delete);
            }
            db.SaveChanges();
        }

        public RepoCar Get(int id)
        {
            if (id != 0)
            {
                return db.Cars.Find(id).FromCarToRepoCar();
            }
            else
            {
                return db.Cars.Last().FromCarToRepoCar();
            }
        }

        public IEnumerable<RepoCar> GetAll()
        {
            return db.Cars.Select(x=>x.FromCarToRepoCar()).ToList();
        }

        public IEnumerable<RepoCar> GetRange(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(RepoCar item)
        {            
            Car car=db.Cars.Find(item.Id);         
            car.CarBrand = item.CarBrand;
            car.Name = item.Name;
            car.Price = item.Price;
            car.Info = item.Info;
            car.Status = item.Status;
            car.ExtencionName = item.ExtencionName;
            car.OwnerId = item.OwnerId;
            db.Entry(car).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
