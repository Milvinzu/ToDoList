namespace ToDoList.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<Models.Task>> GetAllTaskFromList(int listId);
        Task<IEnumerable<Models.Task>> GetAllTaskByUser(int userId);
        Task CreateTaskAsync(int listId, int userId, string title);
        Task SetDeadlineTimeAsync(int id, DateTime dateTime);
        Task UpdateDescriptionAsync(int id, string description);
        Task ChangeStatusAsync(int id);
        Task ChangeTitleAsync(int id,string title);
        Task DeleteTaskAsync(int id);
    }
}
