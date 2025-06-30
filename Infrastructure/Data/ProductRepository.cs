using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext storeContext) : IProductRepository
{
    public void CreateProduct(Product product)
    {
        storeContext.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        storeContext.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetProductBrandsAsync()
    {
        return await storeContext.Products
            .Select(x => x.Brand)
            .Distinct()
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await storeContext.Products.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        /* IReadOnlyList<Product> products = await storeContext.Products
            .Where(x => (string.IsNullOrEmpty(brand) || x.Brand == brand) &&
                        (string.IsNullOrEmpty(type) || x.Type == type))
            .ToListAsync();*/

        IQueryable<Product> queryable = storeContext.Products.AsQueryable();
        if (!string.IsNullOrEmpty(brand))
        {
            queryable = queryable.Where(x => x.Brand == brand);
        }
        if (!string.IsNullOrEmpty(type))
        {
            queryable = queryable.Where(x => x.Type == type);
        }
        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort.ToLower())
            {
                case "priceasc":
                    queryable = queryable.OrderBy(x => x.Price);
                    break;
                case "pricedesc":
                    queryable = queryable.OrderByDescending(x => x.Price);
                    break;
                default:
                    queryable = queryable.OrderBy(x => x.Name);
                    break;
            }
        }
        return await queryable.ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetProductsByBrandAsync(string brand)
    {
        return await storeContext.Products
            .Where(x => x.Brand == brand)
            .ToListAsync();        
    }

    public async Task<IReadOnlyList<Product>> GetProductsByTypeAsync(string type)
    {
        return await storeContext.Products
            .Where(x => x.Type == type)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetProductTypesAsync()
    {
        return await storeContext.Products
            .Select(x => x.Type)
            .Distinct()
            .ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return storeContext.Products.Any(x => x.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await storeContext.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        storeContext.Entry(product).State = EntityState.Modified;
        // Alternatively, you can use:
        // storeContext.Products.Update(product);
        // This will also mark the entity as modified.
    }
}
