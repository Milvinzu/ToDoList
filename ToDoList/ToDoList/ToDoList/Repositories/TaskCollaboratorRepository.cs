using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TaskCollaboratorRepository : BaseRepository<Taskcollaborator>, ITaskCollaboratorRepository
    {
        public TaskCollaboratorRepository(TododbContext context) : base(context) { }
    }
}
