using System.Text.Json;
using Core.Entities; // Add this line or replace with the correct namespace for Product

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext storeContext) {
        if (!storeContext.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products == null) {
                return;
            }
            storeContext.Products.AddRange(products);
            await storeContext.SaveChangesAsync();
        }
    }
}
