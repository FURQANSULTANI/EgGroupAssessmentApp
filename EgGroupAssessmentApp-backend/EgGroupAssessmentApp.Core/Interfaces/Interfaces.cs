using EgGroupAssessmentApp.Core.DTOs;
using EgGroupAssessmentApp.Core.Models;

namespace EgGroupAssessmentApp.Core.Interfaces {
    public interface IUserService {
        Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
        Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<ApiResponse<UserDto>> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<ApiResponse<bool>> DeleteUserAsync(int id);
    }
}
