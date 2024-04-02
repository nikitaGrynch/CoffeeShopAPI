namespace CoffeeShopAPI.Services.Hash;

public class SHA1HashService : IHashService
{
    public string Hash(string text)
    {
        using var sha1 = System.Security.Cryptography.SHA1.Create();
        return 
            Convert.ToHexString(
                sha1.ComputeHash(
                    System.Text.Encoding.UTF8.GetBytes(
                        text)));
    }

    public bool Verify(string text, string hash)
    {
        return Hash(text) == hash;
    }
}