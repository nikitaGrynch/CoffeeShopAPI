namespace CoffeeShopAPI.Data.dto.Models.Request;

public class OrderItemCreationModel
{
    public String MenuItemId { get; set; } = null!;
    public List<String>? AdditivesIds { get; set; }
    public int Quantity { get; set; }
}