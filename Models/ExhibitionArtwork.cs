namespace Feline_Gallery_v1.Models
{
    public class ExhibitionArtwork
    {
        public int ExhibitionId { get; set; }
        public int ArtworkId { get; set; }

        public Exhibition Exhibition { get; set; }
        public Artwork Artwork { get; set; }
    }
}
