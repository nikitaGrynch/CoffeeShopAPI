namespace CoffeeShopAPI.Data.dto.Models.Request;

public class OrderCreationModel
{
    public String? UserId { get; set; }
    public String? Comment { get; set; }
    public List<OrderItemCreationModel> Items { get; set; } = null!;
}