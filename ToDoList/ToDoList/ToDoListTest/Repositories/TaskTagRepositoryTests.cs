using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories;
using ToDoListTest.Data;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Repositories
{
    public class TaskTagRepositoryTests : IDisposable
    {
        private readonly TododbContext _context;
        private readonly TaskTagRepository _repository;

        public TaskTagRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TododbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TestTododbContext(options);
            _repository = new TaskTagRepository(_context);
        }

        [Fact]
        public async Task GetTagsByTaskIdAsync_ShouldReturnCorrectTags()
        {
            int taskId = 1;
            var tag1 = new Tasktag { TaskId = taskId, Tag = "Tag1" };
            var tag2 = new Tasktag { TaskId = taskId, Tag = "Tag2" };
            var tag3 = new Tasktag { TaskId = 2, Tag = "OtherTag" };

            await _repository.AddAsync(tag1);
            await _repository.AddAsync(tag2);
            await _repository.AddAsync(tag3);
            await _context.SaveChangesAsync();

            var result = await _repository.GetTagsByTaskIdAsync(taskId);
            var tagList = result.ToList();

            Assert.Equal(2, tagList.Count);
            Assert.Contains(tagList, t => t.Tag == "Tag1");
            Assert.Contains(tagList, t => t.Tag == "Tag2");
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
