using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories;
using ToDoList.Repositories.Interfaces;
using ToDoListTest.Data;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Repositories
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly TododbContext _context;
        private readonly IUserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TododbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestTododbContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task ChangeName_Should_Update_UserName()
        {
            var user = new User
            {
                Name = "OldName",
                Email = "test@example.com",
                PasswordHash = "oldHash",
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(user);

            await _repository.ChangeNameAsync(user.Id, "NewName");

            var updatedUser = await _repository.GetByIdAsync(user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("NewName", updatedUser.Name);
        }

        [Fact]
        public async Task ChangePassword_Should_Update_UserPasswordHash()
        {
            var user = new User
            {
                Name = "User",
                Email = "test@example.com",
                PasswordHash = "oldHash",
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(user);

            await _repository.ChangePasswordAsync(user.Id, "newHash");

            var updatedUser = await _repository.GetByIdAsync(user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("newHash", updatedUser.PasswordHash);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_User_When_Found()
        {
            var user = new User
            {
                Name = "Test",
                Email = "test@example.com",
                PasswordHash = "hash",
                CreatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(user);
            var result = await _repository.GetByEmailAsync("test@example.com");
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_Null_When_Not_Found()
        {
            var result = await _repository.GetByEmailAsync("nonexistent@example.com");
            Assert.Null(result);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
