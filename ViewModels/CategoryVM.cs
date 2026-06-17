using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.ViewModels
{
    public class CategoryVM
    {
        public Category Category { get; set; }
        public List<Artwork> Artworks { get; set; }
        public int TotalArtworks { get; set; }
    }
}
