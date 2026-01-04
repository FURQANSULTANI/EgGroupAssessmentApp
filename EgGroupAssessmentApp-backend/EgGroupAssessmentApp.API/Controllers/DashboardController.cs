using EgGroupAssessmentApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace EgGroupAssessmentApp.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ApiResponse<string>> GetDashboard() {
            try {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var username = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(role)) {
                    return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid token"));
                }

                string dashboardMessage = role.ToLower() == "admin"
                    ? $"Admin Dashboard - Welcome, {username}!"
                    : $"User Dashboard - Welcome, {username}!";

                _logger.LogInformation("Dashboard accessed by {Username} with role {Role}", username, role);

                return Ok(ApiResponse<string>.SuccessResponse(dashboardMessage, "Dashboard accessed successfully"));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error accessing dashboard");
                return StatusCode(500, ApiResponse<string>.ErrorResponse("An error occurred while accessing dashboard"));
            }
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiResponse<string>> GetAdminDashboard() {
            try {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                return Ok(ApiResponse<string>.SuccessResponse(
                    $"Administrator Privileges - Welcome, {username}!",
                    "Admin dashboard accessed successfully"));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error accessing admin dashboard");
                return StatusCode(500, ApiResponse<string>.ErrorResponse("An error occurred while accessing admin dashboard"));
            }
        }
    }
}
