using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }
    public IMongoCollection<ProductBrand> Brands { get; }
    public IMongoCollection<ProductType> Types { get; }

    public CatalogContext(IConfiguration configuration)
    {
        string C_Value = configuration.GetValue<string>("DatabaseSettings:ConnectionStrings");
        string apiKey = Environment.GetEnvironmentVariable("DatabaseSettings__ConnectionString");
        var client = new MongoClient(apiKey);
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        try
        {
            var serverStatus = database.RunCommandAsync((Command<BsonDocument>)"{ serverStatus: 1 }").Result;
        }
        catch (MongoException ex) 
        {
            Console.WriteLine(ex.Message);
        }
        Brands = database.GetCollection<ProductBrand>(
            configuration.GetValue<string>("DatabaseSettings:BrandsCollection"));
        Types = database.GetCollection<ProductType>(
            configuration.GetValue<string>("DatabaseSettings:TypesCollection"));
        Products = database.GetCollection<Product>(
            configuration.GetValue<string>("DatabaseSettings:CollectionName"));

        BrandContextSeed.SeedData(Brands);
        TypeContextSeed.SeedData(Types);
        CatalogContextSeed.SeedData(Products);
    }
}