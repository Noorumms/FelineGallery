using Microsoft.AspNetCore.Mvc;

namespace Feline_Gallery_v1.Areas.Admin.Controllers
{
    public class ExhibitionAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
