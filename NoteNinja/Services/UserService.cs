using System.Threading.Tasks;
using NoteNinja.Models;
using NoteNinja.Repositories;

namespace NoteNinja.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetAllAsync(u => u.Email == email && u.Password == password);
            return user?.FirstOrDefault();
        }

        public async Task CreateAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }
    }
}
