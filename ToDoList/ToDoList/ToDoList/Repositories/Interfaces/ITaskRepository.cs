namespace ToDoList.Repositories.Interfaces
{
    public interface ITaskRepository : IBaseRepository<ToDoList.Models.Task> 
    {
        Task<IEnumerable<Models.Task>> GetTasksByUserIdAsync(int userId);
        Task<IEnumerable<Models.Task>> GetTasksByListIdAsync(int listId);
    }
}
