using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShopAPI.Data.dto.Entities;

[Table("coffee_shop_orders")]
public class Order
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Moment { get; set; }
    public String Status { get; set; } = null!;
    public String? Comment { get; set; }
    public List<OrderItem> Items { get; set; } = null!;
}