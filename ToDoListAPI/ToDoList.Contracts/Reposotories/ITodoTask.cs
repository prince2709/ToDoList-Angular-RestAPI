using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Contracts.DataModels;

namespace ToDoList.Contracts.Reposotories
{
    public interface ITodoTask
    {
        Task<List<ToDoTask>> GetTasksAsync();
        Task<List<ToDoTask>> GetTasksByUserAsync(int usedID);
        Task<ToDoTask> GetTaskByTaskIDAsync(int taskID);
        Task<List<KeyValuePair<string,string>>> GetAllStatus();
        Task<ToDoTask> UpdateTaskDetailsAsync(int taskID,ToDoTask task);
        Task<ToDoTask> AddNewTaskAsync(ToDoTask task);
        Task<ToDoTask> DeleteTaskAsync(int taskID);
        Task<bool> IsTaskExistsAsync(int taskID);
    }
}
