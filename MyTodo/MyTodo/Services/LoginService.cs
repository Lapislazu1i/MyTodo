using MyTodo.SharedLib;
using MyTodo.SharedLib.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpRestClient _client;
        private readonly string _serviceName = "Login";

        public LoginService(HttpRestClient client)
        {
            _client = client;
        }
        public async Task<ApiResponse> Login(UserDto user)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{_serviceName}/Login";
            request.Parameter = user;
            return await _client.ExecuteAsync(request);
        }

        public async Task<ApiResponse> Resgiter(UserDto user)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{_serviceName}/Resgiter";
            request.Parameter = user;
            return await _client.ExecuteAsync(request);
        }
    }
}
