using DomainCore.Interfaces;
using DomainCore.Models;
using InfructructureDataInterfaces.Repositories;
using DomainService.Mappers;
using System.Collections.Generic;
using System.Linq;
using System;
using TLSharp.Core;
using TeleSharp.TL;

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
        List<DomainSpeach> Speaches;
        List<DomainMessage> Messages;

        public Service(IUnitOfWork Repository)
        {
            _Repositories = Repository;
            Cars = Repository.Cars.GetAll().Select(x=>x.FromRepoCarToDomainCar()).ToList();
            BuyCars = Repository.BuyCars.GetAll().Select(x => x.FromRepoBuyCarToDomainBuyCar()).ToList();
            Buyers = Repository.Buyers.GetAll().Select(x => x.FromRepoBuyerToDomainBuyer()).ToList();
            Brands = Repository.Brands.GetAll().Select(x => x.FromRepoBrandToDomainBrand()).ToList();
            Messages = Repository.Messages.GetAll().Select(x => x.FromRepoMessageToDomainMessage()).ToList();
            Speaches = Repository.Speaches.GetAll().Select(x => x.FromRepoSpeachToDomainSpeach()).ToList();
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

        public IEnumerable<DomainCar> Annociment(int i, int size, string brand, out int Total,int IdBuyer,int? minPrise,int? maxPrice)
        {
            List<DomainCar> currentcars;
            if (brand != "Все")
                currentcars = Cars.Where(x => x.CarBrand == brand && x.Status == "Продается" && x.OwnerId!=IdBuyer && (minPrise==0 ? true : x.Price > minPrise) && (maxPrice == 0 ? true : x.Price < maxPrice)).ToList();
            else
                currentcars = Cars.Where(x => x.Status == "Продается" && x.OwnerId != IdBuyer && (minPrise == 0 ? true : x.Price > minPrise) && (maxPrice == 0 ? true : x.Price < maxPrice)).ToList();
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
            for(int j=0;j< Speaches.Count;j++)
            {
                Speaches[j].IdCar = 0;
                _Repositories.Speaches.Update(Speaches[j].FromDomainSpeachToSpeachSpeach());
            }
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
            _Repositories.Speaches.DeleteRange();
            Speaches.Clear();
            Speaches = _Repositories.Speaches.GetAll().Select(x => x.FromRepoSpeachToDomainSpeach()).ToList();
            Messages.Clear();
            Messages = _Repositories.Messages.GetAll().Select(x => x.FromRepoMessageToDomainMessage()).ToList();
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
               

        public IEnumerable<DomainMessage> GetMessages(int idUser, int AutoId)
        {
            int idSpeach;
            bool sp= Speaches.Exists(x => x.IdCar == AutoId && x.IdUser == idUser);
            if(sp==false)
            {
                DomainSpeach speach = new DomainSpeach() { IdCar=AutoId, IdUser=idUser, IdOwner=(int)Cars.First(x=>x.Id==AutoId).OwnerId, Name=Buyers.First(x=>x.Id==idUser).Email};
                _Repositories.Speaches.Create(speach.FromDomainSpeachToSpeachSpeach());
                Speaches= _Repositories.Speaches.GetAll().Select(x=>x.FromRepoSpeachToDomainSpeach()).ToList();
                idSpeach = Speaches.First(x => x.IdCar == AutoId && x.IdUser == idUser).Id;
                DomainMessage message = new DomainMessage() { SpeachId=idSpeach, IdUser=idUser,Text="Здравствуйте", Name=Buyers.First(x=>x.Id==idUser).Email };
                Messages.Add(message);
                _Repositories.Messages.Create(message.FromDomainSpeachToRepoSpeach());
                Messages.Clear();
                Messages=_Repositories.Messages.GetAll().Select(x => x.FromRepoMessageToDomainMessage()).ToList();
                return Messages.Where(x => x.SpeachId == idSpeach).ToList();
            }
            idSpeach = Speaches.First(x => x.IdCar == AutoId && x.IdUser == idUser).Id;
            return Messages.Where(x=>x.SpeachId==idSpeach).ToList();
        }

        public IEnumerable<DomainMessage> GetMessages(int idSpeach)
        {
            return Messages.Where(x => x.SpeachId == idSpeach).ToList();
        }

        public IEnumerable<DomainMessage> GetRangeMessage(int id)
        {
            throw new NotImplementedException();
        }

        public void CreateSpeach(int AutoId, int UserId)
        {
            DomainSpeach speach = new DomainSpeach() { IdCar = AutoId, IdUser = UserId };
            _Repositories.Speaches.Create(speach.FromDomainSpeachToSpeachSpeach());

        }

        public void CreateMessage(int idSpeach, string name, string messages)
        {
            int id = Buyers.First(x => x.Email == name).Id;
            DomainMessage mes = new DomainMessage() { IdUser=id, SpeachId=idSpeach, Text=messages, Name=name};
            _Repositories.Messages.Create(mes.FromDomainSpeachToRepoSpeach());
            Messages.Add(mes);
        }

        public void RejectMessage(int AutoId, int BuyerId)
        {            
            bool sp = Speaches.Exists(x => x.IdCar == AutoId && x.IdUser == BuyerId);
            if (sp == true)
            {
                string name = Buyers.First(x => x.Id == BuyerId).Email;
                int idSpeach = Speaches.First(x => x.IdCar == AutoId && x.IdUser == BuyerId).Id;
                DomainMessage message = new DomainMessage() { SpeachId = idSpeach, IdUser = BuyerId, Text = "Данное предложение более не актуально для меня", Name = name};
                Messages.Add(message);
                _Repositories.Messages.Create(message.FromDomainSpeachToRepoSpeach());
                Messages.Clear();
                Messages = _Repositories.Messages.GetAll().Select(x => x.FromRepoMessageToDomainMessage()).ToList();
                DomainSpeach speach=Speaches.First(x => x.Id == idSpeach);
                speach.Name =name+" более не поддерживает диалог";
                speach.IdUser = 0;
                _Repositories.Speaches.Update(speach.FromDomainSpeachToSpeachSpeach());
            }          

        }

        public IEnumerable<DomainSpeach> GetAutoSpeach(int id)
        {
            return Speaches.Where(x=>x.IdCar==id).ToList();
        }

        public IEnumerable<DomainSpeach> GetUserSpeach(int id)
        {
            return Speaches.Where(x => x.IdUser == id).ToList();
        }

        public IEnumerable<DomainSpeach> GetOwnerSpeach(int id)
        {
            return Speaches.Where(x => x.IdOwner == id).ToList();
        }

        public IEnumerable<DomainMessage> OwnerGetMessages(int OwnerId,int UserId, int AutoId)
        {
            int idSpeach;
            bool sp = Speaches.Exists(x => x.IdCar == AutoId && x.IdOwner == OwnerId);
            if (sp == false)
            {
                DomainSpeach speach = new DomainSpeach() { IdCar = AutoId, IdOwner=OwnerId, IdUser = UserId, Name=Buyers.First(x=>x.Id==UserId).Email };
                _Repositories.Speaches.Create(speach.FromDomainSpeachToSpeachSpeach());
                Speaches = _Repositories.Speaches.GetAll().Select(x => x.FromRepoSpeachToDomainSpeach()).ToList();
                idSpeach = Speaches.First(x => x.IdCar == AutoId && x.IdOwner==OwnerId).Id;
                DomainMessage message = new DomainMessage() { SpeachId = idSpeach, IdUser = OwnerId, Text = "Здравствуйте", Name = Buyers.First(x => x.Id == OwnerId).Email };
                Messages.Add(message);
                _Repositories.Messages.Create(message.FromDomainSpeachToRepoSpeach());
                Messages.Clear();
                Messages = _Repositories.Messages.GetAll().Select(x => x.FromRepoMessageToDomainMessage()).ToList();
                return Messages.Where(x => x.SpeachId == idSpeach).ToList();
            }
            idSpeach = Speaches.First(x => x.IdCar == AutoId && x.IdUser == UserId).Id;
            return Messages.Where(x => x.SpeachId == idSpeach).ToList();
        }

        public void DelSpeach(int IdSpeach)
        {
            DomainSpeach speach = Speaches.First(x => x.Id == IdSpeach);
            int id = Speaches.IndexOf(speach);
            speach.IdCar = 0;     
            Speaches[id].IdCar = 0;
            _Repositories.Speaches.Update(speach.FromDomainSpeachToSpeachSpeach());
        }


        public async void SendMassege(string mail, int CarId)
        {
            string number = Buyers.First(x=>x.Id==Cars.First(_ => _.Id == CarId).OwnerId).Telephone;

            var client = new TelegramClient(956436, "b1a9d979ab2e0a0da72061f66189e0e7");

            await client.ConnectAsync();

            var hash = await client.SendCodeRequestAsync("+375293061643");
            var code = "48125"; // you can change code in debugger

            //var user = await client.MakeAuthAsync("+375293061643", hash, code);

            //TeleSharp.TL.Contacts.TLRequestImportContacts requestImportContacts = new TeleSharp.TL.Contacts.TLRequestImportContacts();
            //requestImportContacts.Contacts = new TLVector<TLInputPhoneContact>();
            //requestImportContacts.Contacts.Add(new TLInputPhoneContact()
            //{
            //    Phone = "00375257055814",
            //    FirstName = "Холден",
            //    LastName = "Колфилд"
            //});
            //var o = await client.SendRequestAsync<TeleSharp.TL.Contacts.TLImportedContacts>((TLMethod)requestImportContacts);
            //var NewUserId = (o.Users.First() as TLUser).Id;
            //var d = await client.SendMessageAsync(new TLInputPeerUser() { UserId = NewUserId }, "test message text");


            //using (FileStream fstream = new FileStream(@"C:\Users\Banderos\Desktop\Viber\telegram\telegram\note.txt", FileMode.OpenOrCreate))
            //{
            //    // преобразуем строку в байты
            //    byte[] array = Encoding.Default.GetBytes(hash);
            //    // запись массива байтов в файл
            //    fstream.Write(array, 0, array.Length);
            //}
            var user = await client.MakeAuthAsync("+375293061643",hash, code);


            var result = await client.GetContactsAsync();
            var users = result.Users
                .Where(x => x.GetType() == typeof(TLUser))
                .Cast<TLUser>()
                .FirstOrDefault(x => x.Phone == "375333586708");


            //send message
            var d = await client.SendMessageAsync(new TLInputPeerUser() { UserId = users.Id }, mail+" хочет купить ваше авто(Тестовое сообщение, не обращай внимание)");

        }
    }
}
