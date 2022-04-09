using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Contracts.Reposotories;
using ToDoList.Contracts.DataModels;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.SqlRepositories.SqlRepositories
{
    public class SqlTodoTask : ITodoTask
    {
        private readonly ToDoDbContext _context;
        public SqlTodoTask(ToDoDbContext context)
        {
            this._context = context;
        }

        public async Task<List<ToDoTask>> GetTasksAsync()
        {
            return await _context.ToDoTask
                .Include(todoTask => todoTask.AssignedTo).ToListAsync();
        }
        public async Task<List<ToDoTask>> GetTasksByUserAsync(int userID)
        {
            return await _context.ToDoTask.Where(t => t.AssignedTo.UserID == userID)
                 .Include(todoTask => todoTask.AssignedTo)
                .ToListAsync();
        }
        public async Task<ToDoTask> GetTaskByTaskIDAsync(int taskID)
        {
            return await _context.ToDoTask
                .Include(todoTask => todoTask.AssignedTo)
                .FirstOrDefaultAsync(t => t.TaskID == taskID);
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllStatus()
        {
            List<KeyValuePair<string, string>> statusList = new List<KeyValuePair<string, string>>();
            statusList.Add(new KeyValuePair<string, string>("Initiated", "Initiated"));
            statusList.Add(new KeyValuePair<string, string>("In Progress", "In Progress"));
            statusList.Add(new KeyValuePair<string, string>("Completed", "Completed"));
            return statusList;

        }

        public async Task<ToDoTask> UpdateTaskDetailsAsync(int taskID, ToDoTask task)
        {
            var existingTask = _context.ToDoTask.Where(t => t.TaskID == taskID).FirstOrDefault();
            if (existingTask != null)
            {
                existingTask.TaskName = task.TaskName;
                existingTask.DueDate = task.DueDate;
                existingTask.CompletedDate = task.CompletedDate;
                existingTask.Status = task.Status;
                existingTask.AssignedToUserID = task.AssignedToUserID;
                //_context.Entry(existingTask.AssignedTo).State = EntityState.Detached;
                //_context.Entry(existingTask.AssignedBy).State = EntityState.Detached;
                await _context.SaveChangesAsync();
                return existingTask;
            }
            return null;
        }
        public async Task<ToDoTask> AddNewTaskAsync(ToDoTask task)
        {
            var todoTask = await _context.ToDoTask.AddAsync(task);
            await _context.SaveChangesAsync();
            return todoTask.Entity;
        }
        public async Task<ToDoTask> DeleteTaskAsync(int taskID)
        {
            var toDoTask = _context.ToDoTask.Where(t => t.TaskID == taskID).FirstOrDefault();
            if (toDoTask != null)
            {
                _context.ToDoTask.Remove(toDoTask);
                await _context.SaveChangesAsync();
                return toDoTask;
            }
            return null;
        }
        public async Task<bool> IsTaskExistsAsync(int taskID)
        {
            var toDoTaskCount = _context.ToDoTask.Count(t => t.TaskID == taskID);
            if (toDoTaskCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
