using ToDoList.DTO;
using ToDoList.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(string email, string password);
        Task<string> LoginUserAsync(string email, string password);

    }
}
