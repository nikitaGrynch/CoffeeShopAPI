using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopAPI.Data.dto.Entities;

[Table("coffee_shop_orders_items")]
public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ItemId { get; set; }
    public MenuItem Item { get; set; } = null!;
    public List<OrderItemAdditive>? Additives { get; set; }
    public int Quantity { get; set; }
}