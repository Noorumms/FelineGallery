namespace Feline_Gallery_v1.Models.Interfaces
{
    public interface IExhibitionRepository : IRepository<Exhibition>
    {
        Task<IEnumerable<Exhibition>> GetActiveAsync();
        Task<IEnumerable<Exhibition>> GetUpcomingAsync();
        Task<Exhibition> GetWithArtworksAsync(int exhibitionId);
        Task<bool> AddArtworkToExhibitionAsync(int exhibitionId, int artworkId);
        Task<bool> RemoveArtworkFromExhibitionAsync(int exhibitionId, int artworkId);
    }
}
