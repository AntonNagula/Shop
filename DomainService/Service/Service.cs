using DomainCore.Interfaces;
using DomainCore.Models;
using InfructructureDataInterfaces.Repositories;
using DomainService.Mappers;
using System.Collections.Generic;
using System.Linq;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Threading.Tasks;
using System.Threading;

namespace DomainService.Service
{
    public class Service : IService
    {        
        public readonly IUnitOfWork _Repositories;
        static ITelegramBotClient botClient;
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


        // telegramm
        public void SendMessage()
        {         
             
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
        void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                int code = Int32.Parse(e.Message.Text);
                bool b = Buyers.Exists(x=>x.AuthCode == code);
                if (b == true)
                {
                    DomainBuyer buyer = Buyers.First(x => x.AuthCode == code);
                    Buyers.Remove(buyer);
                    buyer.TelegramID = e.Message.From.Id;
                    Buyers.Add(buyer);
                    _Repositories.Buyers.Update(buyer.FromDomainBuyerToRepoBuyer());
                    Task.Run(() => Bot_OnMessage("Подписка прошла успешно", buyer.TelegramID));
                }
            }
        }
        public int Create_Code_Telegramm(int Id, string telephone)
        {
            Random rnd = new Random();
            int value = rnd.Next(100000, 1000000);
            DomainBuyer b = Buyers.First(x=>x.Id==Id);
            Buyers.Remove(b);
            b.AuthCode = value;
            b.Telephone = telephone;
            Buyers.Add(b);
            _Repositories.Buyers.Update(b.FromDomainBuyerToRepoBuyer());
            return value;
        }

