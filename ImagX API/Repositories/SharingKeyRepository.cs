using ImagX_API.Contracts;
using ImagX_API.Data;
using ImagX_API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Repositories
{
    public class SharingKeyRepository : GenericRepository<SharingKey>, ISharingKeyRepository
    {
        private readonly AppDbContext _context;

        public SharingKeyRepository(AppDbContext context): base(context)
        {
           _context = context;
        }

        public async Task<bool> Exists(string key)
        {
            return  await _context.Keys.AnyAsync(k => k.Key == key);
        }
    }
}
