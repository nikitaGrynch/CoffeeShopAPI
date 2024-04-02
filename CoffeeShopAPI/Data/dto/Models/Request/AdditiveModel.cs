namespace CoffeeShopAPI.Data.dto.Models.Request;

public class AdditiveModel
{
    public String Name { get; set; } = null!;
    public float Price { get; set; }
    public String? Description { get; set; }
}