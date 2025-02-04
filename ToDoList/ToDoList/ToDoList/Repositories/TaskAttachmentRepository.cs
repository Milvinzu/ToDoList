using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class TaskAttachmentRepository : BaseRepository<Taskattachment>, ITaskAttachmentRepository
    {
        public TaskAttachmentRepository(TododbContext context) : base(context) { }
    }
}
