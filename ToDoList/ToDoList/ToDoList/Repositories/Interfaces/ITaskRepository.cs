using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Numerics;
namespace ToDoList.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Models.Task>> GetAllTaskASync();
        Task<Models.Task> GetTaskByIdAsync(int id);
        Task AddTaskAsync(Models.Task task);
        Task UpdateTaskAsync(Models.Task task);
        Task DeleteTaskAsync(int id);
    }
}
