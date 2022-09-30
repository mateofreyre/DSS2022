using DSS2022.Business;
using Microsoft.AspNetCore.Mvc;

namespace DSS2022.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await this._userService.GetByIdAsync(id);
            return Ok(user);
        }

    }
}
