using MyTodo.SharedLib.Parameters;

namespace MyTodo.Api.Services
{
    public interface IBaseService<T>
    {
        Task<ApiResponse> GetAllAsync(QueryParameter query);

        Task<ApiResponse> GetSingleAsync(int id);

        Task<ApiResponse> AddAsync(T model);

        Task<ApiResponse> UpdateAsync(T model);

        Task<ApiResponse> DeleteAsync(int id);
    }
}
