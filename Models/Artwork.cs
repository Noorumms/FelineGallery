namespace Feline_Gallery_v1.Models
{
    public class Artwork
    {
        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ArtistId { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int? Year { get; set; }
        public string Dimensions { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; }

        public Category Category { get; set; }
        public Artist Artist { get; set; }
    }
}
