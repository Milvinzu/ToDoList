using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class SubtaskRepository : BaseRepository<Subtask>, ISubtaskRepository
    {
        public SubtaskRepository(TododbContext context) : base(context) { }
    }
}
