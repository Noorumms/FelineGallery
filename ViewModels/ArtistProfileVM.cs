using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.ViewModels
{
    public class ArtistProfileVM
    {
        public Artist Artist { get; set; }
        public List<Artwork> Artworks { get; set; }
        public int TotalArtworks { get; set; }
        public int AvailableArtworks { get; set; }
    }
}
