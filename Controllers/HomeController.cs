using Feline_Gallery_v1.Data;
using Feline_Gallery_v1.Models.Interfaces;
using Feline_Gallery_v1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArtworkRepository _artworkRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IExhibitionRepository _exhibitionRepo;

        public HomeController(
            IArtworkRepository artworkRepo,
            ICategoryRepository categoryRepo,
            IExhibitionRepository exhibitionRepo)
        {
            _artworkRepo = artworkRepo;
            _categoryRepo = categoryRepo;
            _exhibitionRepo = exhibitionRepo;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeVM
            {
                FeaturedArtworks = (await _artworkRepo.GetFeaturedAsync()).ToList(),
                Categories = (await _categoryRepo.GetActiveAsync()).ToList(),
                UpcomingExhibitions = (await _exhibitionRepo.GetUpcomingAsync()).Take(3).ToList()
            };

            return View(viewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}