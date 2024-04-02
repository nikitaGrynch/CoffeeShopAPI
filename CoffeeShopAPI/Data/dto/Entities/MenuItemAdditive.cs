using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopAPI.Data.dto.Entities;

[Table("coffee_shop_menu_items_additives")]
public class MenuItemAdditive
{
    public Guid MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;
    public Guid AdditiveId { get; set; }
    public Additive Additive { get; set; } = null!;
}