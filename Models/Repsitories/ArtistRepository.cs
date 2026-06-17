using Dapper;
using Feline_Gallery_v1.Models;
using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Feline_Gallery_v1.Data
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly string _connectionString;

        public ArtistRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Artist>("SELECT * FROM Artists");
        }

        public async Task<Artist> GetByIdAsync(int id)
        {
            using var conn = Connection;
            return await conn.QueryFirstOrDefaultAsync<Artist>("SELECT * FROM Artists WHERE ArtistId = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Artist entity)
        {
            using var conn = Connection;
            var sql = "INSERT INTO Artists (Name, Bio, ProfileImageUrl, Email, Phone) VALUES (@Name, @Bio, @ProfileImageUrl, @Email, @Phone); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Artist entity)
        {
            using var conn = Connection;
            var sql = "UPDATE Artists SET Name = @Name, Bio = @Bio, ProfileImageUrl = @ProfileImageUrl, Email = @Email, Phone = @Phone WHERE ArtistId = @ArtistId";
            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = Connection;
            var result = await conn.ExecuteAsync("DELETE FROM Artists WHERE ArtistId = @Id", new { Id = id });
            return result > 0;
        }

        public async Task<Artist> GetWithArtworksAsync(int artistId)
        {
            using var conn = Connection;
            var sql = @"SELECT * FROM Artists WHERE ArtistId = @ArtistId;
                       SELECT * FROM Artworks WHERE ArtistId = @ArtistId";

            using var multi = await conn.QueryMultipleAsync(sql, new { ArtistId = artistId });
            var artist = await multi.ReadFirstOrDefaultAsync<Artist>();
            if (artist != null)
            {
                artist.Artworks = (await multi.ReadAsync<Artwork>()).ToList();
            }
            return artist;
        }

        public async Task<IEnumerable<Artist>> SearchAsync(string searchTerm)
        {
            using var conn = Connection;
            return await conn.QueryAsync<Artist>("SELECT * FROM Artists WHERE Name LIKE @Search", new { Search = $"%{searchTerm}%" });
        }
    }
}