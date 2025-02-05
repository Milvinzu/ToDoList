using ToDoList.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User> 
    {
        public Task<User> GetByEmailAsync(string email);
        public Task ChangeNameAsync(int id, string newName);
        public Task ChangePasswordAsync(int Id, string newPasswordHash);
    }

}
