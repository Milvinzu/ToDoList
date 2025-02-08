using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Services
{
    public class SubtaskServiceTests
    {
        private readonly Mock<ISubtaskRepository> _subtaskRepositoryMock;
        private readonly SubtaskService _subtaskService;

        public SubtaskServiceTests()
        {
            _subtaskRepositoryMock = new Mock<ISubtaskRepository>();
            _subtaskService = new SubtaskService(_subtaskRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateSubtaskAsync_ShouldAddSubtask_WhenTitleIsValid()
        {
            int taskId = 1;
            string validTitle = "New Subtask";

            _subtaskRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Subtask>()))
                .Returns(Task.CompletedTask);

            await _subtaskService.CreateSubtaskAsync(taskId, validTitle);

            _subtaskRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Subtask>(subtask =>
                subtask.TaskId == taskId &&
                subtask.Title == validTitle &&
                subtask.Completed == false
            )), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateSubtaskAsync_ShouldThrowArgumentException_WhenTitleIsInvalid(string invalidTitle)
        {
            int taskId = 1;

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _subtaskService.CreateSubtaskAsync(taskId, invalidTitle));
            Assert.Equal("Title empty", exception.Message);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldToggleCompletedStatus_WhenSubtaskExists()
        {
            int subtaskId = 1;
            var subtask = new Subtask { Id = subtaskId, Completed = false };
            _subtaskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(subtaskId))
                .ReturnsAsync(subtask);
            _subtaskRepositoryMock
                .Setup(repo => repo.UpdateAsync(subtask))
                .Returns(Task.CompletedTask);

            await _subtaskService.ChangeStatusAsync(subtaskId);

            Assert.True(subtask.Completed);
            _subtaskRepositoryMock.Verify(repo => repo.UpdateAsync(subtask), Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldThrowArgumentException_WhenSubtaskNotFound()
        {
            int subtaskId = 99;
            _subtaskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(subtaskId))
                .ReturnsAsync((Subtask)null);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _subtaskService.ChangeStatusAsync(subtaskId));
            Assert.Equal("Subtask not found", exception.Message);
        }

        [Fact]
        public async Task ChangeTitleAsync_ShouldUpdateTitle_WhenSubtaskExistsAndTitleIsValid()
        {
            int subtaskId = 1;
            string newTitle = "Updated Title";
            var subtask = new Subtask { Id = subtaskId, Title = "Old Title" };
            _subtaskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(subtaskId))
                .ReturnsAsync(subtask);
            _subtaskRepositoryMock
                .Setup(repo => repo.UpdateAsync(subtask))
                .Returns(Task.CompletedTask);

            await _subtaskService.ChangeTitleAsync(subtaskId, newTitle);

            Assert.Equal(newTitle, subtask.Title);
            _subtaskRepositoryMock.Verify(repo => repo.UpdateAsync(subtask), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ChangeTitleAsync_ShouldThrowArgumentException_WhenTitleIsInvalid(string invalidTitle)
        {
            int subtaskId = 1;

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _subtaskService.ChangeTitleAsync(subtaskId, invalidTitle));
            Assert.Equal("Title empty", exception.Message);
        }

        [Fact]
        public async Task ChangeTitleAsync_ShouldThrowArgumentException_WhenSubtaskNotFound()
        {
            int subtaskId = 99;
            string newTitle = "Updated Title";
            _subtaskRepositoryMock
                .Setup(repo => repo.GetByIdAsync(subtaskId))
                .ReturnsAsync((Subtask)null);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _subtaskService.ChangeTitleAsync(subtaskId, newTitle));
            Assert.Equal("Subtask not found", exception.Message);
        }

        [Fact]
        public async Task GetSubtasksByTaskId_ShouldReturnSubtasks_WhenTheyExist()
        {
            int taskId = 1;
            var subtasks = new List<Subtask>
            {
                new Subtask { Id = 1, TaskId = taskId, Title = "Subtask 1" },
                new Subtask { Id = 2, TaskId = taskId, Title = "Subtask 2" }
            };

            _subtaskRepositoryMock
                .Setup(repo => repo.GetSubtasksByTaskIdAsync(taskId))
                .ReturnsAsync(subtasks);

            var result = await _subtaskService.GetSubtasksByTaskId(taskId);

            Assert.Equal(2, result.Count());
            Assert.Contains(result, s => s.Title == "Subtask 1");
            Assert.Contains(result, s => s.Title == "Subtask 2");
        }

        [Fact]
        public async Task DeleteSubtaskAsync_ShouldCallDeleteAsync()
        {
            int subtaskId = 1;
            _subtaskRepositoryMock
                .Setup(repo => repo.DeleteAsync(subtaskId))
                .Returns(Task.CompletedTask);

            await _subtaskService.DeleteSubtaskAsync(subtaskId);

            _subtaskRepositoryMock.Verify(repo => repo.DeleteAsync(subtaskId), Times.Once);
        }
    }
}
