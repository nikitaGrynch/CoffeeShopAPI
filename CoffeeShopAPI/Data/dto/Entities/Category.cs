using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopAPI.Data.dto.Entities;

[Table("coffee_shop_categories")]
public class Category
{
    public Guid Id { get; set; }
    public String Name { get; set; } = null!;
    public String? Description { get; set; }
}