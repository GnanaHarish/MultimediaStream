using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultimediaStream.Models;
using MultimediaStream.ViewModels;
using MultimediaStream.Constants;
using Microsoft.AspNetCore.Identity;

namespace MultimediaStream.Controllers
{
    public class RegisterPage : BaseController {
        [HttpGet("Register")]
        public IActionResult Register() {
            return View();
        }
    }

    public class LoginPage : BaseController {
        [HttpGet("Login")]
        public IActionResult Login() {
            return View();
        }
    }

    public class Home : BaseController {
        private readonly AppDbContext _dbcontext;
        public Home(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;

        }
        [HttpGet("HomePage")]
        public IActionResult HomePage([FromQuery] string Email) {
            var userName = _dbcontext.Users
            .Where(m => m.Email == Email)
            .Select(m => m.UserName)
            .SingleOrDefault();
            ViewData["name"] = userName;
            return View();
        }
    }

    public class AuthenticationController : BaseController
    {
        private readonly AppDbContext _dbcontext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthenticationController(AppDbContext dbcontext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) {
            _dbcontext = dbcontext;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model) {
            if (model.Email != null) {
                var emailExists = _dbcontext.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                if (emailExists != null) {
                    return BadRequest(new Response { Status = Constants.Constants.ConstError, Message = Constants.Constants.ContEmailExists });
                }

                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                };

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    
                    return Ok(new ResponseForRegister { Status = Constants.Constants.ConstSucess, Message = Constants.Constants.ConstRegistrationSucess });

                }
                else {
                    return BadRequest(new Response { Status = Constants.Constants.ConstFail, Message = Constants.Constants.ConstRegistrationFailed });
                }
            }

            return BadRequest(new Response { Status = Constants.Constants.ConstRegistrationFailed, Message = Constants.Constants.ConstInvalidInput });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (!string.IsNullOrEmpty(model.Email)) {
                try
                {

                    var users = _dbcontext.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                    if (users != null)
                    {
                        if (users != null && await _userManager.CheckPasswordAsync(users, model.Password))
                        {
                            IdentityUser user = new IdentityUser
                            {
                                Email = model.Email,  
                            };

                            await _signInManager.SignInAsync(user, isPersistent: false);
                            //return View("~/Views/Shared/HomePage.cshtml");
                            return Ok(new Response { Status = Constants.Constants.ConstSucess, Message = Constants.Constants.ConstLoginSucess });

                        }
                        return Ok(new Response { Status = Constants.Constants.ConstFail, Message = Constants.Constants.ConstPasswordInvalid });
                    }

                    return Ok(new Response { Status = Constants.Constants.ConstError, Message = Constants.Constants.ConstEmailInvalid });
                }
                catch (Exception ex)
                {
                    return Ok(new Response { Status = Constants.Constants.ConstError, Message = ex.ToString() });
                }
            }

            return Ok(new Response { Status = Constants.Constants.ConstError });
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Login() {
            await _signInManager.SignOutAsync();
            return Ok(new Response { Status = Constants.Constants.ConstSucess, Message = "Logout Sucess" });
        }
    }
}
