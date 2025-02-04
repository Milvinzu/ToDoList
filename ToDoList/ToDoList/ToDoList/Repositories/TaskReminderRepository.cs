using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TaskReminderRepository : BaseRepository<Taskreminder>, ITaskReminderRepository
    {
        public TaskReminderRepository(TododbContext context) : base(context) { }
    }
}
