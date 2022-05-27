using AutoMapper;
using MyTodo.Api.Models;
using MyTodo.SharedLib.Dtos;

namespace MyTodo.Api.Profiles
{
    public class AutoMapperProfile : MapperConfigurationExpression
    {
        public AutoMapperProfile()
        {
            CreateMap<Todo, TodoDto>().ReverseMap();
            CreateMap<Memo, MemoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
