using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryMVC.Helpers;
using InventoryMVC.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T: class
    {
        private readonly InventoryContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(InventoryContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Add(T obj)
        {
            _dbSet.Add(obj);
        }

        public void delete(T obj)
        {
            _dbSet.Remove(obj);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void update(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

    }
}
