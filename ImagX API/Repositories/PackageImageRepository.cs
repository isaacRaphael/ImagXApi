using ImagX_API.Contracts;
using ImagX_API.Data;
using ImagX_API.Entities;

namespace ImagX_API.Repositories
{
    public class PackageImageRepository : GenericRepository<PackageImage>, IPackageImageRepository
    {
        public PackageImageRepository(AppDbContext context) : base(context)
        {

        }
    }
}
