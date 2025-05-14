using System.ComponentModel.DataAnnotations;

namespace server.Dto.Auth
{
    public class RegisterAdminDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedBy { get; set; }

    }
}
