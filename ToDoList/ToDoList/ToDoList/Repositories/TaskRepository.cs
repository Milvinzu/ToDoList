using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TododbContext _context;

        public TaskRepository(TododbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTaskAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Models.Task> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task UpdateTaskAsync(Models.Task task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task AddTaskAsync(Models.Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null) 
            { 
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
