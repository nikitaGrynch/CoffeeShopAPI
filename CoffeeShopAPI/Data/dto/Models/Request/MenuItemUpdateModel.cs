namespace CoffeeShopAPI.Data.dto.Models.Request;

public class MenuItemUpdateModel
{
    public String? Name { get; set; }
    public String? Description { get; set; }
    public float? Price { get; set; }
    public String? CategoryId { get; set; }
    public List<String>? AvailableAdditivesIds { get; set; } 
}