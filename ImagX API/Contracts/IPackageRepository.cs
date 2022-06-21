using ImagX_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        Task<ICollection<Package>> RetrieveUserPackages(string userId);
    }
}
