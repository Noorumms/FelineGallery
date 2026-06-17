using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace Feline_Gallery_v1.Models.Repsitories
{
    public class ArtworkRepository : IArtworkRepository
    {
        private readonly string _connectionString;

        public ArtworkRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Artwork>> GetAllAsync()
        {
            using var conn = Connection;
            var sql = @"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId";
            return await conn.QueryAsync<Artwork>(sql);
        }

        public async Task<Artwork> GetByIdAsync(int id)
        {
            using var conn = Connection;
            var sql = @"SELECT a.*, c.*, ar.*
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                       WHERE a.ArtworkId = @Id";

            var artwork = await conn.QueryAsync<Artwork, Category, Artist, Artwork>(
                sql,
                (artwork, category, artist) =>
                {
                    artwork.Category = category;
                    artwork.Artist = artist;
                    return artwork;
                },
                new { Id = id },
                splitOn: "CategoryId,ArtistId"
            );

            return artwork.FirstOrDefault();
        }

        public async Task<int> AddAsync(Artwork entity)
        {
            using var conn = Connection;
            var sql = @"INSERT INTO Artworks (Title, Description, CategoryId, ArtistId, Price, ImageUrl, Year, Dimensions, IsAvailable, IsFeatured, CreatedDate)
                       VALUES (@Title, @Description, @CategoryId, @ArtistId, @Price, @ImageUrl, @Year, @Dimensions, @IsAvailable, @IsFeatured, @CreatedDate);
                       SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Artwork entity)
        {
            using var conn = Connection;
            var sql = @"UPDATE Artworks 
                       SET Title = @Title, Description = @Description, CategoryId = @CategoryId, 
                           ArtistId = @ArtistId, Price = @Price, ImageUrl = @ImageUrl, 
                           Year = @Year, Dimensions = @Dimensions, IsAvailable = @IsAvailable, 
                           IsFeatured = @IsFeatured
                       WHERE ArtworkId = @ArtworkId";
            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = Connection;
            var sql = "DELETE FROM Artworks WHERE ArtworkId = @Id";
            var result = await conn.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<Artwork>> GetFeaturedAsync()
        {
            using var conn = Connection;
            var sql = @"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                       WHERE a.IsFeatured = 1 AND a.IsAvailable = 1";
            return await conn.QueryAsync<Artwork>(sql);
        }

        public async Task<IEnumerable<Artwork>> GetByCategoryAsync(int categoryId)
        {
            using var conn = Connection;
            var sql = @"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                       WHERE a.CategoryId = @CategoryId AND a.IsAvailable = 1";
            return await conn.QueryAsync<Artwork>(sql, new { CategoryId = categoryId });
        }

        public async Task<IEnumerable<Artwork>> GetByArtistAsync(int artistId)
        {
            using var conn = Connection;
            var sql = @"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                       WHERE a.ArtistId = @ArtistId";
            return await conn.QueryAsync<Artwork>(sql, new { ArtistId = artistId });
        }

        public async Task<IEnumerable<Artwork>> SearchAsync(string searchTerm)
        {
            using var conn = Connection;
            var sql = @"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                       WHERE a.Title LIKE @Search OR a.Description LIKE @Search OR ar.Name LIKE @Search";
            return await conn.QueryAsync<Artwork>(sql, new { Search = $"%{searchTerm}%" });
        }

        public async Task<IEnumerable<Artwork>> GetAvailableAsync()
        {
            using var conn = Connection;
            var sql = @"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                       WHERE a.IsAvailable = 1";
            return await conn.QueryAsync<Artwork>(sql);
        }

        public async Task<IEnumerable<Artwork>> GetFilteredAsync(int? categoryId, decimal? minPrice, decimal? maxPrice, string sortBy)
        {
            using var conn = Connection;
            var sql = @"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                       WHERE a.IsAvailable = 1";

            if (categoryId.HasValue)
                sql += " AND a.CategoryId = @CategoryId";
            if (minPrice.HasValue)
                sql += " AND a.Price >= @MinPrice";
            if (maxPrice.HasValue)
                sql += " AND a.Price <= @MaxPrice";

            sql += sortBy switch
            {
                "price_asc" => " ORDER BY a.Price ASC",
                "price_desc" => " ORDER BY a.Price DESC",
                "newest" => " ORDER BY a.CreatedDate DESC",
                "title" => " ORDER BY a.Title ASC",
                _ => " ORDER BY a.CreatedDate DESC"
            };

            return await conn.QueryAsync<Artwork>(sql, new { CategoryId = categoryId, MinPrice = minPrice, MaxPrice = maxPrice });
        }

        // ============================================
        // NEW PAGINATED METHODS
        // ============================================

        public async Task<PaginatedList<Artwork>> GetAllPaginatedAsync(int pageIndex, int pageSize)
        {
            using var conn = Connection;

            // Get total count
            var countSql = "SELECT COUNT(*) FROM Artworks WHERE IsAvailable = 1";
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql);

            // Get paginated data
            var sql = @"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                       FROM Artworks a
                       LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                       LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                       WHERE a.IsAvailable = 1
                       ORDER BY a.CreatedDate DESC
                       OFFSET @Offset ROWS
                       FETCH NEXT @PageSize ROWS ONLY";

            var offset = (pageIndex - 1) * pageSize;
            var artworks = await conn.QueryAsync<Artwork>(sql, new { Offset = offset, PageSize = pageSize });

            return new PaginatedList<Artwork>(artworks.ToList(), totalCount, pageIndex, pageSize);
        }

        public async Task<PaginatedList<Artwork>> GetFilteredPaginatedAsync(int pageIndex, int pageSize, int? categoryId, decimal? minPrice, decimal? maxPrice, string sortBy)
        {
            using var conn = Connection;

            // Build WHERE clause
            var whereClause = "WHERE a.IsAvailable = 1";
            if (categoryId.HasValue)
                whereClause += " AND a.CategoryId = @CategoryId";
            if (minPrice.HasValue)
                whereClause += " AND a.Price >= @MinPrice";
            if (maxPrice.HasValue)
                whereClause += " AND a.Price <= @MaxPrice";

            // Get total count
            var countSql = $"SELECT COUNT(*) FROM Artworks a {whereClause}";
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, new { CategoryId = categoryId, MinPrice = minPrice, MaxPrice = maxPrice });

            // Build ORDER BY clause
            var orderBy = sortBy switch
            {
                "price_asc" => "ORDER BY a.Price ASC",
                "price_desc" => "ORDER BY a.Price DESC",
                "newest" => "ORDER BY a.CreatedDate DESC",
                "title" => "ORDER BY a.Title ASC",
                _ => "ORDER BY a.CreatedDate DESC"
            };

            // Get paginated data
            var sql = $@"SELECT a.*, c.Name as CategoryName, ar.Name as ArtistName 
                        FROM Artworks a
                        LEFT JOIN Categories c ON a.CategoryId = c.CategoryId
                        LEFT JOIN Artists ar ON a.ArtistId = ar.ArtistId
                        {whereClause}
                        {orderBy}
                        OFFSET @Offset ROWS
                        FETCH NEXT @PageSize ROWS ONLY";

            var offset = (pageIndex - 1) * pageSize;
            var artworks = await conn.QueryAsync<Artwork>(sql, new
            {
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Offset = offset,
                PageSize = pageSize
            });

            return new PaginatedList<Artwork>(artworks.ToList(), totalCount, pageIndex, pageSize);
        }
    }
}