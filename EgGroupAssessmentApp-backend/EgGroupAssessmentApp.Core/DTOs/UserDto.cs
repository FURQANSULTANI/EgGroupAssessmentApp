using System.ComponentModel.DataAnnotations;

namespace EgGroupAssessmentApp.Core.DTOs {
    public class UserDto {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(Admin|User)$", ErrorMessage = "Role must be either 'Admin' or 'User'")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
    }

    public class CreateUserDto : UserDto {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUserDto : UserDto {
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters if provided")]
        public string? Password { get; set; }
    }
}
