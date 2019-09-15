using DomainCore.Interfaces;
using DomainCore.Models;
using InfructructureDataInterfaces.Repositories;
using DomainService.Mappers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DomainService.Service
{
    public class Service : IService
    {        
        public readonly IUnitOfWork _Repositories;

        public void CreateBrand(string brandName)
        {
            DomainBrands brand = new DomainBrands {  BrandName=brandName};
            _Repositories.Brands.Create(brand.FromDomainBrandToRepoBrand());
        }

        List<DomainBrands> Brands;
        List<DomainCar> Cars;
        List<DomainBuyCar> BuyCars;
        List<DomainBuyer> Buyers;

        public Service(IUnitOfWork Repository)
        {
            _Repositories = Repository;
            Cars = Repository.Cars.GetAll().Select(x=>x.FromRepoCarToDomainCar()).ToList();
            BuyCars = Repository.BuyCars.GetAll().Select(x => x.FromRepoBuyCarToDomainBuyCar()).ToList();
            Buyers = Repository.Buyers.GetAll().Select(x => x.FromRepoBuyerToDomainBuyer()).ToList();
            Brands = Repository.Brands.GetAll().Select(x => x.FromRepoBrandToDomainBrand()).ToList();
        }       
        //
        public IEnumerable<DomainBrands> GetAllBrands()
        {
           
            return Brands;
        }
        //
        public IEnumerable<DomainCar> GetAllCars(int i,int  size,out int Total)
        {            
            List<DomainCar> cars = Cars.ToList();
            Total = cars.Count;
            if (i * size > Total)
                i = Total / size+1;
            List<DomainCar> result = Cars.Skip((i - 1) * size).Take(size).ToList();
            return result;
        }
        //
        //
        public IEnumerable<DomainCar> GetAllCars(int i, int size, string brand,out int Total)
        {
            List<DomainCar> currentcars;
            if (brand != "Все")
                currentcars = Cars.Where(x => x.CarBrand == brand && x.Status== "Продается").ToList();
            else
                currentcars = Cars.Where(x=>x.Status== "Продается").ToList();
            Total = currentcars.Count;
            if (Total < size)
                size = Total;
            else if (i * size > Total)
                i = Total / size + 1;
            List<DomainCar> result = currentcars.Skip((i - 1) * size).Take(size).ToList();
            return result;
        }

        //
        public IEnumerable<DomainCar> GetAllCars()
        {
            return Cars;
        }

        //
        public IEnumerable<DomainCar> GetCarsBuyerId(int id)
        {            
            List<DomainCar> domainCars = new List<DomainCar>();
            foreach(DomainBuyCar c in BuyCars)
            {
                if(c.BuyerId==id)
                {
                    domainCars.Add(c.Car);
                }
            }
            return domainCars;
        }
        //
        //
        public IEnumerable<DomainCar> GetCarsByOwnerId(int id)
        {            
            return Cars.Where(x => x.OwnerId == id).ToList();
        }
               

        //
        public DomainCar GetCar(int id)
        {
            return Cars.FirstOrDefault(x=>x.Id==id);
        }
        //
        //
        public void Create_Car(DomainCar item)
        {
            if (item.BrandId == 0)
            {                
                DomainBrands brand = new DomainBrands { BrandName = item.CarBrand };                
                _Repositories.Brands.Create(brand.FromDomainBrandToRepoBrand());
                Brands.Add(_Repositories.Brands.Get(0).FromRepoBrandToDomainBrand());
                item.BrandId = Brands.Last().Id;
            }
            else
            {
                item.CarBrand = Brands.FirstOrDefault(x => x.Id == item.BrandId).BrandName;
            }           
           
            _Repositories.Cars.Create(item.FromDomainCarToRepoCar());
            Cars.Add(_Repositories.Cars.Get(0).FromRepoCarToDomainCar());
        }
        

        public void Update_Car(DomainCar item)
        {
            if (item.BrandId == 0)
            {                
                int BrandId = Brands.Find(x=>x.BrandName==item.CarBrand).Id;
                item.BrandId = BrandId;
            }
            _Repositories.Cars.Update(item.FromDomainCarToRepoCar());
            DomainCar current = Cars.Find(x=>x.Id==item.Id);
            int index = Cars.IndexOf(current);
            Cars[index] = item;
        }
        public void Delete_Car(int id)
        {           
            DomainCar car=Cars.Find(x=>x.Id==id);
            int i = Cars.IndexOf(car);
            car.Status = "Больше не продается";
            car.OwnerId = null;
            Cars[i] = car;            
            _Repositories.Cars.Update(car.FromDomainCarToRepoCar());
        }

        public void AdminDelete(int id)
        {
            DomainCar car = Cars.Find(x => x.Id == id);
            int i = Cars.IndexOf(car);
            car.Status = "Запрет админа";
            Cars[i] = car;
            _Repositories.Cars.Update(car.FromDomainCarToRepoCar());
        }

        public void DeleteRangeCars()
        {
            _Repositories.Cars.DeleteRange();
            Cars.Clear();
            Cars = _Repositories.Cars.GetAll().Select(x=>x.FromRepoCarToDomainCar()).ToList();
            BuyCars.Clear();
            BuyCars = _Repositories.BuyCars.GetAll().Select(x => x.FromRepoBuyCarToDomainBuyCar()).ToList();
        }
                
        //
        public bool Buy(DomainBuyCar item)
        {          
            DomainCar car=Cars.FirstOrDefault(x=>x.Id==item.CarId);
            item.Car = car;
            item.Buyer = Buyers.FirstOrDefault(x=>x.Id==item.BuyerId);
            DomainBuyCar obj = BuyCars.FirstOrDefault(x=>x.CarId==item.CarId && x.BuyerId==item.BuyerId);
            if (obj==null)
            {                
                _Repositories.BuyCars.Create(item.FromDomainBuyCarToRepoBuyCar());
                BuyCars.Add(item);
                return true;
            }

            return false;
        }

        


        //
        public IEnumerable<DomainBuyer> GetAllBuyers()
        {
            return Buyers;
        }

        //       
        public IEnumerable<DomainBuyer> GetBuyersByCarId(int id)
        {
            List<DomainBuyer> domainBuyers = new List<DomainBuyer>();
            foreach (DomainBuyCar c in BuyCars)
            {
                if (c.CarId == id)
                {
                    domainBuyers.Add(c.Buyer);
                }
            }
            return domainBuyers;
        }
        
        //
        public DomainBuyer GetBuyer(int id)
        {
            return Buyers.FirstOrDefault(x=>x.Id==id);
        }

        //
        public DomainBuyer GetBuyer(string email)
        {
            DomainBuyer model=Buyers.FirstOrDefault(x => x.Email == email);
            if(model==null)
            {
                model = new DomainBuyer { Email = email };
                Buyers.Add(model);
                _Repositories.Buyers.Create(model.FromDomainBuyerToRepoBuyer());
                Buyers.Clear();
                Buyers = _Repositories.Buyers.GetAll().Select(x=>x.FromRepoBuyerToDomainBuyer()).ToList();
                model= Buyers.FirstOrDefault(x => x.Email == email); 
            }
            return model;
        }
        
        //
        public void Create_Buyer(DomainBuyer item)
        {
            _Repositories.Buyers.Create(item.FromDomainBuyerToRepoBuyer());
            Buyers.Clear();
            Buyers = _Repositories.Buyers.GetAll().Select(x => x.FromRepoBuyerToDomainBuyer()).ToList();
        }

        //
        public void Update_Buyer(DomainBuyer item)
        {

        }

        //
        public void Delete_Buyer(int id)
        {
            _Repositories.Buyers.Delete(id);
        }

        public void Delete_BuyerByEmail(string mail)
        {
            DomainBuyer buyer= Buyers.Find(x => x.Email == mail);           
            int i = buyer.Id;
            if (buyer != null)
            {
                _Repositories.Buyers.Delete(buyer.Id);
                Buyers.Remove(buyer);
            }
            List<DomainCar> cars= Cars.Where(x=>x.OwnerId==i).ToList();
            for(int count=0;count<cars.Count;count++)
            {
                int index=Cars.IndexOf(cars[count]);
                cars[count].Status = "Продавец был удален админом";
                cars[count].OwnerId = null;
                Cars[index] = cars[count];
                _Repositories.Cars.Update(cars[count].FromDomainCarToRepoCar());
            }
        }
        
        //
        public void Delete_Purchase(int Id,int BuyerId)
        {
            DomainBuyCar buyCar = BuyCars.FirstOrDefault(x=>x.CarId==Id && x.BuyerId==BuyerId);
            _Repositories.BuyCars.Delete(buyCar.FromDomainBuyCarToRepoBuyCar());
            BuyCars.Remove(buyCar);
        }
    }
}
