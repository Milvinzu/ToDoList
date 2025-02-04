using ToDoList.Models;

namespace ToDoList.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User> { }
    public interface IListRepository : IRepository<List> { }
    public interface ITaskRepository : IRepository<ToDoList.Models.Task> { }
    public interface ITaskReminderRepository : IRepository<Taskreminder> { }
    public interface ITaskTagRepository : IRepository<Tasktag> { }
    public interface ISubtaskRepository : IRepository<Subtask> { }
    public interface ITaskAttachmentRepository : IRepository<Taskattachment> { }
    public interface ITaskCollaboratorRepository : IRepository<Taskcollaborator> { }
    public interface IRecurringTaskRepository : IRepository<Recurringtask> { }
    public interface INotificationRepository : IRepository<Notification> { }

}
