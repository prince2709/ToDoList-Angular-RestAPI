using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Contracts.DataModels;

namespace ToDoList.Contracts.Reposotories
{
    public interface IUser
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIDAsync(int userID);
        Task<User> UpdateUserDetailsAsync(int userID, User user);
        Task<User> AddNewUserAsync(User user);
        Task<User> DeleteUserAsync(int userID);
        Task<bool> IsUserExistsAsync(int userID);
    }
}
