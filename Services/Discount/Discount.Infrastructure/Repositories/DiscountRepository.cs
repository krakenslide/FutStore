using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)", new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

                /*if (affected == 0)
                {
                    return false;
                }
                return true;*/
                return affected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in creating coupon: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

                return affected > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in deleting coupon: {ex.Message}");
                return false;
            }
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
                if (coupon == null)
                {
                    return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Available" };
                }
                return coupon;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in getting discount: {ex.Message}");
                return new Coupon { ProductName = "Error", Amount = 0, Description = "Error in retrieving discount" };
            }
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            {
                try
                {
                    await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                    var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id", new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

                    return affected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in updating coupon: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
