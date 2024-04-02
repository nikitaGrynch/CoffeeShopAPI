namespace CoffeeShopAPI.Services.Hash;

public interface IHashService
{
    String Hash(String text);
    Boolean Verify(String text, String hash);
}