        public Service(IUnitOfWork Repository)
        {
            _Repositories = Repository;
            Cars = Repository.Cars.GetAll().Select(x=>x.FromRepoCarToDomainCar()).ToList();
            BuyCars = Repository.BuyCars.GetAll().Select(x => x.FromRepoBuyCarToDomainBuyCar()).ToList();
            Buyers = Repository.Buyers.GetAll().Select(x => x.FromRepoBuyerToDomainBuyer()).ToList();
            Brands = Repository.Brands.GetAll().Select(x => x.FromRepoBrandToDomainBrand()).ToList();
            Messages = Repository.Messages.GetAll().Select(x => x.FromRepoMessageToDomainMessage()).ToList();
            Speaches = Repository.Speaches.GetAll().Select(x => x.FromRepoSpeachToDomainSpeach()).ToList();
            botClient = new TelegramBotClient("986923820:AAH7Df3wrTKkfsusnCrWpnash3RZPChCOgI");
            Task.Run(()=>SendMessage());
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

        public IEnumerable<DomainCar> Annociment(int i, int size, string brand, out int Total, int IdBuyer)
        {
            List<DomainCar> currentcars;
            if (brand != "Все")
                currentcars = Cars.Where(x => x.CarBrand == brand && x.Status == "Продается" && x.OwnerId != IdBuyer).ToList();
            else
                currentcars = Cars.Where(x => x.Status == "Продается" && x.OwnerId != IdBuyer).ToList();
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
                botClient = new TelegramBotClient("986923820:AAH7Df3wrTKkfsusnCrWpnash3RZPChCOgI");
                Task.Run(()=>Bot_OnMessage(item.Buyer.Email+" хочет купить "+item.Car.Name,Buyers.First(x=>x.Id==car.OwnerId).TelegramID));
                                
                _Repositories.BuyCars.Create(item.FromDomainBuyCarToRepoBuyCar());
                BuyCars.Add(item);
                return true;
            }

            return false;
        }
        //telegramm
        static async void Bot_OnMessage(string message,int chatid)
        {
            await botClient.SendTextMessageAsync(

                  chatId: chatid,
                  text: message +"\n"
                );
        }
        //
        //


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
        public void CreateBuyer(string email,string phone)
        {
            DomainBuyer model = new DomainBuyer { Email = email, Telephone=phone };
            Buyers.Add(model);
            _Repositories.Buyers.Create(model.FromDomainBuyerToRepoBuyer());
            Buyers.Clear();
            Buyers = _Repositories.Buyers.GetAll().Select(x => x.FromRepoBuyerToDomainBuyer()).ToList();
            
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
        public void Delete_Purchase(int CarId,int BuyerId)
        {
            DomainBuyCar buyCar = BuyCars.FirstOrDefault(x=>x.CarId== CarId && x.BuyerId==BuyerId);
            _Repositories.BuyCars.Delete(buyCar.FromDomainBuyCarToRepoBuyCar());
            BuyCars.Remove(buyCar);
            bool flag = Speaches.Exists(x => x.IdCar == CarId && x.IdUser == BuyerId && x.IsDeleted == false);
            if (flag == true)
            {
                DomainSpeach speach = Speaches.First(x => x.IdCar == CarId && x.IdUser == BuyerId && x.IsDeleted == false);
                Speaches.Remove(speach);
                speach.IsDeleted = true;
                _Repositories.Speaches.Update(speach.FromDomainSpeachToSpeachSpeach());
            }
        }
               

        public IEnumerable<DomainMessage> GetMessages(int idUser, int AutoId)
        {
            int idSpeach;
            bool sp= Speaches.Exists(x => x.IdCar == AutoId && x.IdUser == idUser && x.IsDeleted==false);
            if(sp==false)
            {
                DomainSpeach speach = new DomainSpeach() { IdCar=AutoId, IdUser=idUser, IdOwner=(int)Cars.First(x=>x.Id==AutoId).OwnerId, Name=Buyers.First(x=>x.Id==idUser).Email, IsDeleted=false};
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
            DomainSpeach speach = new DomainSpeach() { IdCar = AutoId, IdUser = UserId, IsDeleted=false };
            _Repositories.Speaches.Create(speach.FromDomainSpeachToSpeachSpeach());

        }

        public void CreateMessage(int idSpeach, string name, string messages)
        {
            int id = Buyers.First(x => x.Email == name).Id;
            DomainMessage mes = new DomainMessage() { IdUser=id, SpeachId=idSpeach, Text=messages, Name=name};
            _Repositories.Messages.Create(mes.FromDomainSpeachToRepoSpeach());
            //int idSpeachInList = Speaches.IndexOf(Speaches.Find(x=>x.Id==idSpeach));
            //Speaches[idSpeachInList].LastMes=messages;
            //Speaches[idSpeachInList].Readed = false;
            //Speaches[idSpeachInList].Name = name;
            Messages.Add(mes);
        }        

        public IEnumerable<DomainSpeach> GetAutoSpeach(int IdCar,string name)
        {
            //List<DomainSpeach> newmessges=Speaches.Where(x=>x.IdCar==id && x.Name!=name && x.Readed==false).ToList();
            //List<DomainSpeach> othersmessges = Speaches.Where(x =>x.IdCar==id && !(x.Name != name && x.Readed == false)).ToList();
            //newmessges.AddRange(othersmessges);
            return Speaches.Where(x=>x.IdCar==IdCar && x.IsDeleted==false);
        }

        public IEnumerable<DomainSpeach> GetUserSpeach(int idUser)
        {
            return Speaches.Where(x => x.IdUser == idUser && x.IsDeleted==false).ToList();
        }

        public IEnumerable<DomainSpeach> GetOwnerSpeach(int id)
        {
            return Speaches.Where(x => x.IdOwner == id && x.IsDeleted==false).ToList();
        }

        public IEnumerable<DomainMessage> OwnerGetMessages(int OwnerId,int UserId, int AutoId)
        {
            int idSpeach;
            bool sp = Speaches.Exists(x => x.IdCar == AutoId && x.IdOwner == OwnerId && x.IsDeleted==false);
            if (sp != true)
            {
                DomainSpeach speach = new DomainSpeach() { IdCar = AutoId, IdOwner=OwnerId, IdUser = UserId, Name=Buyers.First(x=>x.Id==UserId).Email, IsDeleted=false };
                _Repositories.Speaches.Create(speach.FromDomainSpeachToSpeachSpeach());
                Speaches = _Repositories.Speaches.GetAll().Where(x=>x.IsDeleted==false).Select(x => x.FromRepoSpeachToDomainSpeach()).ToList();
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
            DomainSpeach speach = Speaches.First(x => x.Id == IdSpeach && x.IsDeleted==false);
            Speaches.Remove(speach);
            speach.IsDeleted = true;            
            _Repositories.Speaches.Update(speach.FromDomainSpeachToSpeachSpeach());
        }        
    }
}
