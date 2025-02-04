using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Repositories;
using ToDoListTest.Data;

namespace ToDoListTest
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
        public async System.Threading.Tasks.Task AddTaskAsync_Should_Add_Task_To_Database()
        {
            var newTask = new ToDoList.Models.Task
            {
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            await _repository.AddAsync(newTask);

            var taskInDb = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == newTask.Id);
            Assert.NotNull(taskInDb);
            Assert.Equal("Test Task", taskInDb.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_Should_Remove_Task_From_Database()
        {
            var newTask = new ToDoList.Models.Task
            {
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            await _repository.AddAsync(newTask);

            await _repository.DeleteAsync(newTask.Id);

            var taskInDb = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == newTask.Id);
            Assert.Null(taskInDb);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskAsync_Should_Update_Task_In_Database()
        {
            var newTask = new ToDoList.Models.Task
            {
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            await _repository.AddAsync(newTask);

            newTask.Title = "Test Task Update";
            newTask.Description = "Updated Description";
            await _repository.UpdateAsync(newTask);

            var taskInDb = await _repository.GetByIdAsync(newTask.Id);
            Assert.NotNull(taskInDb);
            Assert.Equal("Test Task Update", taskInDb.Title);
            Assert.Equal("Updated Description", taskInDb.Description);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_Should_Get_Task_By_Id_From_Database()
        {
            var newTask = new ToDoList.Models.Task
            {
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            await _repository.AddAsync(newTask);

            var retrievedTask = await _repository.GetByIdAsync(newTask.Id);

            Assert.NotNull(retrievedTask);
            Assert.Equal("Test Task", retrievedTask.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTaskAsync_Should_Get_All_Tasks_From_Database()
        {
            var task1 = new ToDoList.Models.Task
            {
                ListId = 1,
                UserId = 1,
                Title = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            var task2 = new ToDoList.Models.Task
            {
                ListId = 1,
                UserId = 1,
                Title = "Task 2",
                Description = "Description 2",
                DueDate = DateTime.Now.AddDays(2),
                Priority = 2,
                Completed = false
            };

            await _repository.AddAsync(task1);
            await _repository.AddAsync(task2);

            var tasks = await _repository.GetAllAsync();

            Assert.NotNull(tasks);
            Assert.Equal(2, tasks.Count());
            Assert.Contains(tasks, t => t.Title == "Task 1");
            Assert.Contains(tasks, t => t.Title == "Task 2");
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

