namespace MyTodo.Api.Models
{
    public class Todo : BaseEntity
    {
        public string Title { get; set; }

        public string? Content { get; set; }

        public int Status { get; set; }
    }
}
