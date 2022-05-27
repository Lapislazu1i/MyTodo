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
    public interface ITodoService : IBaseService<TodoDto>
    {
        public  Task<ApiResponse<PagedList<TodoDto>>> GetAllFilterAsync(TodoParameter parameter);

        public  Task<ApiResponse<SummaryDto>> SummaryAsync();
    }
}
