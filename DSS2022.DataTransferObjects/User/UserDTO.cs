using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.DataTransferObjects.User
{
    public class UserDTO : BaseEntityDTO
    {
        //public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
