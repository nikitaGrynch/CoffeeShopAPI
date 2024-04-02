namespace CoffeeShopAPI.Data.dto.Models;

public class MenuItemCreationModel
{
    public String Name { get; set; }
    public String? Description { get; set; }
    public float Price { get; set; }
    public String CategoryId { get; set; }
    public List<String> AvailableAdditivesIds { get; set; }
    
}