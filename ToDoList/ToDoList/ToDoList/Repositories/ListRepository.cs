using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class ListRepository : BaseRepository<List>, IListRepository
    {
        public ListRepository(TododbContext context) : base(context) { }
    }
}
