using InfructructureDataInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfructructureDataInterfaces.Repositories
{
    public interface IUnitOfWork
    {
        IRepositories<RepoBuyer> Buyers { get; }

        IRepositories<RepoCar> Cars { get; }

        PurchaseRepositories<RepoBuyCar> BuyCars { get; }

        IRepositories<RepoBrands> Brands { get; }

        IRepositories<RepoMessage> Messages { get; }

        IRepositories<RepoSpeach> Speaches { get; }
    }
}
