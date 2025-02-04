using ToDoList.Data;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TaskRepository : BaseRepository<ToDoList.Models.Task>, ITaskRepository
    {
        public TaskRepository(TododbContext context) : base(context) { }
    }
}
