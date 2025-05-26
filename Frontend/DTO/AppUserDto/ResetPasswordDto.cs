using System.ComponentModel.DataAnnotations;

namespace DTO.AppUserDto;

public class ResetPasswordDto
{
    public string Email { get; set; }
    public string Token { get; set; }

    [Required(ErrorMessage = "Yeni şifre boş olamaz")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}