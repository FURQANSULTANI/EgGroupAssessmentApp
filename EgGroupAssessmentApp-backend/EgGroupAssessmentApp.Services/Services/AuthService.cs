using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EgGroupAssessmentApp.Core.Models;
using EgGroupAssessmentApp.Core.Interfaces;

namespace EgGroupAssessmentApp.Services.Services {
    public class AuthService : IAuthService {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository) {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request) {
            var user = await GetUserByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var token = GenerateJwtToken(user);

            return new LoginResponse {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public string GenerateJwtToken(User user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyForJwtTokenGenerationThatIsAtLeast32CharactersLong");

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User?> GetUserByUsernameAsync(string username) {
            return await _userRepository.GetByUsernameAsync(username);
        }
    }
}
