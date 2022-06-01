using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<ICollection<T>> GetAll();
        Task<T> GetById(int id);
        Task<ICollection<T>> GetPaginated(int page = 1, int pageSize = 10);
        Task<T> Add(T entity);
        Task<bool> Update(JsonPatchDocument entity, int id);
        Task<bool> Remove(int id);
        Task<bool> Exists(int id);

    }
    
}
