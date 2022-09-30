using DSS2022.Data.Repositories.Interfaces;
using DSS2022.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DSS2022DataContext context) : base(context)
        {
        }
    }
}
