using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopAPI.Data.dto.Entities;

[Table("coffee_shop_users")]
public class User
{
    public Guid Id { get; set; }
    public String Name { get; set; } = null!;
    public String PhoneNumber { get; set; } = null!;
    public String PasswordHash { get; set; } = null!;
    public DateTime RegisterDt { get; set; }
}