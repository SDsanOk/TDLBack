using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpleApp.Models;

namespace simpleApp.Data
{
    public interface IStore<T>
    {
        void Add(T entity, int id);
        T Get(int id);
        void Delete(int id);
        void Update(T entity);
        IEnumerable<T> GetListByUserId(int id);
    }
}
