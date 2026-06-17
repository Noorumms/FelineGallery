using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IArtworkRepository _artworkRepo;

        public CategoryController(ICategoryRepository categoryRepo, IArtworkRepository artworkRepo)
        {
            _categoryRepo = categoryRepo;
            _artworkRepo = artworkRepo;
        }

        // Show all categories
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepo.GetActiveAsync();
            return View(categories);
        }

        // Show artworks in specific category with pagination
        public async Task<IActionResult> Details(int id, string sortBy = "newest", int page = 1)
        {
            const int pageSize = 12;

            // Get category info
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Get paginated artworks for this category
            var paginatedArtworks = await _artworkRepo.GetFilteredPaginatedAsync(
                page, pageSize, id, null, null, sortBy);

            ViewBag.Category = category;
            ViewBag.CurrentSort = sortBy;

            return View(paginatedArtworks);
        }
    }
}