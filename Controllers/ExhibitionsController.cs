using Feline_Gallery_v1.Data;
using Feline_Gallery_v1.Models.Interfaces;
using Feline_Gallery_v1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Controllers
{
    public class ExhibitionsController : Controller
    {
        private readonly IExhibitionRepository _exhibitionRepo;

        public ExhibitionsController(IExhibitionRepository exhibitionRepo)
        {
            _exhibitionRepo = exhibitionRepo;
        }

        public async Task<IActionResult> Index()
        {
            var exhibitions = await _exhibitionRepo.GetActiveAsync();
            return View(exhibitions);
        }

       
    }
}