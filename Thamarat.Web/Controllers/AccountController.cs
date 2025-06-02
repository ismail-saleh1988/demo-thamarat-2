//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace Thamarat.Web.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly SignInManager<IdentityUser> _signInManager;

//        public AccountController(SignInManager<IdentityUser> signInManager)
//        {
//            _signInManager = signInManager;
//        }

//        [Authorize]
//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            await _signInManager.SignOutAsync();
//            return RedirectToAction("Index", "Home");
//        }
//    }
//}
