using ImagX_API.Contracts;
using ImagX_API.Data;
using ImagX_API.Entities.Base;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _table;


        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _table = context.Set<T>();
        }
        public async Task<T> Add(T entity)
        {
            await _table.AddAsync(entity);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0 ? entity : null;
        }

        public async Task<bool> Exists(int id)
        {
            return await _table.AnyAsync(x => x.Id == id);
        }

        public async Task<ICollection<T>> GetAll()
        {
            if (_table.Any())
                return await _table.ToListAsync();

            return null;
        }

        public async Task<T> GetById(int id)
        {
            return await _table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<T>> GetPaginated(int page = 1, int pageSize = 10)
        {
            return await _table.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<bool> Remove(int id)
        {
            var toremove = await _table.FindAsync(id);
            
            _table.Remove(toremove);
            return await _context.SaveChangesAsync() > 0;
        }

        public async virtual Task<bool> Update(JsonPatchDocument entity, int id)
        {
            var toUpdate = await _table.FirstOrDefaultAsync(x => x.Id == id);
            if (toUpdate is not null)
                entity.ApplyTo(toUpdate);

            return await _context.SaveChangesAsync() > 0;

        }

        
    }
}
