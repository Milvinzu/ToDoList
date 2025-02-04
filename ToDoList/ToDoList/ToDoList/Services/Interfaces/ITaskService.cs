namespace ToDoList.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<Models.Task>> GetAllTaskASync();
        Task<Models.Task> GetTaskByIdAsync(int id);
        Task AddTaskAsync(Models.Task task);
        Task UpdateTaskAsync(Models.Task task);
        Task DeleteTaskAsync(int id);
    }
}
