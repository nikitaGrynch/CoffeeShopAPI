namespace CoffeeShopAPI.Data.dto.Models.Request;

public class UserRegistrationModel
{
    public String Name { get; set; } = null!;
    public String PhoneNumber { get; set; } = null!;
    public String Password { get; set; } = null!;
    public String PasswordConfirm { get; set; } = null!;
}