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

namespace ToDoListTest.Repositories
{
    public abstract class BaseRepositoryTests<TRepository, TEntity> : IDisposable
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : class
    {
        protected readonly TododbContext _context;
        protected readonly TRepository _repository;

        protected BaseRepositoryTests(Func<TododbContext, TRepository> repositoryFactory)
        {
            var options = new DbContextOptionsBuilder<TododbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestTododbContext(options);
            _repository = repositoryFactory(_context);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddAsync_ShouldAddEntityToDatabase()
        {
            var entity = CreateTestEntity();
            await _repository.AddAsync(entity);

            var entityInDb = await _context.Set<TEntity>().FirstOrDefaultAsync();
            Assert.NotNull(entityInDb);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteAsync_ShouldRemoveEntityFromDatabase()
        {
            var entity = CreateTestEntity();
            await _repository.AddAsync(entity);
            await _repository.DeleteAsync((int)GetEntityId(entity));

            var entityInDb = await _context.Set<TEntity>().FindAsync(GetEntityId(entity));
            Assert.Null(entityInDb);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateAsync_ShouldUpdateEntityInDatabase()
        {
            var entity = CreateTestEntity();
            await _repository.AddAsync(entity);

            ModifyEntity(entity);
            await _repository.UpdateAsync(entity);

            var entityInDb = await _context.Set<TEntity>().FindAsync(GetEntityId(entity));
            Assert.NotNull(entityInDb);
            AssertEntityUpdated(entityInDb);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByIdAsync_ShouldGetEntityById()
        {
            var entity = CreateTestEntity();
            await _repository.AddAsync(entity);

            var retrievedEntity = await _repository.GetByIdAsync((int)GetEntityId(entity));

            Assert.NotNull(retrievedEntity);
            AssertEntityMatches(entity, retrievedEntity);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllAsync_ShouldGetAllEntities()
        {
            var entity1 = CreateTestEntity();
            var entity2 = CreateTestEntity();
            await _repository.AddAsync(entity1);
            await _repository.AddAsync(entity2);

            var entities = await _repository.GetAllAsync();

            Assert.NotNull(entities);
            Assert.Equal(2, entities.Count());
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        protected abstract TEntity CreateTestEntity();
        protected abstract object GetEntityId(TEntity entity);
        protected abstract void ModifyEntity(TEntity entity);
        protected abstract void AssertEntityUpdated(TEntity entity);
        protected abstract void AssertEntityMatches(TEntity expected, TEntity actual);
    }
}
