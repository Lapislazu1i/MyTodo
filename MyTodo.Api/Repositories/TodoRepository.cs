using Arch.EntityFrameworkCore.UnitOfWork;
using MyTodo.Api.Models;

namespace MyTodo.Api.Repositories
{
    public class TodoRepository : Repository<Todo>, IRepository<Todo>
    {
        public TodoRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
