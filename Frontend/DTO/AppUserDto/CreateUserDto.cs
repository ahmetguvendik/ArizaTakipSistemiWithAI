namespace DTO.AppUserDto;

public class CreateUserDto
{
    public string Username { get; set; }
    public string NameSurname { get; set; }     
    public string Password { get; set; }
    public string Email { get; set; }
    public string DepartmanId { get; set; }
    public string Role { get; set; }
}