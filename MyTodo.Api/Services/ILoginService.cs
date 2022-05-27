namespace MyTodo.Api.Services
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync(string account, string password);

        Task<ApiResponse> Resgiter(SharedLib.Dtos.UserDto user);
    }
}
