using Auto.ModelsApp;
using DomainCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auto.Mappers
{
    public static class Mapping
    {
        public static DomainBuyCar FromAppCarToDomainBuyCar(this AppBuyCar item)
        {
            return new DomainBuyCar
            {
                 CarId=item.CarId,
                 BuyerId=item.BuyerId
            };
        }

        public static DomainCar FromRepoCarToDomainCar(this AppCar item)
        {
            return new DomainCar
            {
                Id = item.Id,
                Name = item.Name,
                CarBrand = item.CarBrand,
                Price = item.Price,
                ExtencionName = item.ExtencionName,
                Info = item.Info,
                image = item.image,
                OwnerId=item.OwnerId,
                BrandId=item.BrandId,
                Status=item.Status
            };
        }

        public static AppCar FromDomainCarToRepoCar(this DomainCar item)
        {
            return new AppCar
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

        public static DomainBuyer FromRepoBuyerToDomainBuyer(this AppBuyer item)
        {
            return new DomainBuyer
            {
                Id = item.Id,
                Email = item.Email,
                Telephone = item.Telephone,
                TelegramID=item.TelegramID,
            };
        }

        public static AppBuyer FromDomainBuyerToRepoBuyer(this DomainBuyer item)
        {
            return new AppBuyer
            {
                Id = item.Id,
                Email = item.Email,
                Telephone=item.Telephone,
                TelegramID=item.TelegramID,
            };
        }

        public static string FromAppBrandToBrand(this DomainBrands item)
        {
            return item.BrandName;
        }

        public static DomainBrands FromAppCarBrandToDomainBrand(this AppCarBrand item)
        {
            return new DomainBrands
            {
                Id = item.Id,
                BrandName =item.BrandName,

            };
        }

        public static AppCarBrand FromDomainBrandsToAppCarBrand(this DomainBrands item)
        {
            return new AppCarBrand
            {
                Id = item.Id,
                BrandName = item.BrandName,
            };
        }


        public static DomainSpeach FromRepoSpeachToDomainSpeach(this AppSpeach item)
        {
            return new DomainSpeach
            {
                Id = item.Id,
                IdCar = item.IdCar,
                IdOwner=item.IdOwner,
                IdUser = item.IdUser,
                LastMes = item.LastMes,
                Name = item.Name,
                Readed = item.readed,
                IsDeleted=item.IsDeleted,
            };
        }

        public static AppSpeach FromDomainSpeachToSpeachSpeach(this DomainSpeach item)
        {
            return new AppSpeach
            {
                Id = item.Id,
                IdCar = item.IdCar,
                IdOwner = item.IdOwner,
                IdUser = item.IdUser,
                LastMes = item.LastMes,
                Name = item.Name,
                readed=item.Readed,
                IsDeleted=item.IsDeleted,
            };
        }

        public static DomainMessage FromAppMessageToDomainMessage(this AppMessage item)
        {
            return new DomainMessage
            {
                Id = item.Id,
                IdUser = item.IdUser,
                SpeachId = item.SpeachId,
                Text = item.Text,
                Name = item.Name,
            };
        }

        public static AppMessage FromDomainMessageToAppMessage(this DomainMessage item)
        {
            return new AppMessage
            {
                Id = item.Id,
                IdUser = item.IdUser,
                SpeachId = item.SpeachId,
                Text = item.Text,
                Name=item.Name,
            };
        }
    }
}