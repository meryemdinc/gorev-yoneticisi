using GorevYoneticisiProjesi.Models;

namespace GorevYoneticisiProjesi.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string password);
        Task<User> ValidateUserAsync(string username, string password);
        Task<string> GenerateJwtTokenAsync(User user);
        Task<User?> GetByIdAsync(Guid id);

    }
}
