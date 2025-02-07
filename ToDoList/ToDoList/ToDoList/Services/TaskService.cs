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

        public async System.Threading.Tasks.Task CreateTaskAsync(int listId, int userId, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Task title cannot be empty.");
            }

            Models.Task newTask = new Models.Task()
            {
                ListId = listId,
                UserId = userId,
                Title = title,
                Completed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(newTask);
        }

        public async System.Threading.Tasks.Task ChangeStatusAsync(int id)
        {
            Models.Task task = await _taskRepository.GetByIdAsync(id);

            if(task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.Completed = !task.Completed;
            await _taskRepository.UpdateAsync(task);
        }

        public async System.Threading.Tasks.Task ChangeTitleAsync(int id, string title)
        {
            Models.Task task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.Title = title;
            await _taskRepository.UpdateAsync(task);
        }

        public async System.Threading.Tasks.Task SetDeadlineTimeAsync(int id, DateTime dateTime)
        {
            Models.Task task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.DueDate = dateTime;
            await _taskRepository.UpdateAsync(task);
        }

        public async System.Threading.Tasks.Task UpdateDescriptionAsync(int id, string description)
        {
            Models.Task task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            task.Description = description;
            await _taskRepository.UpdateAsync(task);
        }

        public async Task<IEnumerable<Models.Task>> GetAllTaskByUser(int userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Models.Task>> GetAllTaskFromList(int listId)
        {
            return await _taskRepository.GetTasksByListIdAsync(listId);
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }
    }
}
