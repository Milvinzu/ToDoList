using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Repositories;
using ToDoListTest.Data;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Repositories
{
    public class ListRepositoryTests : IDisposable
    {
        private readonly TododbContext _context;
        private readonly IListRepository _repository;

        public ListRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TododbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TestTododbContext(options);
            _repository = new ListRepository(_context);
        }

        [Fact]
        public async Task GetListByUserIdAsync_ShouldReturnListWhenFound()
        {
            int userId = 1;
            var list = new List
            {
                Id = 5,
                UserId = userId,
                Name = "Test List",
                Color = "#ABCDEF",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(list);
            var result = await _repository.GetListByUserIdAsync(userId);
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetListByUserIdAsync_ShouldReturnNullWhenNotFound()
        {
            int userId = 99;
            var result = await _repository.GetListByUserIdAsync(userId);
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_Should_AddList_ToDatabase()
        {
            var list = new List
            {
                UserId = 2,
                Name = "New List",
                Color = "#112233",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(list);
            var listInDb = await _repository.GetByIdAsync(list.Id);
            Assert.NotNull(listInDb);
            Assert.Equal("New List", listInDb.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateListInDatabase()
        {
            var list = new List
            {
                UserId = 2,
                Name = "Initial Name",
                Color = "#445566",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(list);
            list.Name = "Updated Name";
            list.Color = "#778899";
            await _repository.UpdateAsync(list);
            var updatedList = await _repository.GetByIdAsync(list.Id);
            Assert.NotNull(updatedList);
            Assert.Equal("Updated Name", updatedList.Name);
            Assert.Equal("#778899", updatedList.Color);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveListFromDatabase()
        {
            var list = new List
            {
                UserId = 3,
                Name = "List To Delete",
                Color = "#ABC123",
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(list);
            await _repository.DeleteAsync(list.Id);
            var deletedList = await _repository.GetByIdAsync(list.Id);
            Assert.Null(deletedList);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
