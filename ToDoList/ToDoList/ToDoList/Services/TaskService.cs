using ToDoList.Data;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services.Interfaces;
using TaskEntity = ToDoList.Models.Task;

namespace ToDoList.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task CreateTaskAsync(int listId, int userId, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Task title cannot be empty.");
            }

            TaskEntity newTask = new TaskEntity()
            {
                ListId = listId,
                UserId = userId,
                Title = title,
                Completed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(newTask);
        }

        public async Task ChangeStatusAsync(int id)
        {
            TaskEntity task = await _taskRepository.GetByIdAsync(id);

            if(task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.Completed = !task.Completed;
            await _taskRepository.UpdateAsync(task);
        }

        public async Task ChangeTitleAsync(int id, string title)
        {
            TaskEntity task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.Title = title;
            await _taskRepository.UpdateAsync(task);
        }

        public async Task SetDeadlineTimeAsync(int id, DateTime dateTime)
        {
            TaskEntity task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.DueDate = dateTime;
            await _taskRepository.UpdateAsync(task);
        }

        public async Task UpdateDescriptionAsync(int id, string description)
        {
            TaskEntity task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.Description = description;
            await _taskRepository.UpdateAsync(task);
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTaskByUser(int userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTaskFromList(int listId)
        {
            return await _taskRepository.GetTasksByListIdAsync(listId);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }
    }
}
