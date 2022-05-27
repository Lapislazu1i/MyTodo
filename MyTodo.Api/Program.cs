using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyTodo.Api.Models;
using MyTodo.Api.Profiles;
using MyTodo.Api.Repositories;
using MyTodo.Api.Services;

namespace MyTodo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            //builder.Services.AddControllers().AddNewtonsoftJson(o =>
            //{
            //    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //});


            // Add Db context and UnitOfWorks and Repositories
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");
                options.UseSqlite(connectionString);
            }).AddUnitOfWork<AppDbContext>()
              .AddCustomRepository<Todo, TodoRepository>()
              .AddCustomRepository<Memo, MemoRepository>()
              .AddCustomRepository<User, UserRepository>();


            // Add AutoMapper
            var automapperConfog = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });

            builder.Services.AddSingleton(automapperConfog.CreateMapper());

            //Add Cors
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("Cors", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            // Add services

            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IMemoService, MemoService>();
            builder.Services.AddScoped<IToDoService, TodoService>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseCors("Cors");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}