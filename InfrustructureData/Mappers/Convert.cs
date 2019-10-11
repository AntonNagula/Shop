using AutoMapper;
using InfructructureDataInterfaces.Models;
using InfrustructureData.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrustructureData.Mappers
{
    public static class Convert
    {
        
        public static int FromCarToInt(this Car item)
        {
            return item.Id;
        }

        public static BuyCar FromRepoBuyCarToBuyCar(this RepoBuyCar item)
        {
            return new BuyCar
            {
                BuyerId = item.BuyerId,
                CarId = item.CarId
            };
        }

        public static RepoBuyCar FromBuyCarToRepoBuyCar(this BuyCar item)
        {
            return new RepoBuyCar
            {
                BuyerId = item.BuyerId,
                Buyer = item.Buyer.FromBuyerToRepoBuyer(),
                Car = item.Car.FromCarToRepoCar(),
                CarId = item.CarId
            };
        }

        public static RepoBrands FromBrandsToRepoBrands(this Brands item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Brands, RepoBrands>()
            .ForMember(x => x.Cars, _ => _.Ignore()));
            return Mapper.Map<Brands, RepoBrands>(item);
        }

        public static Brands FromRepoBrandsToBrands(this RepoBrands item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RepoBrands, Brands>()
            .ForMember(x => x.Cars, _ => _.Ignore()));
            return Mapper.Map<RepoBrands, Brands>(item);
        }


        public static Car FromRepoCarToCar(this RepoCar item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RepoCar, Car>()
            .ForMember(x => x.BuyCars, _ => _.Ignore()));
            return Mapper.Map<RepoCar, Car>(item);
        }

        public static RepoCar FromCarToRepoCar(this Car item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Car, RepoCar>()
            .ForMember(x => x.BuyCars, _ => _.Ignore()));
            return Mapper.Map<Car, RepoCar>(item);
        }


        public static Buyer FromRepoBuyerToBuyer(this RepoBuyer item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RepoBuyer, Buyer>()
            .ForMember(x => x.BuyCars, _ => _.Ignore())
            .ForMember(x=>x.Cars,x=>x.Ignore()));
            return Mapper.Map<RepoBuyer, Buyer>(item);
        }

        public static RepoBuyer FromBuyerToRepoBuyer(this Buyer item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Buyer, RepoBuyer>()
            .ForMember(x => x.BuyCars, _ => _.Ignore()));
            return Mapper.Map<Buyer, RepoBuyer>(item);
        }

        public static RepoSpeach FromSpeachToRepoSpeach(this Speach item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Speach, RepoSpeach>());
            return Mapper.Map<Speach, RepoSpeach>(item);
        }

        public static Speach FromRepoSpeachToSpeach(this RepoSpeach item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RepoSpeach, Speach>()
            .ForMember(x => x.Messages, _ => _.Ignore()));
            return Mapper.Map<RepoSpeach, Speach>(item);
        }

        public static RepoMessage FromMessageToRepoMessage(this Message item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Message, RepoMessage>());
            return Mapper.Map<Message, RepoMessage>(item);
        }

        public static Message FromRepoMessageToMessage(this RepoMessage item)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RepoMessage, Message>()
            .ForMember(x => x.Speach, _ => _.Ignore()));
            return Mapper.Map<RepoMessage, Message>(item);
        }
    }
}
