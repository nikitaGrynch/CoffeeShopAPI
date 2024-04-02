using CoffeeShopAPI.Data.dto.Entities;
using CoffeeShopAPI.Data.dto.Models.Response;

namespace CoffeeShopAPI.Data.dto.Models;

public class OrderItemModel
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ItemId { get; set; }
    public MenuItemModel Item { get; set; } = null!;
    public List<Additive>? Additives { get; set; }
    public int Quantity { get; set; }
    
    public OrderItemModel(OrderItem orderItem)
    {
        Id = orderItem.Id;
        OrderId = orderItem.OrderId;
        ItemId = orderItem.ItemId;
        Item = new MenuItemModel(orderItem.Item);
        Additives = orderItem.Additives.Select(oi => oi.Additive).ToList();
        Quantity = orderItem.Quantity;
    }
}