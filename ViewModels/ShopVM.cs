using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.ViewModels
{
    public class ShopVM
    {
        public List<Artwork> Artworks { get; set; }
        public List<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortBy { get; set; } // "price_asc", "price_desc", "newest", "title"

        // Pagination
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
