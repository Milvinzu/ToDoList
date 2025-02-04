using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories;
using ToDoList.Repositories.Interfaces;
using ToDoListTest.Data;

namespace ToDoListTest
{
    public class TaskRepositoryTests
    {
        private readonly TododbContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TododbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TestTododbContext(options);
            _repository = new TaskRepository(_context);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddTaskAsync_Should_Add_Task_To_Database()
        {
            var newTask = new ToDoList.Models.Task
            {
                Id = 1,
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            await _repository.AddTaskAsync(newTask);

            var taskInDb = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == newTask.Id);
            Assert.NotNull(taskInDb);
            Assert.Equal("Test Task", taskInDb.Title);
        }
    }
}
