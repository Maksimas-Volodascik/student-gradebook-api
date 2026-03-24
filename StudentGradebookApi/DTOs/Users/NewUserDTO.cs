using System.ComponentModel.DataAnnotations;

namespace StudentGradebookApi.DTOs.Users
{
    public class NewUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
