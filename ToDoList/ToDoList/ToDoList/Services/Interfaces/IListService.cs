using ToDoList.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Services.Interfaces
{
    public interface IListService
    {
        Task CreateListAsync(int userId, string name, string color);
        Task ChangeColorAndNameAsync(int id, string color, string name);
        Task DeleteListAsync(int id);
        Task<List> GetAllListByUserAsync(int userId);
    }
}
