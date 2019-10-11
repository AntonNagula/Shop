﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfructructureDataInterfaces.Repositories
{
    public interface IRepositories<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetRange(int id);
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void DeleteRange();
    }
}
