using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TaskTagRepository : BaseRepository<Tasktag>, ITaskTagRepository
    {
        public TaskTagRepository(TododbContext context) : base(context) { }
    }
}
