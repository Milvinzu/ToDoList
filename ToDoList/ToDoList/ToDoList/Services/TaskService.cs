using ToDoList.Data;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services.Interfaces;

namespace ToDoList.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }

        public async System.Threading.Tasks.Task CreateTaskAsync(Models.Task task)
        {
            if (!string.IsNullOrWhiteSpace(task.Title))
            {
                await _taskRepository.AddTaskAsync(task);
            }
            else
            {
                throw new ArgumentException("Task title cannot be empty.");
            }
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }


        public async Task<Models.Task> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task)
        {
            await _taskRepository.UpdateTaskAsync(task);
        }
    }

    
    
}
