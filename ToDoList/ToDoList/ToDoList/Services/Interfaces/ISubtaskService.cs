using ToDoList.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Services.Interfaces
{
    public interface ISubtaskService
    {
        Task CreateSubtaskAsync(int taskId, string title);
        Task ChangeTitleAsync(int id, string title);
        Task ChangeStatusAsync(int id);
        Task<IEnumerable<Subtask>> GetSubtasksByTaskId(int taskId);
        Task DeleteSubtaskAsync(int id);
    }
}
