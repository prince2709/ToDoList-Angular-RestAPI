using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.SqlRepositories;
using ToDoList.Contracts.Reposotories;
using ToDoList.SqlRepositories.SqlRepositories;

namespace ToDoListAPI
{
    public class Startup
    {
        readonly string ToDoListUIAllowOrgins = "ToDoListUI";
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            
            services.AddControllers();
           
            //Sql Server Database
            //services.AddDbContext<ToDoDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("ToDoListPortalDb")));

            //In Memory Database
            services.AddDbContext<ToDoDbContext>(options =>
            options.UseInMemoryDatabase("ToDoListPortalDb"));

            services.AddScoped<ITodoTask, SqlTodoTask>();
            services.AddScoped<IUser, SqlUser>();

            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var context = serviceProvider.GetService<ToDoDbContext>();
            AddSeedData(context);

            //app.UseCors((options) =>
            //{
            //    options.AddPolicy(ToDoListUIAllowOrgins, (policy) =>
            //     {
            //         policy.WithOrigins("http://localhost:4200")
            //         .AllowAnyHeader()
            //         .WithMethods("GET", "POST", "PUT", "DELETE")
            //         .WithExposedHeaders("*");
            //     });
            //});
            app.UseCors(options => options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           );

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddSeedData(ToDoDbContext context)
        {
            var user = new ToDoList.Contracts.DataModels.User
            {
                EmailAddress = "Prince1@mailbox.com"
            ,
                FirstName = "Prince"
            ,
                LastName = "Thomas"
            
               // UserID = 1
            };
            context.User.Add(user);
            var user1 = new ToDoList.Contracts.DataModels.User
            {
                EmailAddress = "Test@mailbox.com"
            ,
                FirstName = "Test"
            ,
                LastName = "User"
            
               // UserID = 2
            };
            context.User.Add(user1);

            var task1 = new ToDoList.Contracts.DataModels.ToDoTask
            {
                DueDate = new DateTime(2022, 12, 1),
                Status = "Initiated",
                TaskName = "Test Task 1"
                //TaskID = 1
            };
            context.ToDoTask.Add(task1);
            var task2 = new ToDoList.Contracts.DataModels.ToDoTask
            {
                DueDate = new DateTime(2022, 12, 10),
                Status = "Initiated",
                TaskName = "Test Task 2"
                //TaskID = 2
            };
            context.ToDoTask.Add(task2);
            context.SaveChanges();
        }
    }
}
