using System.Threading.Tasks;
using Dapper;
using Wx.Demo.Api.Data;
using Wx.Demo.Api.Models;

namespace Wx.Demo.Api.Services
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(int id);
        Task<int> Create(string name, string description);
        Task<bool> Update(Product product);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly IDatabaseConnectionProvider _databaseConnectionProvider;

        public ProductRepository(IDatabaseConnectionProvider databaseConnectionProvider)
        {
            _databaseConnectionProvider = databaseConnectionProvider;
        }

        public async Task<Product> GetProduct(int id)
        {
            using var connection = await _databaseConnectionProvider.GetOpenConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(
                "SELECT Id, Name, Description FROM Product WHERE Id=@id", new { id });

        }

        public async Task<bool> Update(Product product)
        {
            using var connection = await _databaseConnectionProvider.GetOpenConnection();

            var updatedCount= await connection.ExecuteAsync(
                "UPDATE Product SET Name=@name, Description=@description WHERE Id=@id",
                new { name = product.Name, description = product.Descrition, id = product.Id });
            return updatedCount == 1;
        }

        public async Task<int> Create(string name, string description)
        {
            using var connection = await _databaseConnectionProvider.GetOpenConnection();

            var sqlStatement =
                @"INSERT INTO Product VALUES (@name, @description);
                  SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(
                sqlStatement,
                new { name, description });
        }
    }
}
