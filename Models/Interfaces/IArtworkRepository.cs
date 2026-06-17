using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.Models.Interfaces
{
    public interface IArtworkRepository : IRepository<Artwork>
    {
        Task<IEnumerable<Artwork>> GetFeaturedAsync();
        Task<IEnumerable<Artwork>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Artwork>> GetByArtistAsync(int artistId);
        Task<IEnumerable<Artwork>> SearchAsync(string searchTerm);
        Task<IEnumerable<Artwork>> GetAvailableAsync();
        Task<IEnumerable<Artwork>> GetFilteredAsync(int? categoryId, decimal? minPrice, decimal? maxPrice, string sortBy);

        // NEW: Paginated methods
        Task<PaginatedList<Artwork>> GetAllPaginatedAsync(int pageIndex, int pageSize);
        Task<PaginatedList<Artwork>> GetFilteredPaginatedAsync(int pageIndex, int pageSize, int? categoryId, decimal? minPrice, decimal? maxPrice, string sortBy);
    }
}