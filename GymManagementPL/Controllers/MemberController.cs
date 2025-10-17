using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{ 
    public class MemberController : Controller
    {
        public IActionResult Index(int id)
        {
            return View();
        }

        public IActionResult GetMembers()
        {
             return View();
        }

        public IActionResult CreateMember()
        {
            return View();
        }

    }
}
