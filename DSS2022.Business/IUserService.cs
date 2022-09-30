using DSS2022.DataTransferObjects.User;
using DSS2022.Model;

namespace DSS2022.Business
{
    public interface IUserService
    {
        User GetById(int id);
        Task<User> GetByIdAsync(int id);
        Task<UserDTO> Create(CreateUserDTO dto);

    }
}
