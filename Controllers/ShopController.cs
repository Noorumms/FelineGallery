using Feline_Gallery_v1.Models;
using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Controllers
{
    public class ShopController : Controller
    {
        private readonly IArtworkRepository _artworkRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ShopController(IArtworkRepository artworkRepo, ICategoryRepository categoryRepo)
        {
            _artworkRepo = artworkRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index(int? categoryId, string priceRange, string sortBy = "newest", int page = 1)
        {
            const int pageSize = 12;

            // Parse price range
            decimal? minPrice = null;
            decimal? maxPrice = null;

            if (!string.IsNullOrEmpty(priceRange))
            {
                var prices = priceRange.Split('-');
                if (prices.Length == 2)
                {
                    minPrice = decimal.Parse(prices[0]);
                    maxPrice = decimal.Parse(prices[1]);
                }
            }

            // Get paginated artworks
            var paginatedArtworks = await _artworkRepo.GetFilteredPaginatedAsync(
                page, pageSize, categoryId, minPrice, maxPrice, sortBy);

            // Get categories for filter
            var categories = await _categoryRepo.GetActiveAsync();

            // Pass data to view
            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = categoryId;
            ViewBag.CurrentPriceRange = priceRange;
            ViewBag.CurrentSort = sortBy;

            return View(paginatedArtworks);
        }
    }
}