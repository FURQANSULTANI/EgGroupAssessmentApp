using System.Security.Claims;
using EgGroupAssessmentApp.Core.DTOs;
using EgGroupAssessmentApp.Core.Interfaces;
using EgGroupAssessmentApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EgGroupAssessmentApp.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger) {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers() {
            try {
                var result = await _userService.GetAllUsersAsync();
                if (!result.Success) {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error retrieving all users");
                return StatusCode(500, ApiResponse<IEnumerable<UserDto>>.ErrorResponse("An error occurred while retrieving users"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int id) {
            try {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var isAdmin = User.IsInRole("Admin");

                if (!isAdmin && currentUserId != id) {
                    return Forbid();
                }

                var result = await _userService.GetUserByIdAsync(id);
                if (!result.Success) {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
                return StatusCode(500, ApiResponse<UserDto>.ErrorResponse("An error occurred while retrieving user"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserDto createUserDto) {
            try {
                if (!ModelState.IsValid) {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<UserDto>.ErrorResponse("Invalid input", errors));
                }

                var result = await _userService.CreateUserAsync(createUserDto);
                if (!result.Success) {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(GetUserById), new { id = result.Data?.Id }, result);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, ApiResponse<UserDto>.ErrorResponse("An error occurred while creating user"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto) {
            try {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var isAdmin = User.IsInRole("Admin");

                if (!isAdmin && currentUserId != id) {
                    return Forbid();
                }

                if (!ModelState.IsValid) {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(ApiResponse<UserDto>.ErrorResponse("Invalid input", errors));
                }

                var result = await _userService.UpdateUserAsync(id, updateUserDto);
                if (!result.Success) {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error updating user with ID {UserId}", id);
                return StatusCode(500, ApiResponse<UserDto>.ErrorResponse("An error occurred while updating user"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(int id) {
            try {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (currentUserId == id) {
                    return BadRequest(ApiResponse<bool>.ErrorResponse("Cannot delete your own account"));
                }

                var result = await _userService.DeleteUserAsync(id);
                if (!result.Success) {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while deleting user"));
            }
        }
    }
}