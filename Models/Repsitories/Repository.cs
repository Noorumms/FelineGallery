using Dapper;
using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Feline_Gallery_v1.Models.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public Repository(IConfiguration configuration, string tableName)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tableName = tableName;
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var conn = Connection;
            var sql = $"SELECT * FROM {_tableName}";
            return await conn.QueryAsync<T>(sql);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using var conn = Connection;
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id"; // assumes PK column is 'Id'
            return await conn.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(T entity)
        {
            using var conn = Connection;

            // Generate INSERT dynamically using reflection (simple approach)
            var properties = typeof(T).GetProperties()
                                      .Where(p => p.Name.ToLower() != "id") // skip Id
                                      .ToList();

            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));
            var sql = $"INSERT INTO {_tableName} ({columnNames}) VALUES ({paramNames}); SELECT CAST(SCOPE_IDENTITY() as int)";

            return await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            using var conn = Connection;

            var properties = typeof(T).GetProperties()
                                      .Where(p => p.Name.ToLower() != "id")
                                      .ToList();

            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            var sql = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id"; // assumes PK column is 'Id'

            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = Connection;
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id"; // assumes PK column is 'Id'
            var result = await conn.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }
    }
}
