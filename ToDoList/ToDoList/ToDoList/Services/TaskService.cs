using ToDoList.Data;
using ToDoList.Services.Interfaces;

namespace ToDoList.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskService _taskService;

        public TaskService(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTaskASync()
        {
            return await _taskService.GetAllTaskASync();
        }

        public async System.Threading.Tasks.Task AddTaskAsync(Models.Task task)
        {
            await _taskService.AddTaskAsync(task);
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            await _taskService.DeleteTaskAsync(id);
        }


        public async Task<Models.Task> GetTaskByIdAsync(int id)
        {
            return await _taskService.GetTaskByIdAsync(id);
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task)
        {
            await _taskService.UpdateTaskAsync(task);
        }
    }

    
    
}
