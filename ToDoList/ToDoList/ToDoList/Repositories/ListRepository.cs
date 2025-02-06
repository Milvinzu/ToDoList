using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;

namespace ToDoList.Repositories
{
    public class ListRepository : BaseRepository<List>, IListRepository
    {
        public ListRepository(TododbContext context) : base(context) { }

        public async Task<List> GetListByUserIdAsync(int userId)
        {
            return await _context.Lists.FirstOrDefaultAsync(list => list.UserId == userId);
        }
    }
}
