using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Services
{
    public class ListServiceTests
    {
        private readonly Mock<IListRepository> _listRepositoryMock;
        private readonly ListService _listService;

        public ListServiceTests()
        {
            _listRepositoryMock = new Mock<IListRepository>();
            _listService = new ListService(_listRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateListAsync_ShouldAddListWhenNameAndColorAreValid()
        {
            int userId = 1;
            string validName = "My List";
            string validColor = "#FFAABB";

            _listRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<List>()))
                               .Returns(Task.CompletedTask);

            await _listService.CreateListAsync(userId, validName, validColor);

            _listRepositoryMock.Verify(repo => repo.AddAsync(It.Is<List>(l =>
                l.UserId == userId &&
                l.Name == validName &&
                l.Color == validColor)), Times.Once);
        }

        [Theory]
        [InlineData("", "#FFAABB")]
        [InlineData("My List", "invalidColor")]
        [InlineData(null, "#FFAABB")]
        public async Task CreateListAsync_ShouldThrowArgumentException_WhenInvalidInput(string name, string color)
        {
            int userId = 1;
            await Assert.ThrowsAsync<ArgumentException>(() => _listService.CreateListAsync(userId, name, color));
        }

        [Fact]
        public async Task ChangeColorAndNameAsync_ShouldUpdateListWhenListExistsAndInputsAreValid()
        {
            int listId = 10;
            string newName = "Updated List";
            string newColor = "#123456";

            var existingList = new List
            {
                Id = listId,
                UserId = 1,
                Name = "Old Name",
                Color = "#FFFFFF",
                CreatedAt = DateTime.UtcNow
            };

            _listRepositoryMock.Setup(repo => repo.GetByIdAsync(listId))
                               .ReturnsAsync(existingList);
            _listRepositoryMock.Setup(repo => repo.UpdateAsync(existingList))
                               .Returns(Task.CompletedTask);

            await _listService.ChangeColorAndNameAsync(listId, newColor, newName);

            Assert.Equal(newName, existingList.Name);
            Assert.Equal(newColor, existingList.Color);
            _listRepositoryMock.Verify(repo => repo.UpdateAsync(existingList), Times.Once);
        }

        [Theory]
        [InlineData("", "#123456")]
        [InlineData("Valid Name", "invalidColor")]
        [InlineData(null, "#123456")]
        public async Task ChangeColorAndNameAsync_ShouldThrowArgumentExceptionWhenInputsAreInvalid(string name, string color)
        {
            int userId = 1;
            await Assert.ThrowsAsync<ArgumentException>(() => _listService.ChangeColorAndNameAsync(userId, color, name));
        }
    }
}
