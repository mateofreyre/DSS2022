using DSS2022.Data.Repositories;
using DSS2022.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DSS2022DataContext _context;

        public UnitOfWork(DSS2022DataContext dSS2022DataContext)
        {
            _context = dSS2022DataContext;
            UserRepository = new UserRepository(dSS2022DataContext);
            CollectionRepository = new CollectionRepository(dSS2022DataContext);

        }

        public IUserRepository UserRepository { get; private set; }
        public ICollectionRepository CollectionRepository { get; private set; }

    }
}
