using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class SubtaskRepository : BaseRepository<Subtask>, ISubtaskRepository
    {
        public SubtaskRepository(TododbContext context) : base(context) { }

        public async Task<IEnumerable<Subtask>> GetSubtasksByTaskIdAsync(int taskId)
        {
            return await _context.Subtasks.Where(subtask => subtask.TaskId == taskId).ToListAsync();
        }
    }
}
