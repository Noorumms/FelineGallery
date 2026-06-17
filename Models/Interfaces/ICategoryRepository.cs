namespace Feline_Gallery_v1.Models.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveAsync();
        Task<Category> GetWithArtworksAsync(int categoryId);
    }
}
