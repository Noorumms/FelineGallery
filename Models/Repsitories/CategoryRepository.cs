using Feline_Gallery_v1.Models;
using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace Feline_Gallery_v1.Models.Repsitories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var conn = Connection;
            var sql = "SELECT * FROM Categories ORDER BY Name";
            return await conn.QueryAsync<Category>(sql);
        }

        public async Task<IEnumerable<Category>> GetActiveAsync()
        {
            using var conn = Connection;
            var sql = "SELECT * FROM Categories WHERE IsActive = 1 ORDER BY Name";
            return await conn.QueryAsync<Category>(sql);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            using var conn = Connection;
            var sql = "SELECT * FROM Categories WHERE CategoryId = @Id";
            return await conn.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<Category> GetWithArtworksAsync(int id)
        {
            using var conn = Connection;

            // Get category
            var categorySql = "SELECT * FROM Categories WHERE CategoryId = @Id";
            var category = await conn.QueryFirstOrDefaultAsync<Category>(categorySql, new { Id = id });

            if (category != null)
            {
                // Get artworks for this category
                var artworksSql = @"SELECT a.*, ar.Name as ArtistName 
                                   FROM Artworks a
                                   LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                                   WHERE a.CategoryId = @CategoryId AND a.IsAvailable = 1
                                   ORDER BY a.CreatedDate DESC";

                var artworks = await conn.QueryAsync<Artwork>(artworksSql, new { CategoryId = id });
                category.Artworks = artworks.ToList();
            }

            return category;
        }

        public async Task<int> AddAsync(Category entity)
        {
            using var conn = Connection;
            var sql = @"INSERT INTO Categories (Name, Description, ImageUrl, IsActive)
                       VALUES (@Name, @Description, @ImageUrl, @IsActive);
                       SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            using var conn = Connection;
            var sql = @"UPDATE Categories 
                       SET Name = @Name, Description = @Description, 
                           ImageUrl = @ImageUrl, IsActive = @IsActive
                       WHERE CategoryId = @CategoryId";
            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = Connection;
            var sql = "DELETE FROM Categories WHERE CategoryId = @Id";
            var result = await conn.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }
}