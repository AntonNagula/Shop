using DomainCore.Interfaces;
using DomainService.Service;
using InfructructureDataInterfaces.Models;
using InfructructureDataInterfaces.Repositories;
using InfrustructureData.Data;
using InfrustructureData.Repositories;
using Ninject.Modules;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Ninject.Web.Common;
namespace IoC
{
    public class Module : NinjectModule
    {       

        public override void Load()
        {
            Bind<IRepositories<RepoBuyer>>().To<BuyerRepository>().InSingletonScope();
            Bind<IRepositories<RepoCar>>().To<CarRepository>().InSingletonScope();
            Bind<IRepositories<RepoBrands>>().To<BrandRepository>().InSingletonScope();
            Bind<PurchaseRepositories<RepoBuyCar>>().To<PurchaseRepository>().InSingletonScope();
            Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();
            Bind<IService>().To<Service>().InSingletonScope();
            Bind<IRepositories<RepoMessage>>().To<MessageRepository>().InSingletonScope();
            Bind<IRepositories<RepoSpeach>>().To<SpeachRepository>().InSingletonScope();
            Bind<AutoContext>().ToSelf().InSingletonScope().WithConstructorArgument("_connectionstring", c => Config.ConnectionString);
        }

    }
}
