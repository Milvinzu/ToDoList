using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class CRUDRepository
    {
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TododbContext context) : base(context) { }
    }

    public class ListRepository : Repository<List>, IListRepository
    {
        public ListRepository(TododbContext context) : base(context) { }
    }

    public class TaskRepository : Repository<ToDoList.Models.Task>, ITaskRepository
    {
        public TaskRepository(TododbContext context) : base(context) { }
    }

    public class TaskReminderRepository : Repository<Taskreminder>, ITaskReminderRepository
    {
        public TaskReminderRepository(TododbContext context) : base(context) { }
    }

    public class TaskTagRepository : Repository<Tasktag>, ITaskTagRepository
    {
        public TaskTagRepository(TododbContext context) : base(context) { }
    }

    public class SubtaskRepository : Repository<Subtask>, ISubtaskRepository
    {
        public SubtaskRepository(TododbContext context) : base(context) { }
    }

    public class TaskAttachmentRepository : Repository<Taskattachment>, ITaskAttachmentRepository
    {
        public TaskAttachmentRepository(TododbContext context) : base(context) { }
    }

    public class TaskCollaboratorRepository : Repository<Taskcollaborator>, ITaskCollaboratorRepository
    {
        public TaskCollaboratorRepository(TododbContext context) : base(context) { }
    }

    public class RecurringTaskRepository : Repository<Recurringtask>, IRecurringTaskRepository
    {
        public RecurringTaskRepository(TododbContext context) : base(context) { }
    }

    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(TododbContext context) : base(context) { }
    }

}
