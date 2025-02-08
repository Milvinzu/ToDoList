using ToDoList.Models;

namespace ToDoList.Repositories.Interfaces
{
    public interface ISubtaskRepository : IBaseRepository<Subtask> 
    {
        Task<IEnumerable<Subtask>> GetSubtasksByTaskIdAsync(int  taskId);
    }
}
