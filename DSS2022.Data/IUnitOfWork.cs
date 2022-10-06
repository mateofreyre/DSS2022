using DSS2022.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Data
{
    public interface IUnitOfWork
    { 
        IUserRepository UserRepository { get; }
        ICollectionRepository CollectionRepository { get; }
        Task<int> Complete();

    }
}
