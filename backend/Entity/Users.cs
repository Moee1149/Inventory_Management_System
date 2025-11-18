
namespace Backend.Entity;

public class User : BaseEntity
{
    public string Email { get; set; } = "";
    public string HashedPassword { get; set; } = "";
    public string Role { get; set; } = "";
}
