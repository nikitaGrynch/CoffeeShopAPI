using CoffeeShopAPI.Data.dto.Entities;

namespace CoffeeShopAPI.Data.dto.Models;

public class OrderModel
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Moment { get; set; }
    public String Status { get; set; } = null!;
    public String? Comment { get; set; }
    public List<OrderItemModel> Items { get; set; } = null!;
    public float TotalPrice { get; set; }
    
    public OrderModel(Order order)
    {
        Id = order.Id;
        UserId = order.UserId;
        Moment = order.Moment;
        Status = order.Status;
        Comment = order.Comment;
        Items = order.Items.Select(oi => new OrderItemModel(oi)).ToList();
        float totalPrice = 0;
        foreach (OrderItemModel oi in Items)
        {
            if (oi.Additives != null)
            {
                foreach (var additive in oi.Additives)
                {
                    if (additive != null)
                    {
                        totalPrice += additive.Price * oi.Quantity;
                    }
                }
            }
            totalPrice += oi.Item.Price * oi.Quantity;
        }
        TotalPrice = totalPrice;
    }
}