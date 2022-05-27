using MyTodo.SharedLib.Dtos;
using MyTodo.SharedLib.Parameters;

namespace MyTodo.Api.Services
{
    public interface IToDoService : IBaseService<TodoDto>
    {
        Task<ApiResponse> GetAllAsync(TodoParameter query);

        Task<ApiResponse> Summary();
    }
}
