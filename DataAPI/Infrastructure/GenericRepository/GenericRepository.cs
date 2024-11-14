using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAPI.Infrastructure.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DotnetCurrencyProjectContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DotnetCurrencyProjectContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetById(int id)
        {
            var result = await _dbSet.FindAsync(id);
            if (result == null)
                throw new Exception("There is no field with that Id");
            return result;
        }
        public async Task<T> Add(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            if (result == null)
                throw new Exception("Creation Failed");
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Update(T entity)
        {
            _dbSet.Attach(entity);
            var result = _context.Entry(entity).State == EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new Exception("There is no field with that Id");
            else
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
