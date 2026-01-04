using EgGroupAssessmentApp.Core.Models;

namespace EgGroupAssessmentApp.Core.Interfaces {
    public interface IAuthService {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        string GenerateJwtToken(User user);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
