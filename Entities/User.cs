using server.Utils;
using System.ComponentModel.DataAnnotations;

namespace server.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string Role { get; set; }
    public string Address { get; set; } = "";

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpire { get; set; }



    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetPasswordExpire { get; set; }

    public string? PhoneNumber { get; set; } = "";

    public string? BusinessName { get; set; } = "";


    public BusinessTypes? BusinessType { get; set; } = null;


    public string? GSTNumber { get; set; } = "";


}
