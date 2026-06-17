namespace Feline_Gallery_v1.Models
{
    public class Exhibition
    {
        public int ExhibitionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ExhibitionArtwork> ExhibitionArtworks { get; set; }
    }
}
