using ToDoList.Repositories;
using TaskEntity = ToDoList.Models.Task;

namespace ToDoListTest.Repositories
{
    public class BaseTaskRepositoryTests : BaseRepositoryTests<TaskRepository, TaskEntity>
    {
        public BaseTaskRepositoryTests()
            : base(context => new TaskRepository(context)) { }

        protected override TaskEntity CreateTestEntity()
        {
            return new TaskEntity
            {
                ListId = 1,
                UserId = 1,
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                Completed = false
            };
        }

        protected override object GetEntityId(TaskEntity entity) => entity.Id;

        protected override void ModifyEntity(TaskEntity entity)
        {
            entity.Title = "Updated Task";
            entity.Description = "Updated Description";
        }

        protected override void AssertEntityUpdated(TaskEntity entity)
        {
            Assert.Equal("Updated Task", entity.Title);
            Assert.Equal("Updated Description", entity.Description);
        }

        protected override void AssertEntityMatches(TaskEntity expected, TaskEntity actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
        }
    }

}