using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Controllers
{
    public class TestController : Controller
    {
        private readonly IArtworkRepository _artworkRepo;

        public TestController(IArtworkRepository artworkRepo)
        {
            _artworkRepo = artworkRepo;
        }

        public async Task<IActionResult> TestRepository()
        {
            // Test GetAll
            var allArtworks = await _artworkRepo.GetAllAsync();

            // Test GetById
            var artwork = await _artworkRepo.GetByIdAsync(1);

            // Test GetFeatured
            var featured = await _artworkRepo.GetFeaturedAsync();

            // Return JSON to see results
            return Json(new
            {
                TotalArtworks = allArtworks.Count(),
                FirstArtwork = artwork,
                FeaturedCount = featured.Count()
            });
        }
    }
}
