using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Services
{
    public class TaskTagService : ITaskTagService
    {

        private readonly ITaskTagRepository _tasktagRepository;

        public TaskTagService(ITaskTagRepository taskTagRepository)
        {
            _tasktagRepository = taskTagRepository;

        }
        public async Task AddTagAsync(int taskId, string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("Tag is empty");
            }

            Tasktag tasktag = new Tasktag
            {
                Tag = tag,
                TaskId = taskId
            };

            await _tasktagRepository.AddAsync(tasktag);
        }

        public async Task ChangeTagAsync(int id, string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException("Tag is empty");
            }

            Tasktag task = await _tasktagRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new ArgumentNullException("Task is not found");
            }
            
            task.Tag = tag;
            await _tasktagRepository.UpdateAsync(task);
        }

        public async Task DeleteTagAsync(int id)
        {
            await _tasktagRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Tasktag>> GetTagsAsync(int taskId)
        {
            return await _tasktagRepository.GetTagsByTaskIdAsync(taskId);
        }
    }
}
