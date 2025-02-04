using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class RecurringTaskRepository : BaseRepository<Recurringtask>, IRecurringTaskRepository
    {
        public RecurringTaskRepository(TododbContext context) : base(context) { }
    }
}
