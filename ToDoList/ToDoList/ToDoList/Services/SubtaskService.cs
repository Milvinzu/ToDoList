using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Services
{
    public class SubtaskService : ISubtaskService
    {
        private readonly ISubtaskRepository _subtaskRepository;

        public SubtaskService(ISubtaskRepository subtaskRepository)
        {
            _subtaskRepository = subtaskRepository;
        }

        public async Task CreateSubtaskAsync(int taskId, string title)
        {
            if(string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title empty");
            }

            Subtask subtask = new Subtask
            {
                TaskId = taskId,
                Title = title,
                Completed = false
            };
            await _subtaskRepository.AddAsync(subtask);
        }

        public async Task ChangeStatusAsync(int id)
        {
            Subtask subtask = await _subtaskRepository.GetByIdAsync(id);
            if(subtask == null)
            {
                throw new ArgumentException("Subtask not found");
            }

            subtask.Completed = !subtask.Completed;
            await _subtaskRepository.UpdateAsync(subtask);
        }

        public async Task ChangeTitleAsync(int id, string title)
        {
            if(string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title empty");
            }

            Subtask subtask = await _subtaskRepository.GetByIdAsync(id);
            if (subtask == null)
            {
                throw new ArgumentException("Subtask not found");
            }

            subtask.Title = title;
            await _subtaskRepository.UpdateAsync(subtask);
        }

        public async Task<IEnumerable<Models.Subtask>> GetSubtasksByTaskId(int taskId)
        {
            return await _subtaskRepository.GetSubtasksByTaskIdAsync(taskId);
        }

        public async Task DeleteSubtaskAsync(int id)
        {
            await _subtaskRepository.DeleteAsync(id);
        }
    }
}
