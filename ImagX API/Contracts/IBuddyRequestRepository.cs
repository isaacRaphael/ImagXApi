using ImagX_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IBuddyRequestRepository : IGenericRepository<BuddyRequest>
    {
        Task<bool> ConfirmBuddyRequest(int id);
    }
    
}
