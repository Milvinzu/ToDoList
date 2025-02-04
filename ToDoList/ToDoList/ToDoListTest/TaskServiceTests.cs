﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services;
using ToDoList.Services.Interfaces;

namespace ToDoListTest
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
        public async System.Threading.Tasks.Task GetAllTasksAsync_Should_Return_All_Tasks()
        {
            // Arrange
            var expectedTasks = new List<ToDoList.Models.Task>
            {
                new ToDoList.Models.Task { Id = 1, Title = "Task 1", ListId = 1, UserId = 1, DueDate = DateTime.Now.AddDays(1) },
                new ToDoList.Models.Task { Id = 2, Title = "Task 2", ListId = 1, UserId = 1, DueDate = DateTime.Now.AddDays(2) }
            };

            _mockTaskRepository
                .Setup(repo => repo.GetAllTasksAsync())
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Title == "Task 1");
            Assert.Contains(result, t => t.Title == "Task 2");
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_Should_Return_Task_When_Found()
        {
            // Arrange
            var task = new ToDoList.Models.Task { Id = 3, Title = "Task 3", ListId = 1, UserId = 1, DueDate = DateTime.Now.AddDays(1) };

            _mockTaskRepository
                .Setup(repo => repo.GetTaskByIdAsync(3))
                .ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskByIdAsync(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Task 3", result.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_Should_Call_AddTaskAsync_When_Task_Is_Valid()
        {
            // Arrange
            var newTask = new ToDoList.Models.Task { Id = 4, Title = "Valid Task", ListId = 1, UserId = 1, DueDate = DateTime.Now.AddDays(1) };

            _mockTaskRepository
                .Setup(repo => repo.AddTaskAsync(newTask))
                .Returns(System.Threading.Tasks.Task.CompletedTask)
                .Verifiable();

            // Act
            await _taskService.CreateTaskAsync(newTask);

            // Assert
            _mockTaskRepository.Verify(repo => repo.AddTaskAsync(newTask), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_Should_Throw_Exception_When_Title_Is_Empty()
        {
            // Arrange
            var newTask = new ToDoList.Models.Task { Id = 5, Title = "   ", ListId = 1, UserId = 1, DueDate = DateTime.Now.AddDays(1) };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(newTask));
            Assert.Equal("Task title cannot be empty.", exception.Message);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskAsync_Should_Call_UpdateTaskAsync_On_Repository()
        {
            // Arrange
            var existingTask = new ToDoList.Models.Task { Id = 6, Title = "Old Title", ListId = 1, UserId = 1, DueDate = DateTime.Now.AddDays(1) };

            _mockTaskRepository
                .Setup(repo => repo.UpdateTaskAsync(existingTask))
                .Returns(System.Threading.Tasks.Task.CompletedTask)
                .Verifiable();

            // Act
            existingTask.Title = "New Title";
            await _taskService.UpdateTaskAsync(existingTask);

            // Assert
            _mockTaskRepository.Verify(repo => repo.UpdateTaskAsync(existingTask), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_Should_Call_DeleteTaskAsync_On_Repository()
        {
            // Arrange
            int taskId = 7;
            _mockTaskRepository
                .Setup(repo => repo.DeleteTaskAsync(taskId))
                .Returns(System.Threading.Tasks.Task.CompletedTask)
                .Verifiable();

            // Act
            await _taskService.DeleteTaskAsync(taskId);

            // Assert
            _mockTaskRepository.Verify(repo => repo.DeleteTaskAsync(taskId), Times.Once);
        }
    }
}
