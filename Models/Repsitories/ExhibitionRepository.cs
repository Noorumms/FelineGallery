using Dapper;
using Feline_Gallery_v1.Models;
using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Feline_Gallery_v1.Data
{
    public class ExhibitionRepository : IExhibitionRepository
    {
        private readonly string _connectionString;

        public ExhibitionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Exhibition>> GetAllAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Exhibition>("SELECT * FROM Exhibitions ORDER BY StartDate DESC");
        }

        public async Task<Exhibition> GetByIdAsync(int id)
        {
            using var conn = Connection;
            return await conn.QueryFirstOrDefaultAsync<Exhibition>("SELECT * FROM Exhibitions WHERE ExhibitionId = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Exhibition entity)
        {
            using var conn = Connection;
            var sql = "INSERT INTO Exhibitions (Title, Description, StartDate, EndDate, ImageUrl, IsActive) VALUES (@Title, @Description, @StartDate, @EndDate, @ImageUrl, @IsActive); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Exhibition entity)
        {
            using var conn = Connection;
            var sql = "UPDATE Exhibitions SET Title = @Title, Description = @Description, StartDate = @StartDate, EndDate = @EndDate, ImageUrl = @ImageUrl, IsActive = @IsActive WHERE ExhibitionId = @ExhibitionId";
            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = Connection;
            var result = await conn.ExecuteAsync("DELETE FROM Exhibitions WHERE ExhibitionId = @Id", new { Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<Exhibition>> GetActiveAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Exhibition>("SELECT * FROM Exhibitions WHERE IsActive = 1 ORDER BY StartDate DESC");
        }

        public async Task<IEnumerable<Exhibition>> GetUpcomingAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Exhibition>("SELECT * FROM Exhibitions WHERE StartDate > GETDATE() AND IsActive = 1 ORDER BY StartDate ASC");
        }

        public async Task<Exhibition> GetWithArtworksAsync(int exhibitionId)
        {
            using var conn = Connection;
            var sql = @"SELECT * FROM Exhibitions WHERE ExhibitionId = @ExhibitionId;
                       SELECT a.* FROM Artworks a
                       INNER JOIN ExhibitionArtworks ea ON a.ArtworkId = ea.ArtworkId
                       WHERE ea.ExhibitionId = @ExhibitionId";

            using var multi = await conn.QueryMultipleAsync(sql, new { ExhibitionId = exhibitionId });
            var exhibition = await multi.ReadFirstOrDefaultAsync<Exhibition>();
            if (exhibition != null)
            {
                var artworks = await multi.ReadAsync<Artwork>();
                exhibition.ExhibitionArtworks = artworks.Select(a => new ExhibitionArtwork
                {
                    ExhibitionId = exhibitionId,
                    ArtworkId = a.ArtworkId,
                    Artwork = a
                }).ToList();
            }
            return exhibition;
        }

        public async Task<bool> AddArtworkToExhibitionAsync(int exhibitionId, int artworkId)
        {
            using var conn = Connection;
            var sql = "INSERT INTO ExhibitionArtworks (ExhibitionId, ArtworkId) VALUES (@ExhibitionId, @ArtworkId)";
            var result = await conn.ExecuteAsync(sql, new { ExhibitionId = exhibitionId, ArtworkId = artworkId });
            return result > 0;
        }

        public async Task<bool> RemoveArtworkFromExhibitionAsync(int exhibitionId, int artworkId)
        {
            using var conn = Connection;
            var sql = "DELETE FROM ExhibitionArtworks WHERE ExhibitionId = @ExhibitionId AND ArtworkId = @ArtworkId";
            var result = await conn.ExecuteAsync(sql, new { ExhibitionId = exhibitionId, ArtworkId = artworkId });
            return result > 0;
        }
    }
}