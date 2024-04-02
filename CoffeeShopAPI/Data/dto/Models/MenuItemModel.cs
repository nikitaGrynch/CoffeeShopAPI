using CoffeeShopAPI.Data.dto.Entities;

namespace CoffeeShopAPI.Data.dto.Models;

public class MenuItemModel
{
    public Guid Id { get; set; }
    public String Name { get; set; } = null!;
    public String? Description { get; set; }
    public float Price { get; set; }
    public Category Category { get; set; } = null!;
    public List<Additive> AvailableAdditives { get; set; } = null!;

    public MenuItemModel(MenuItem menuItem)
    {
        Id = menuItem.Id;
        Name = menuItem.Name;
        Description = menuItem.Description;
        Price = menuItem.Price;
        Category = menuItem.Category;
        AvailableAdditives = menuItem.AvailableAdditives.Select(aa => aa.Additive).ToList();
    }

    public MenuItemModel()
    {
        
    }
}