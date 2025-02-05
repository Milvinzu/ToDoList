using ToDoList.Models;

namespace ToDoList.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User> 
    {
        public Task<User> GetByEmailAsync(string email);
    }

}
