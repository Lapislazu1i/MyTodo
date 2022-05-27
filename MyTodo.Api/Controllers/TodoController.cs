using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTodo.Api.Services;
using MyTodo.SharedLib.Dtos;
using MyTodo.SharedLib.Parameters;

namespace MyTodo.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IToDoService _service;

        public TodoController(IToDoService todoService)
        {
            _service = todoService;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id) =>
            await _service.GetSingleAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] TodoParameter param)=>
            await _service.GetAllAsync(param);

        [HttpGet]
        public async Task<ApiResponse> Summary() => await _service.Summary();

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] TodoDto param) =>
            await _service.AddAsync(param);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] TodoDto param) =>
            await _service.UpdateAsync(param);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => 
            await _service.DeleteAsync(id);


    }
}
