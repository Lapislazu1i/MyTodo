using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyTodo.Api.Models;
using MyTodo.SharedLib;
using MyTodo.SharedLib.Dtos;

namespace MyTodo.Api.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public LoginService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(string account, string password)
        {
            try
            {
                password = password.GetMD5();
                var repository = _work.GetRepository<User>();
                var modal = await repository.GetFirstOrDefaultAsync(predicate: v => v.Account.Equals(account) && v.PassWord.Equals(password));
                if (modal == null)
                    return new ApiResponse("账号或密码错误");

                return new ApiResponse(true, new UserDto
                {
                    Account = modal.Account,
                    UserName = modal.UserName,
                    Id = modal.Id
                });
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, "登录失败！");
            }
        }

        public async Task<ApiResponse> Resgiter(UserDto user)
        {         
            try
            {
                var model = _mapper.Map<User>(user);
                var repository = _work.GetRepository<User>();
                var userModel = await repository.GetFirstOrDefaultAsync(predicate: v => v.Account.Equals(model.Account));
                if(userModel != null)
                {
                    return new ApiResponse($"当前账号:{model.Account}已存在,请重新注册！");
                }
                model.CreateDate = DateTime.Now;
                model.PassWord = model.PassWord.GetMD5();
                await repository.InsertAsync(model);

                if (await _work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);

                return new ApiResponse("注册失败");
            }
            catch(Exception ex)
            {
                return new ApiResponse("注册失败");
            }
        }
    }
}
