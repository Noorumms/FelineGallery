namespace Feline_Gallery_v1.Models.Interfaces
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Task<Artist> GetWithArtworksAsync(int artistId);
        Task<IEnumerable<Artist>> SearchAsync(string searchTerm);
    }
}
