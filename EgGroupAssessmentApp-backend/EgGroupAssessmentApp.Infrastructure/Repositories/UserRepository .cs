using EgGroupAssessmentApp.Core.Interfaces;
using EgGroupAssessmentApp.Core.Models;
using EgGroupAssessmentApp.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EgGroupAssessmentApp.Infrastructure.Repositories {

    public class UserRepository : IUserRepository {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id) {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username) {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<User?> GetByEmailAsync(string email) {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetAllAsync() {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> AddAsync(User user) {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User user) {
            var existingUser = await GetByIdAsync(id);
            if (existingUser == null)
                return null;

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(user.PasswordHash)) {
                existingUser.PasswordHash = user.PasswordHash;
            }

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id) {
            var user = await GetByIdAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id) {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> UsernameExistsAsync(string username) {
            return await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> EmailExistsAsync(string email) {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }
    }
}