using ToDoList.Models;

namespace ToDoList.Repositories.Interfaces
{
    public interface ITaskTagRepository : IBaseRepository<Tasktag> 
    {
        Task<IEnumerable<Tasktag>> GetTagsByTaskIdAsync(int taskID);
    }
}
