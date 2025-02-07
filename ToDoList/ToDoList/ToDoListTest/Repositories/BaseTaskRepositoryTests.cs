using ToDoList.Repositories;
using ToDoListTest.Repositories;


namespace ToDoListTest.Repositories
{
    public class BaseTaskRepositoryTests : BaseRepositoryTests<TaskRepository, ToDoList.Models.Task>
    {
        public BaseTaskRepositoryTests()
            : base(context => new TaskRepository(context)) { }

        protected override ToDoList.Models.Task CreateTestEntity()
        {
            return new ToDoList.Models.Task
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

        protected override object GetEntityId(ToDoList.Models.Task entity) => entity.Id;

        protected override void ModifyEntity(ToDoList.Models.Task entity)
        {
            entity.Title = "Updated Task";
            entity.Description = "Updated Description";
        }

        protected override void AssertEntityUpdated(ToDoList.Models.Task entity)
        {
            Assert.Equal("Updated Task", entity.Title);
            Assert.Equal("Updated Description", entity.Description);
        }

        protected override void AssertEntityMatches(ToDoList.Models.Task expected, ToDoList.Models.Task actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);
        }
    }

}