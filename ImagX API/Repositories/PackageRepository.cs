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
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        private readonly AppDbContext _context;

        public PackageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Package>> RetrieveUserPackages(string userId)
        {
            return await _context.Packages.Where(x => x.receiverId == userId).ToListAsync();
        }
    }
}
