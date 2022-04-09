using Microsoft.EntityFrameworkCore;
using ToDoList.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.SqlRepositories
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<ToDoTask> ToDoTask { get; set; }
    }
}
