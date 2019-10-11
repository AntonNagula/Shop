﻿using DomainCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainCore.Interfaces
{
    public interface IService
    {
        IEnumerable<DomainCar> GetAllCars();
        IEnumerable<DomainCar> GetAllCars(int i,int size,out int Total);
        IEnumerable<DomainCar> GetAllCars(int i, int size, string brand,out int Total);
        IEnumerable<DomainCar> Annociment(int i, int size, string brand, out int Total, int IdBuyer, int? minPrise, int? maxPrice);
        IEnumerable<DomainCar> GetCarsBuyerId(int id);
        IEnumerable<DomainCar> GetCarsByOwnerId(int id);
        DomainCar GetCar(int id);
        void Create_Car(DomainCar item);
        void Update_Car(DomainCar item);
        void Delete_Car(int id);
        void AdminDelete(int id);
        void DeleteRangeCars();
        IEnumerable<DomainBuyer> GetAllBuyers();
        IEnumerable<DomainBuyer> GetBuyersByCarId(int id);
        DomainBuyer GetBuyer(int id);
        DomainBuyer GetBuyer(string email);
        void Create_Buyer(DomainBuyer item);
        void Update_Buyer(DomainBuyer item);
        void Delete_Buyer(int id);
        void Delete_BuyerByEmail(string mail);


        bool Buy(DomainBuyCar item);
        void Delete_Purchase(int Id,int BuyerId);

        void CreateBrand(string brandName);
        IEnumerable<DomainBrands> GetAllBrands();

        IEnumerable<DomainSpeach> GetAutoSpeach(int id);
        IEnumerable<DomainSpeach> GetUserSpeach(int id);
        IEnumerable<DomainMessage> GetMessages(int idUser,int AutoId);
        IEnumerable<DomainMessage> GetRangeMessage(int id);
        IEnumerable<DomainMessage> GetMessages(int idSpeach);
        IEnumerable<DomainMessage> OwnerGetMessages(int OwnerId,int UserId, int AutoId);
        void CreateSpeach(int AutoId, int UserId);
        void CreateMessage(int idSpeach, string name, string messages);
        void RejectMessage(int AutoId, int BuyerId);
        void DelSpeach(int IdSpeach);
        void SendMassege(string mail,int CarId);
        
    }
}
