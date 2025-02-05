using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services;
using ToDoList.Services.Interfaces;

namespace ToDoListTest.Services
{
    public class TaskServiceTests
    {
        private readonly ITaskService _taskService;
        private readonly Mock<ITaskRepository> _mockTaskRepository;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _taskService = new TaskService(_mockTaskRepository.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            var expectedTasks = new List<ToDoList.Models.Task>
            {
                new ToDoList.Models.Task
                {
                    Id = 1,
                    Title = "Task 1",
                    ListId = 1, UserId = 1,
                    DueDate = DateTime.Now.AddDays(1)
                },
                new ToDoList.Models.Task
                {
                    Id = 2,
                    Title = "Task 2",
                    ListId = 1,
                    UserId = 1,
                    DueDate = DateTime.Now.AddDays(2)
                }
            };

            _mockTaskRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedTasks);

            var result = await _taskService.GetAllTasksAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Title == "Task 1");
            Assert.Contains(result, t => t.Title == "Task 2");
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_ShouldReturnTaskWhenFound()
        {
            var task = new ToDoList.Models.Task
            {
                Id = 3,
                Title = "Task 3",
                ListId = 1,
                UserId = 1,
                DueDate = DateTime.Now.AddDays(1)
            };

            _mockTaskRepository
                .Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync(task);

            var result = await _taskService.GetTaskByIdAsync(3);

            Assert.NotNull(result);
            Assert.Equal("Task 3", result.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_ShouldCallAddTaskAsyncWhenTaskIsValid()
        {
            var newTask = new ToDoList.Models.Task
            {
                Id = 4,
                Title = "Valid Task",
                ListId = 1,
                UserId = 1,
                DueDate = DateTime.Now.AddDays(1)
            };

            _mockTaskRepository
                .Setup(repo => repo.AddAsync(newTask))
                .Returns(System.Threading.Tasks.Task.CompletedTask)
                .Verifiable();

            await _taskService.CreateTaskAsync(newTask);

            _mockTaskRepository.Verify(repo => repo.AddAsync(newTask), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_ShouldThrowExceptionWhenTitleIsEmpty()
        {
            var newTask = new ToDoList.Models.Task
            {
                Id = 5,
                Title = "   ",
                ListId = 1,
                UserId = 1,
                DueDate = DateTime.Now.AddDays(1)
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(newTask));
            Assert.Equal("Task title cannot be empty.", exception.Message);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskAsync_ShouldCallUpdateTaskAsyncOnRepository()
        {
            var existingTask = new ToDoList.Models.Task
            {
                Id = 6,
                Title = "Old Title",
                ListId = 1,
                UserId = 1,
                DueDate = DateTime.Now.AddDays(1)
            };

            _mockTaskRepository
                .Setup(repo => repo.UpdateAsync(existingTask))
                .Returns(System.Threading.Tasks.Task.CompletedTask)
                .Verifiable();

            existingTask.Title = "New Title";
            await _taskService.UpdateTaskAsync(existingTask);

            _mockTaskRepository.Verify(repo => repo.UpdateAsync(existingTask), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_ShouldCallDeleteTaskAsyncOnRepository()
        {
            int taskId = 7;
            _mockTaskRepository
                .Setup(repo => repo.DeleteAsync(taskId))
                .Returns(System.Threading.Tasks.Task.CompletedTask)
                .Verifiable();

            await _taskService.DeleteTaskAsync(taskId);

            _mockTaskRepository.Verify(repo => repo.DeleteAsync(taskId), Times.Once);
        }
    }
}
