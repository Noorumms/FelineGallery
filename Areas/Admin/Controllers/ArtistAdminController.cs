using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Areas.Admin.Controllers
{
    public class ArtistAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
