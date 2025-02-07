using ToDoList.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Services.Interfaces
{
    public interface ITaskTagService
    {
        Task AddTagAsync(int taskId, string tag);
        Task ChangeTagAsync(int id, string tag);
        Task<IEnumerable<Tasktag>> GetTagsAsync(int taskId);
        Task DeleteTagAsync(int id);
    }
}
