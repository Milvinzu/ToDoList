using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TaskTagRepository : BaseRepository<Tasktag>, ITaskTagRepository
    {
        public TaskTagRepository(TododbContext context) : base(context) { }

        public async Task<IEnumerable<Tasktag>> GetTagsByTaskIdAsync(int taskID)
        {
            return await _context.Tasktags.Where(tag => tag.TaskId == taskID).ToListAsync();
        }
    }
}
