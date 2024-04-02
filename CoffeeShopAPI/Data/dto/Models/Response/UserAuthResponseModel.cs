using CoffeeShopAPI.Data.dto.Entities;

namespace CoffeeShopAPI.Data.dto.Models.Response;

public class UserAuthResponseModel : ResponseModel
{
    public User? User { get; set; }
}