using EgGroupAssessmentApp.Core.Models;

namespace EgGroupAssessmentApp.Core.Interfaces {
    public interface IUserRepository {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(int id, User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}
