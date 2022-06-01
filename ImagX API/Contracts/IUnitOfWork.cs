using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; set; }

    }
}
