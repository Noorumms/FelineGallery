using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.ViewModels
{
    public class ArtworkDetailsVM
    {
        public Artwork Artwork { get; set; }
        public Artist Artist { get; set; }
        public Category Category { get; set; }
        public List<Artwork> RelatedArtworks { get; set; }
    }
}
