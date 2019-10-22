using AutoMapper;
using DomainCore.Models;
using InfructructureDataInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Mappers
{
    public static class Map
    {
        public static DomainCar FromRepoCarToDomainCar(this RepoCar item)
        {
            if (item == null) return null;
            return new DomainCar
            {
                Id = item.Id,
                Name = item.Name,
                CarBrand = item.CarBrand,
                Price = item.Price,
                ExtencionName = item.ExtencionName,
                Info = item.Info,
                image = item.image,
                OwnerId = item.OwnerId,
                BrandId = item.BrandId,
                Status=item.Status                
            };
        }

        public static RepoCar FromDomainCarToRepoCar(this DomainCar item)
        {
            if (item == null) return null;
            return new RepoCar
            {
                Id = item.Id,
                Name = item.Name,
                CarBrand = item.CarBrand,
                Price = item.Price,
                ExtencionName = item.ExtencionName,
                Info = item.Info,
                image = item.image,
                OwnerId = item.OwnerId,
                BrandId = item.BrandId,
                Status=item.Status
            };
        }

        public static DomainBuyer FromRepoBuyerToDomainBuyer(this RepoBuyer item)
        {
            if (item == null) return null;
            return new DomainBuyer
            {
                Id = item.Id,
                Email = item.Email,
                Telephone = item.Telephone,
                TelegramID=item.TelegramID,
            };
        }

        public static RepoBuyer FromDomainBuyerToRepoBuyer(this DomainBuyer item)
        {
            if (item == null) return null;
            return new RepoBuyer
            {
                Id = item.Id,
                Email = item.Email,
                Telephone = item.Telephone,
                TelegramID=item.TelegramID,
            };
        }

        public static DomainBuyCar FromRepoBuyCarToDomainBuyCar(this RepoBuyCar item)
        {
            return new DomainBuyCar
            {
                BuyerId = item.BuyerId,
                Buyer = item.Buyer.FromRepoBuyerToDomainBuyer(),
                Car = item.Car.FromRepoCarToDomainCar(),
                CarId = item.CarId
            };
        }

        public static RepoBuyCar FromDomainBuyCarToRepoBuyCar(this DomainBuyCar item)
        {
            return new RepoBuyCar
            {
                BuyerId = item.BuyerId,
                Buyer = item.Buyer.FromDomainBuyerToRepoBuyer(),
                Car = item.Car.FromDomainCarToRepoCar(),
                CarId = item.CarId
            };
        }

        public static DomainBrands FromRepoBrandToDomainBrand(this RepoBrands item)
        {
            return new DomainBrands
            {
                Id = item.Id,
                BrandName = item.BrandName,

            };
        }

        public static RepoBrands FromDomainBrandToRepoBrand(this DomainBrands item)
        {
            return new RepoBrands
            {                
                BrandName = item.BrandName,
            };
        }

        public static DomainSpeach FromRepoSpeachToDomainSpeach(this RepoSpeach item)
        {
            return new DomainSpeach
            {
                Id = item.Id,
                IdCar =item.IdCar,
                IdOwner = item.IdOwner,
                IdUser =item.IdUser,
                LastMes=item.LastMes,
                Name = item.Name,
                IsDeleted=item.IsDeleted,
            };
        }

        public static RepoSpeach FromDomainSpeachToSpeachSpeach(this DomainSpeach item)
        {
            return new RepoSpeach
            {
                Id = item.Id,
                IdCar = item.IdCar,
                IdOwner = item.IdOwner,
                IdUser = item.IdUser,
                LastMes = item.LastMes,
                Name = item.Name,
                IsDeleted=item.IsDeleted,
            };
        }

        public static DomainMessage FromRepoMessageToDomainMessage(this RepoMessage item)
        {
            return new DomainMessage
            {
                Id = item.Id,
                IdUser=item.IdUser,
                SpeachId=item.SpeachId,
                Text=item.Text,
                Name = item.Name,
            };
        }

        public static RepoMessage FromDomainSpeachToRepoSpeach(this DomainMessage item)
        {
            return new RepoMessage
            {
                Id = item.Id,
                IdUser = item.IdUser,
                SpeachId = item.SpeachId,
                Text = item.Text,
                Name = item.Name,
            };
        }
    }
}
