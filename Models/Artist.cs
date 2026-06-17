namespace Feline_Gallery_v1.Models
{

    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Artwork> Artworks { get; set; }
    }
}
