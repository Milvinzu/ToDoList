using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories;
using ToDoListTest.Data;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Repositories
{
    public class SubtaskRepositoryTests : IDisposable
    {
        private readonly TododbContext _context;
        private readonly SubtaskRepository _repository;

        public SubtaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TododbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TestTododbContext(options);
            _repository = new SubtaskRepository(_context);
        }

        [Fact]
        public async Task GetSubtasksByTaskIdAsync_ShouldReturnSubtasks_ForGivenTaskId()
        {
            int taskId = 1;
            var subtask1 = new Subtask { TaskId = taskId, Title = "Subtask 1", Completed = false };
            var subtask2 = new Subtask { TaskId = taskId, Title = "Subtask 2", Completed = false };
            var subtask3 = new Subtask { TaskId = 2, Title = "Other Subtask", Completed = false };

            await _repository.AddAsync(subtask1);
            await _repository.AddAsync(subtask2);
            await _repository.AddAsync(subtask3);
            await _context.SaveChangesAsync();

            var result = await _repository.GetSubtasksByTaskIdAsync(taskId);
            var subtasksList = result.ToList();

            Assert.Equal(2, subtasksList.Count);
            Assert.All(subtasksList, s => Assert.Equal(taskId, s.TaskId));
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
