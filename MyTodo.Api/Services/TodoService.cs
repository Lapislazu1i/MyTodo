using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyTodo.Api.Models;
using MyTodo.SharedLib.Dtos;
using MyTodo.SharedLib.Parameters;
using System.Collections.ObjectModel;

namespace MyTodo.Api.Services
{
    public class TodoService : IToDoService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public TodoService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(TodoDto model)
        {
            try
            {
                var todo = _mapper.Map<Todo>(model);
                todo.CreateDate = DateTime.Now;
                await _work.GetRepository<Todo>().InsertAsync(todo);
                if (await _work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = _work.GetRepository<Todo>();
                var todo = await repository
                    .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(todo);
                if (await _work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(TodoParameter query)
        {
            try
            {
                var repository = _work.GetRepository<Todo>();
                var todos = await repository.GetPagedListAsync(predicate:
                    x => ((string.IsNullOrWhiteSpace(query.Search) || string.IsNullOrEmpty(query.Search)) ? true : x.Title.Contains(query.Search))
                   && (query.Status == null ? true : x.Status.Equals(query.Status)),
                   pageIndex: query.PageIndex,
                   pageSize: query.PageSize,
                   orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter query)
        {
            try
            {
                var repository = _work.GetRepository<Todo>();
                var todos = await repository.GetPagedListAsync(predicate:
                   x => (string.IsNullOrWhiteSpace(query.Search) || string.IsNullOrEmpty(query.Search)) ? true : x.Title.Contains(query.Search),
                   pageIndex: query.PageIndex,
                   pageSize: query.PageSize,
                   orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = _work.GetRepository<Todo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, todo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> Summary()
        {
            try
            {
                //待办事项结果
                var todos = _work.GetRepository<Todo>().GetAll().OrderByDescending(v => v.CreateDate);

                //备忘录结果
                var memos = _work.GetRepository<Memo>().GetAll().OrderByDescending(v => v.CreateDate);
                SummaryDto summary = new SummaryDto();
                summary.Sum = todos.Count(); //汇总待办事项数量
                summary.CompletedCount = todos.Where(t => t.Status == 1).Count(); //统计完成数量
                summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%"); //统计完成率
                summary.MemoeCount = memos.Count();  //汇总备忘录数量
                summary.TodoList = new ObservableCollection<TodoDto>(_mapper.Map<List<TodoDto>>(todos));
                summary.MemoList = new ObservableCollection<MemoDto>(_mapper.Map<List<MemoDto>>(memos));

                return new ApiResponse(true, summary);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(TodoDto model)
        {
            try
            {
                var dbToDo = _mapper.Map<Todo>(model);
                var repository = _work.GetRepository<Todo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbToDo.Id));

                todo.Title = dbToDo.Title;
                todo.Content = dbToDo.Content;
                todo.Status = dbToDo.Status;
                todo.UpdateDate = DateTime.Now;

                repository.Update(todo);

                if (await _work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse("更新数据异常！");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
