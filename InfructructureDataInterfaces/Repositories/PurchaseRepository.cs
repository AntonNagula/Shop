using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfructructureDataInterfaces.Repositories
{
    public interface PurchaseRepositories<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(T id);
        void DeleteRange(List<T> items);
    }
}
