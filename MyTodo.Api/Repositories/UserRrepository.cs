using Arch.EntityFrameworkCore.UnitOfWork;
using MyTodo.Api.Models;

namespace MyTodo.Api.Repositories
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
