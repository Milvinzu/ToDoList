using ToDoList.Models;

namespace ToDoList.Repositories.Interfaces
{
    public interface IListRepository : IBaseRepository<List> 
    {
        Task<List> GetListByUserIdAsync(int userId);
    }
}
