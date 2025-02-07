using Moq;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services;
using TaskEntity = ToDoList.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldAddTaskWhenValid()
        {
            int listId = 1;
            int userId = 1;
            string title = "New Task";

            _taskRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<TaskEntity>()))
                .Returns(Task.CompletedTask);

            await _taskService.CreateTaskAsync(listId, userId, title);

            _taskRepositoryMock.Verify(repo => repo.AddAsync(It.Is<TaskEntity>(taskEntity =>
                taskEntity.ListId == listId &&
                taskEntity.UserId == userId &&
                taskEntity.Title == title)), Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldToggleCompletedStatus()
        {
            int taskId = 1;
            var taskEntity = new TaskEntity { Id = taskId, Completed = false };

            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync(taskEntity);
            _taskRepositoryMock
                .Setup(repo => repo.UpdateAsync(taskEntity))
                .Returns(Task.CompletedTask);

            await _taskService.ChangeStatusAsync(taskId);

            Assert.True(taskEntity.Completed);
            _taskRepositoryMock.Verify(repo => repo.UpdateAsync(taskEntity), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldRemoveTask()
        {
            int taskId = 1;
            _taskRepositoryMock
                .Setup(repo => repo.DeleteAsync(taskId))
                .Returns(Task.CompletedTask);

            await _taskService.DeleteTaskAsync(taskId);

            _taskRepositoryMock.Verify(repo => repo.DeleteAsync(taskId), Times.Once);
        }
    }
}