using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CoffeeShopAPI.Data.dto.Entities;

[Table("coffee_shop_menu_items")]
public class MenuItem
{
    public Guid Id { get; set; }
    public String Name { get; set; } = null!;
    public String? Description { get; set; }
    public float Price { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public List<MenuItemAdditive> AvailableAdditives { get; set; } = null!;
}