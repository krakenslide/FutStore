using System.Text.Json;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public static class BrandContextSeed
{
    public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
    {
        bool checkBrands = brandCollection.Find(b => true).Any();
        string path = Path.Combine("Data", "SeedData", "brands.json");
        ///src/Services/Catalog/Catalog.Infrastructure/Data/SeedData
        string newpath = Path.Combine(SharedPath.BasePath, "Data", "SeedData", "brands.json");
        string dockerPath = Path.Combine(SharedPath.DockerPath, "brands.json");
        if (!checkBrands)
        {
            var brandsData = File.ReadAllText(dockerPath);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            if (brands != null)
            {
                foreach (var item in brands)
                {
                    brandCollection.InsertOneAsync(item);
                }
            }
        }
    } 
}