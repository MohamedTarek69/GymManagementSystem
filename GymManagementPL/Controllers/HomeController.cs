using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        // Action 
        // BaseURL/Home/Index
        //  [NonAction]

        public HomeController(IAnalyticsService analyticsService) 
        {
            _analyticsService = analyticsService;
        }

        public IActionResult Index()
        {
            var analyticsData = _analyticsService.GetAnalyticsData();
            return View(analyticsData);
        }

        public IActionResult Trainers()
        {
            var Trainers = new List<Trainer>()
            {
                new Trainer(){ Name = "Mohamed Tarek", Phone = "01002097078" },
                new Trainer(){ Name = "Ahmed Samy", Phone = "01016334658" }
            };
            return Json(Trainers); 
        }

        public IActionResult Redirect()
        {
            return Redirect("https://www.google.com");
        }

        public IActionResult Content()
        {
            return Content("<h1>Welcome to Gym Management System</h1>", "text/html");
        }

        public IActionResult DownloadFile()
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Files", "plans.Json");

            var FileBytes = System.IO.File.ReadAllBytes(FilePath);

            return File(FileBytes,"Text/Json","DownloadFile.Json" );
        }

        public IActionResult EmptyAction()
        {
            return new EmptyResult();
        }

    }
}
