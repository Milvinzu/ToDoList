using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(TododbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task ChangeNameAsync(int Id, string newName)
        {
            User user = await GetByIdAsync(Id);
            user.Name = newName;
            await UpdateAsync(user);
        }
        public async Task ChangePasswordAsync(int Id, string newPasswordHash)
        {
            User user = await GetByIdAsync(Id);
            user.PasswordHash = newPasswordHash;
            await UpdateAsync(user);
        }
    }
}
