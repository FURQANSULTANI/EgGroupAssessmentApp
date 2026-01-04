using EgGroupAssessmentApp.Core.DTOs;
using EgGroupAssessmentApp.Core.Interfaces;
using EgGroupAssessmentApp.Core.Models;

namespace EgGroupAssessmentApp.Services.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int id) {
            try {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null) {
                    return ApiResponse<UserDto>.ErrorResponse($"User with ID {id} not found");
                }

                var userDto = MapToDto(user);
                return ApiResponse<UserDto>.SuccessResponse(userDto, "User retrieved successfully");
            }
            catch (Exception ex) {
                return ApiResponse<UserDto>.ErrorResponse($"Error retrieving user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync() {
            try {
                var users = await _userRepository.GetAllAsync();
                var userDtos = users.Select(MapToDto).ToList();
                return ApiResponse<IEnumerable<UserDto>>.SuccessResponse(userDtos, "Users retrieved successfully");
            }
            catch (Exception ex) {
                return ApiResponse<IEnumerable<UserDto>>.ErrorResponse($"Error retrieving users: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto) {
            try {
                if (await _userRepository.UsernameExistsAsync(createUserDto.Username)) {
                    return ApiResponse<UserDto>.ErrorResponse("Username already exists");
                }

                if (await _userRepository.EmailExistsAsync(createUserDto.Email)) {
                    return ApiResponse<UserDto>.ErrorResponse("Email already exists");
                }

                var user = new User {
                    Username = createUserDto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                    Role = createUserDto.Role,
                    Email = createUserDto.Email,
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.AddAsync(user);
                var userDto = MapToDto(createdUser);

                return ApiResponse<UserDto>.SuccessResponse(userDto, "User created successfully");
            }
            catch (Exception ex) {
                return ApiResponse<UserDto>.ErrorResponse($"Error creating user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto) {
            try {
                if (!await _userRepository.ExistsAsync(id)) {
                    return ApiResponse<UserDto>.ErrorResponse($"User with ID {id} not found");
                }

                var existingUserWithUsername = await _userRepository.GetByUsernameAsync(updateUserDto.Username);
                if (existingUserWithUsername != null && existingUserWithUsername.Id != id) {
                    return ApiResponse<UserDto>.ErrorResponse("Username already exists");
                }

                var existingUserWithEmail = await _userRepository.GetByEmailAsync(updateUserDto.Email);
                if (existingUserWithEmail != null && existingUserWithEmail.Id != id) {
                    return ApiResponse<UserDto>.ErrorResponse("Email already exists");
                }

                var user = new User {
                    Username = updateUserDto.Username,
                    Role = updateUserDto.Role,
                    Email = updateUserDto.Email,
                    UpdatedAt = DateTime.UtcNow
                };

                if (!string.IsNullOrEmpty(updateUserDto.Password)) {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
                }

                var updatedUser = await _userRepository.UpdateAsync(id, user);
                if (updatedUser == null) {
                    return ApiResponse<UserDto>.ErrorResponse($"Error updating user with ID {id}");
                }

                var userDto = MapToDto(updatedUser);
                return ApiResponse<UserDto>.SuccessResponse(userDto, "User updated successfully");
            }
            catch (Exception ex) {
                return ApiResponse<UserDto>.ErrorResponse($"Error updating user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int id) {
            try {
                var result = await _userRepository.DeleteAsync(id);
                if (!result) {
                    return ApiResponse<bool>.ErrorResponse($"User with ID {id} not found");
                }

                return ApiResponse<bool>.SuccessResponse(true, "User deleted successfully");
            }
            catch (Exception ex) {
                return ApiResponse<bool>.ErrorResponse($"Error deleting user: {ex.Message}");
            }
        }

        private UserDto MapToDto(User user) {
            return new UserDto {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Email = user.Email
            };
        }
    }
}
