using EgGroupAssessmentApp.Core.Interfaces;
using EgGroupAssessmentApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EgGroupAssessmentApp.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger) {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request) {
            try {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password)) {
                    return BadRequest(ApiResponse<LoginResponse>.ErrorResponse("Username and password are required"));
                }

                var loginResponse = await _authService.AuthenticateAsync(request);
                return Ok(ApiResponse<LoginResponse>.SuccessResponse(loginResponse, "Login successful"));
            }
            catch (UnauthorizedAccessException ex) {
                _logger.LogWarning(ex, "Failed login attempt for username: {Username}", request.Username);
                return Unauthorized(ApiResponse<LoginResponse>.ErrorResponse("Invalid credentials"));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error during login for username: {Username}", request.Username);
                return StatusCode(500, ApiResponse<LoginResponse>.ErrorResponse("An error occurred during login"));
            }
        }
    }
}
