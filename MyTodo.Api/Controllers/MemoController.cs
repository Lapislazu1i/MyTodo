using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyTodo.Api.Services;
using MyTodo.SharedLib.Dtos;
using MyTodo.SharedLib.Parameters;

namespace MyTodo.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemoController : ControllerBase
    {
        private readonly IMemoService _service;

        public MemoController(IMemoService memoService)
        {
            _service = memoService;
        }
        [HttpGet]
        public async Task<ApiResponse> Get([FromQuery]int id) => 
            await _service.GetSingleAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter param) =>
            await _service.GetAllAsync(param);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MemoDto param) =>
            await _service.AddAsync(param);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] MemoDto param) =>
            await _service.UpdateAsync(param);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) =>
            await _service.DeleteAsync(id);
    }
}
