namespace CoffeeShopAPI.Data.dto.Models.Request;

public class ChangeUserPasswordModel
{
    public String? OldPassword { get; set; }
    public String? NewPassword { get; set; }
    public String? ConfirmPassword { get; set; }
}