using Arch.EntityFrameworkCore.UnitOfWork;
using MyTodo.Api.Models;

namespace MyTodo.Api.Repositories
{
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
