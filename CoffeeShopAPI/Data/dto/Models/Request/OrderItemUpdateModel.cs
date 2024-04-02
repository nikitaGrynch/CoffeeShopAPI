namespace CoffeeShopAPI.Data.dto.Models.Request;

public class OrderItemUpdateModel
{
    public List<String>? AdditivesIds { get; set; }
    public int? Quantity { get; set; }
}