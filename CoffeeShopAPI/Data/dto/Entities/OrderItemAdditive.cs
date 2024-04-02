using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopAPI.Data.dto.Entities;

[Table("coffee_shop_order_items_additives")]
public class OrderItemAdditive
{
    public Guid OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; } = null!;
    public Guid AdditiveId { get; set; }
    public Additive Additive { get; set; } = null!;
}