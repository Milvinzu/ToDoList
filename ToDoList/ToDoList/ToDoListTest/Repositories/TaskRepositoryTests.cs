using Microsoft.EntityFrameworkCore;
using Moq;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services;
using ToDoListTest.Data;

namespace ToDoListTest.Repositories
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly TododbContext _context;
        private readonly ITaskRepository _repository;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TododbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TestTododbContext(options);
            _repository = new TaskRepository(_context);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTasksByListIdAsync_ShouldReturnTasks()
        {
            int listId = 1;
            var task = new ToDoList.Models.Task 
            { 
                ListId = listId, 
                UserId = 1, 
                Title = "Test Task", 
                Completed = false 
            };
            await _repository.AddAsync(task);

            var result = await _repository.GetTasksByListIdAsync(listId);
            Assert.Single(result);
            Assert.Equal("Test Task", result.First().Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTasksByUserIdAsync_ShouldReturnTasks()
        {
            int userId = 2;
            var task = new ToDoList.Models.Task 
            { 
                ListId = 1, 
                UserId = userId, 
                Title = "User Task", 
                Completed = false 
            };
            await _repository.AddAsync(task);

            var result = await _repository.GetTasksByUserIdAsync(userId);
            Assert.Single(result);
            Assert.Equal("User Task", result.First().Title);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
