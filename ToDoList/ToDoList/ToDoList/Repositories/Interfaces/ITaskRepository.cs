using TaskEntity = ToDoList.Models.Task;

namespace ToDoList.Repositories.Interfaces
{
    public interface ITaskRepository : IBaseRepository<TaskEntity> 
    {
        Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(int userId);
        Task<IEnumerable<TaskEntity>> GetTasksByListIdAsync(int listId);
    }
}
