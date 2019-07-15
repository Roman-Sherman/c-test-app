using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using aspdotnet_managesys.Models;
using aspdotnet_managesys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnet_managesys.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        //<summary>ログイン状態を確認します</summary>
        [HttpGet("loginStatus")]
        public IActionResult LoginStatus()
        {
            return this.Json(true);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromServices]SignInManager<Account> signInManager, [FromForm] string userName, [FromForm] string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return Unauthorized();
            }

            var result = await signInManager.PasswordSignInAsync(userName, passWord, false, false);
            
            if (result.Succeeded) {
                return Ok();
                  
            } else {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IdentityResult> LogOut([FromServices]SignInManager<Account> signInManager) {
            await signInManager.SignOutAsync();
            return IdentityResult.Success;
        }

        [HttpGet("loginAccount")]
        public Account loginAccount([FromServices]SignInManager<Account> signInManager, [FromServices]UserManager<Account> userManager) {
            return signInManager.IsSignedIn(this.User) ? userManager.GetUserAsync(this.User).Result : null;
        }
    }
}