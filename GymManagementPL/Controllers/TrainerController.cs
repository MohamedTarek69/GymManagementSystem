using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetTrainer()
        {
            return View();
        }

        public IActionResult CreateTrainer()
        {
            return View();
        }

    }
}
