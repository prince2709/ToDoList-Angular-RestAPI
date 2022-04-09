using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Contracts.DataModels;
using ToDoList.Contracts.Reposotories;

namespace ToDoList.SqlRepositories.SqlRepositories
{
    public class SqlUser : IUser
    {
        private readonly ToDoDbContext _context;
        public SqlUser(ToDoDbContext context)
        {
            this._context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> GetUserByIDAsync(int userID)
        {
            return await _context.User.FirstOrDefaultAsync(t => t.UserID == userID);
        }

        public async Task<User> UpdateUserDetailsAsync(int userID, User user)
        {
            var existingUser = _context.User.Where(t => t.UserID == userID).FirstOrDefault();
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.EmailAddress = user.EmailAddress;
                await _context.SaveChangesAsync();
                return existingUser;
            }
            return null;
        }

        public async Task<User> AddNewUserAsync(User user)
        {
            var userEntity = await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return userEntity.Entity;
        }

        public async Task<User> DeleteUserAsync(int userID)
        {
            var user = _context.User.Where(t => t.UserID == userID).FirstOrDefault();
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public async Task<bool> IsUserExistsAsync(int userID)
        {
            var userCount = _context.User.Count(t => t.UserID == userID);
            if (userCount > 0)
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
