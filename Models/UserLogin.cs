using System.ComponentModel.DataAnnotations;

//Login is already required
public class UserLogin
{
    [Required, UIHint("email")]
    public string Email { get; set; }

    [Required, UIHint("password")]
    public string Password { get; set; }
}