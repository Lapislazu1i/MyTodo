using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyTodo.Api.Models;
using MyTodo.SharedLib.Dtos;
using MyTodo.SharedLib.Parameters;

namespace MyTodo.Api.Services
{
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public MemoService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }
        public async Task<ApiResponse> GetAllAsync(QueryParameter query)
        {
            try
            {
                var repository = _work.GetRepository<Memo>();
                var memos = await repository.GetPagedListAsync(
                    predicate: v => string.IsNullOrWhiteSpace(query.Search) ? true : v.Title.Contains(query.Search),
                    pageIndex: query.PageIndex,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, memos);
            }
            catch(Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = _work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate:v => v.Id == id);
                return new ApiResponse(true, memo);
            }
            catch(Exception e)
            {
                return new ApiResponse(e.Message);
            }
        }

        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var memo = _mapper.Map<Memo>(model);
                await _work.GetRepository<Memo>().InsertAsync(memo);
                if(await _work.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, memo);
                }
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            try
            {
                var memo = _mapper.Map<Memo>(model);
                var repository = _work.GetRepository<Memo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: v => v.Id.Equals(model.Id));
                
                todo.Title = model.Title;
                todo.Content = model.Content;
                todo.UpdateDate = DateTime.Now;
                repository.Update(todo);

                if (await _work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);


                return new ApiResponse("更新数据失败");
            }
            catch(Exception e)
            {
                return new ApiResponse("更新数据失败");

            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = _work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate:v=>v.Id.Equals(id));
                repository.Delete(memo);
                if (await _work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

    }
}
