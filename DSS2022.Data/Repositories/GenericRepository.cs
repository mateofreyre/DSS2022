using DSS2022.Data.Repositories.Interfaces;
using DSS2022.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DSS2022DataContext _context;

        public GenericRepository(DSS2022DataContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

        }

        public T Read(int id)
        {
            return _context.Set<T>().SingleOrDefault(m => m.Id == id);

        }

        public async Task<T> ReadAsync(int id)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(m => m.Id == id);

        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);

        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public virtual async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
