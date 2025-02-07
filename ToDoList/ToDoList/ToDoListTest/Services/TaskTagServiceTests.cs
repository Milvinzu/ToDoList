using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services;
using TaskTagEntity = ToDoList.Models.Tasktag;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Services
{
    public class TaskTagServiceTests
    {
        private readonly Mock<ITaskTagRepository> _taskTagRepositoryMock;
        private readonly TaskTagService _taskTagService;

        public TaskTagServiceTests()
        {
            _taskTagRepositoryMock = new Mock<ITaskTagRepository>();
            _taskTagService = new TaskTagService(_taskTagRepositoryMock.Object);
        }

        [Fact]
        public async Task AddTagAsync_ShouldCallAddAsync_WhenTagIsValid()
        {
            int taskId = 1;
            string tag = "Important";

            _taskTagRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<TaskTagEntity>()))
                .Returns(Task.CompletedTask);

            await _taskTagService.AddTagAsync(taskId, tag);

            _taskTagRepositoryMock.Verify(repo => repo.AddAsync(
                It.Is<TaskTagEntity>(tt => tt.TaskId == taskId && tt.Tag == tag)
            ), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddTagAsync_ShouldThrowArgumentNullException_WhenTagIsEmptyOrNull(string invalidTag)
        {
            int taskId = 1;

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _taskTagService.AddTagAsync(taskId, invalidTag));
        }

        [Fact]
        public async Task ChangeTagAsync_ShouldUpdateTag_WhenTaskExistsAndTagIsValid()
        {
            int tagId = 1;
            string newTag = "Urgent";
            var existingTaskTag = new TaskTagEntity { Id = tagId, Tag = "OldTag", TaskId = 1 };

            _taskTagRepositoryMock
                .Setup(repo => repo.GetByIdAsync(tagId))
                .ReturnsAsync(existingTaskTag);
            _taskTagRepositoryMock
                .Setup(repo => repo.UpdateAsync(existingTaskTag))
                .Returns(Task.CompletedTask);

            await _taskTagService.ChangeTagAsync(tagId, newTag);

            Assert.Equal(newTag, existingTaskTag.Tag);
            _taskTagRepositoryMock.Verify(repo => repo.UpdateAsync(existingTaskTag), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ChangeTagAsync_ShouldThrowArgumentNullException_WhenTagIsEmptyOrNull(string invalidTag)
        {
            int tagId = 1;

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _taskTagService.ChangeTagAsync(tagId, invalidTag));
        }

        [Fact]
        public async Task ChangeTagAsync_ShouldThrowArgumentNullException_WhenTaskTagNotFound()
        {
            int tagId = 99;
            string newTag = "NewTag";

            _taskTagRepositoryMock
                .Setup(repo => repo.GetByIdAsync(tagId))
                .ReturnsAsync((TaskTagEntity)null);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _taskTagService.ChangeTagAsync(tagId, newTag));
        }

        [Fact]
        public async Task DeleteTagAsync_ShouldCallDeleteAsync()
        {
            int tagId = 1;
            _taskTagRepositoryMock
                .Setup(repo => repo.DeleteAsync(tagId))
                .Returns(Task.CompletedTask);

            await _taskTagService.DeleteTagAsync(tagId);

            _taskTagRepositoryMock.Verify(repo => repo.DeleteAsync(tagId), Times.Once);
        }

        [Fact]
        public async Task GetTagsAsync_ShouldReturnTagsList()
        {
            int taskId = 1;
            var tagsList = new List<TaskTagEntity>
            {
                new TaskTagEntity { Id = 1, TaskId = taskId, Tag = "Tag1" },
                new TaskTagEntity { Id = 2, TaskId = taskId, Tag = "Tag2" }
            };

            _taskTagRepositoryMock
                .Setup(repo => repo.GetTagsByTaskIdAsync(taskId))
                .ReturnsAsync(tagsList);

            var result = await _taskTagService.GetTagsAsync(taskId);

            Assert.Equal(2, result.Count());
            Assert.Contains(result, tag => tag.Tag == "Tag1");
            Assert.Contains(result, tag => tag.Tag == "Tag2");
        }
    }
}
