using ImagX_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage>
    {
    }
}
