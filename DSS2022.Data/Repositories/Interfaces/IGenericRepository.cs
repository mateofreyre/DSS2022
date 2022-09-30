using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task AddAsync(T entity);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        Task<T> ReadAsync(int id);
        T Read(int id);
    }
}
