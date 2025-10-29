using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService , SignInManager<ApplicationUser> signInManager )
        {
            _accountService=accountService;
            _signInManager=signInManager;
        }

        #region Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel account)
        {
            if(!ModelState.IsValid) return View(account);

            var user = _accountService.ValidateUser(account);

            if (user == null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
            }
            var Result = _signInManager.PasswordSignInAsync(user!, account.Password, account.RemeberMe, false).Result;

            if (Result.IsNotAllowed)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account is Not Allowed");
            }
            if (Result.IsLockedOut)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account is Locked Out");
            }
            if (Result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(account);

        }

        #endregion

        #region LogOut

        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult(); 
            return RedirectToAction(nameof(Login));
        }

        #endregion

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}
