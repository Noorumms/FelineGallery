using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.ViewModels
{
    public class HomeVM
    {

        public List<Artwork> FeaturedArtworks { get; set; }
        public List<Category> Categories { get; set; }
        public List<Exhibition> UpcomingExhibitions { get; set; }
        public List<Artist> FeaturedArtists { get; set; }
    }
}
