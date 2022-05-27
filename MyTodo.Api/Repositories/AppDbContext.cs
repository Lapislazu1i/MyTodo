using Microsoft.EntityFrameworkCore;
using MyTodo.Api.Models;

namespace MyTodo.Api.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Memo> Memos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
