using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTodo.Api.Services;
using MyTodo.SharedLib.Dtos;

namespace MyTodo.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService loginService)
        {
            _service = loginService;
        }
        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto param) => 
            await _service.LoginAsync(param.Account, param.PassWord);
        [HttpPost]
        public async Task<ApiResponse> Resgister([FromBody] UserDto param) =>
            await _service.Resgiter(param);
    }
}
