using ImagX_API.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IUserRepository 
    {
        Task<ICollection<AppUser>> GetAll();
        Task<ICollection<AppUser>> GetBuddies(string id);
        Task<AppUser> GetById(string id);
        Task<ICollection<AppUser>> GetPaginated(int page = 1, int pageSize = 10);
        Task<AppUser> Add(AppUser entity);
        Task<bool> Update(JsonPatchDocument entity, string id);
        Task<bool> Remove(string id);
        Task<bool> Exists(string id);
        Task<bool> Hasfriend(string senderId, string receipientId);
        
    }
    
}
