using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Repositories.Interfaces;
using TaskEntity = ToDoList.Models.Task;

namespace ToDoList.Repositories
{
    public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(TododbContext context) : base(context) { }

        public async Task<IEnumerable<TaskEntity>> GetTasksByListIdAsync(int listId)
        {
            return await _context.Tasks.Where(task => task.ListId == listId).ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks.Where(task => task.UserId == userId).ToListAsync();
        }
    }
}
