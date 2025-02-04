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
    public class TaskRepositoryTests : IDisposable
    {
        private readonly TododbContext _context;
        private readonly TaskRepository _repository;

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

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_Should_Remove_Task_From_Database()
        {
            var newTask = new ToDoList.Models.Task
            {
                Id = 2,
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            await _repository.AddTaskAsync(newTask);

            await _repository.DeleteTaskAsync(newTask.Id);

            var taskInDb = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == newTask.Id);
            Assert.Null(taskInDb);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskAsync_Should_Update_Task_In_Database()
        {
            var newTask = new ToDoList.Models.Task
            {
                Id = 3,
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            await _repository.AddTaskAsync(newTask);
            await _context.SaveChangesAsync();

            newTask.Title = "Test Task Update";
            newTask.Description = "Updated Description";

            await _repository.UpdateTaskAsync(newTask);
            await _context.SaveChangesAsync();

            var taskInDb = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == newTask.Id);
            Assert.NotNull(taskInDb);
            Assert.Equal("Test Task Update", taskInDb.Title);
            Assert.Equal("Updated Description", taskInDb.Description);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_Should_Get_Task_By_Id_From_Database()
        {
            var newTask = new ToDoList.Models.Task
            {
                Id = 4,
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };

            await _repository.AddTaskAsync(newTask);

            var taskById = await _repository.GetTaskByIdAsync(newTask.Id);

            Assert.NotNull(taskById);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTaskAsync_Should_Get_All_Task_From_Database()
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

            await _repository.AddTaskAsync(task1);
            await _repository.AddTaskAsync(task2);
            await _context.SaveChangesAsync();

            var tasks = await _repository.GetAllTaskAsync();

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
