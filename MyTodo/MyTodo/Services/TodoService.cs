using MyTodo.SharedLib;
using MyTodo.SharedLib.Dtos;
using MyTodo.SharedLib.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodo.Services
{
    internal class TodoService : BaseService<TodoDto>, ITodoService
    {
        private HttpRestClient _client;
        public TodoService(HttpRestClient client) : base(client, "Todo")
        {
            _client = client;
        }

        public async Task<ApiResponse<PagedList<TodoDto>>> GetAllFilterAsync(TodoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/Todo/GetAll?pageIndex={parameter.PageIndex}" +
                $"&PageSize={parameter.PageSize}" +
                $"&Search={parameter.Search}" +
                $"&Status={parameter.Status}";
            return await _client.ExecuteAsync<PagedList<TodoDto>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> SummaryAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Route = "api/Todo/Summary";
            return await _client.ExecuteAsync<SummaryDto>(request);
        }
    }
}
