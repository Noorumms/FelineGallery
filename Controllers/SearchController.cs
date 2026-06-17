using Feline_Gallery_v1.Data;
using Feline_Gallery_v1.Models.Interfaces;
using Feline_Gallery_v1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Controllers
{
    public class SearchController : Controller
    {
        private readonly IArtworkRepository _artworkRepo;
        private readonly IArtistRepository _artistRepo;
        private readonly IExhibitionRepository _exhibitionRepo;

        public SearchController(
            IArtworkRepository artworkRepo,
            IArtistRepository artistRepo,
            IExhibitionRepository exhibitionRepo)
        {
            _artworkRepo = artworkRepo;
            _artistRepo = artistRepo;
            _exhibitionRepo = exhibitionRepo;
        }

        public async Task<IActionResult> Result(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return RedirectToAction("Index", "Home");
            }

            var artworks = await _artworkRepo.SearchAsync(q);
            var artists = await _artistRepo.SearchAsync(q);

            var viewModel = new SearchResultVM
            {
                SearchTerm = q,
                Artworks = artworks.ToList(),
                Artists = artists.ToList(),
                Exhibitions = new List<Models.Exhibition>(),
                TotalResults = artworks.Count() + artists.Count()
            };

            return View(viewModel);
        }
    }
}