using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TaskRepository : BaseRepository<ToDoList.Models.Task>, ITaskRepository
    {
        public TaskRepository(TododbContext context) : base(context) { }

        public async Task<IEnumerable<Models.Task>> GetTasksByListIdAsync(int listId)
        {
            return await _context.Tasks.Where(task => task.ListId == listId).ToListAsync();
        }

        public async Task<IEnumerable<Models.Task>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.Tasks.Where(task => task.UserId == userId).ToListAsync();
        }
    }
}
