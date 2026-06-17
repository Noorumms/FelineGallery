using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.ViewModels
{
    public class SearchResultVM
    {
        public string SearchTerm { get; set; }
        public List<Artwork> Artworks { get; set; }
        public List<Artist> Artists { get; set; }
        public List<Exhibition> Exhibitions { get; set; }
        public int TotalResults { get; set; }
    }
}
