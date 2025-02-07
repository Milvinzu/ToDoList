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

        [Fact]
        public async Task SetTimeRemindAsync_ShouldUpdateReminderTime_WhenTaskExists()
        {
            int taskId = 1;
            DateTime newRemindTime = DateTime.UtcNow.AddHours(1);
            var taskEntity = new TaskEntity { Id = taskId, ReminderTime = null };

            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync(taskEntity);
            _taskRepositoryMock
                .Setup(repo => repo.UpdateAsync(taskEntity))
                .Returns(Task.CompletedTask);

            await _taskService.SetTimeRemindAsync(taskId, newRemindTime);

            Assert.Equal(newRemindTime, taskEntity.ReminderTime);
            _taskRepositoryMock.Verify(repo => repo.UpdateAsync(taskEntity), Times.Once);
        }

        [Fact]
        public async Task SetTimeRemindAsync_ShouldThrowArgumentException_WhenTaskNotFound()
        {
            int taskId = 99;
            DateTime newRemindTime = DateTime.UtcNow.AddHours(1);

            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync((TaskEntity)null);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _taskService.SetTimeRemindAsync(taskId, newRemindTime));
        }

        [Fact]
        public async Task SetTimeRemindAsync_ShouldThrowException_WhenRemindTimeIsInThePast()
        {
            int taskId = 1;
            DateTime pastTime = DateTime.UtcNow.AddHours(-1);
            var taskEntity = new TaskEntity { Id = taskId, ReminderTime = null };

            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync(taskEntity);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _taskService.SetTimeRemindAsync(taskId, pastTime));
        }

        [Fact]
        public async Task OffRemindAsync_ShouldSetReminderTimeToNull_WhenTaskExists()
        {
            int taskId = 1;
            var existingTask = new TaskEntity
            {
                Id = taskId,
                ReminderTime = DateTime.UtcNow.AddHours(1)
            };

            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync(existingTask);
            _taskRepositoryMock
                .Setup(repo => repo.UpdateAsync(existingTask))
                .Returns(Task.CompletedTask);

            await _taskService.OffRemindAsync(taskId);

            Assert.Null(existingTask.ReminderTime);
            _taskRepositoryMock.Verify(repo => repo.UpdateAsync(existingTask), Times.Once);
        }

        [Fact]
        public async Task OffRemindAsync_ShouldThrowArgumentException_WhenTaskNotFound()
        {
            int taskId = 99;
            _taskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync((TaskEntity)null);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _taskService.OffRemindAsync(taskId));
        }
    }
}