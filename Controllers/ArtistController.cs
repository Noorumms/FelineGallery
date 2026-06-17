using Feline_Gallery_v1.Data;
using Feline_Gallery_v1.Models.Interfaces;
using Feline_Gallery_v1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Controllers
{
    public class ArtistController : Controller
    {
        private readonly IArtistRepository _artistRepo;

        public ArtistController(IArtistRepository artistRepo)
        {
            _artistRepo = artistRepo;
        }

        public async Task<IActionResult> Profile(int id)
        {
            var artist = await _artistRepo.GetWithArtworksAsync(id);

            if (artist == null)
            {
                return NotFound();
            }

            var viewModel = new ArtistProfileVM
            {
                Artist = artist,
                Artworks = artist.Artworks?.ToList() ?? new List<Models.Artwork>(),
                TotalArtworks = artist.Artworks?.Count() ?? 0,
                AvailableArtworks = artist.Artworks?.Count(a => a.IsAvailable) ?? 0
            };

            return View(viewModel);
        }
    }
}