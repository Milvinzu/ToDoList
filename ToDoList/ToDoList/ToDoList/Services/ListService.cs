using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace ToDoList.Services
{
    public class ListService : IListService
    {
        private readonly IListRepository _listRepository;

        public ListService(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public async Task CreateListAsync(int userId, string name, string color)
        {
            if(string.IsNullOrWhiteSpace(name) || !IsValidHexColor(color))
            {
                throw new ArgumentException("Invalid name or color");
            }

            List list = new List()
            {
                UserId = userId,
                Name = name,
                Color = color,
                CreatedAt = DateTime.Now
            };

            await _listRepository.AddAsync(list);
        }

        public async Task ChangeColorAndNameAsync(int id, string color, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !IsValidHexColor(color))
            {
                throw new ArgumentException("Invalid name or color");
            }

            List list = await _listRepository.GetByIdAsync(id);
            if (list == null)
            {
                throw new InvalidOperationException("List not found for the given user.");
            }

            list.Name = name;
            list.Color = color;

            await _listRepository.UpdateAsync(list);
        }

        public async Task DeleteListAsync(int id)
        {
            await _listRepository.DeleteAsync(id);
        }

        public async Task<List> GetAllListByUserAsync(int userId)
        {
            return await _listRepository.GetListByUserIdAsync(userId);
        }

        private static bool IsValidHexColor(string color)
        {
            if(string.IsNullOrEmpty(color))
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(color) &&
                   System.Text.RegularExpressions.Regex.IsMatch(color, @"^#(?:[0-9a-fA-F]{3}|[0-9a-fA-F]{6})$");
        }

    }
}
