
using Feline_Gallery_v1.Data;
using Feline_Gallery_v1.Models.Interfaces;
using Feline_Gallery_v1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Controllers
{
    public class ArtworkController : Controller
    {
        private readonly IArtworkRepository _artworkRepo;

        public ArtworkController(IArtworkRepository artworkRepo)
        {
            _artworkRepo = artworkRepo;
        }

        public async Task<IActionResult> Details(int id)
        {
            var artwork = await _artworkRepo.GetByIdAsync(id);

            if (artwork == null)
            {
                return NotFound();
            }

            var relatedArtworks = await _artworkRepo.GetByCategoryAsync(artwork.CategoryId);

            var viewModel = new ArtworkDetailsVM
            {
                Artwork = artwork,
                Artist = artwork.Artist,
                Category = artwork.Category,
                RelatedArtworks = relatedArtworks.Where(a => a.ArtworkId != id).Take(4).ToList()
            };

            return View(viewModel);
        }
    }
}