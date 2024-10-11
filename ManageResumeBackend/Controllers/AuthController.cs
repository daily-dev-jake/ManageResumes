using ManageResumeBackend.API.Models;
using ManageResumeBackend.API.Models.Authentication.SignUp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManageResumeBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            // Check User Exist
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null) return StatusCode(StatusCodes.Status403Forbidden,
                new Response() { Status = "Error", Message = "User already Exists!" });

            // if != in db, add user in.

            IdentityUser user = new()
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(user, registerUser.Password);
            return result.Succeeded 
                ? StatusCode(StatusCodes.Status201Created,
                    new Response() { Status = "Success", Message = " User Created Successfully" })
                : StatusCode(StatusCodes.Status500InternalServerError,
                    new Response() { Status = "Error", Message = " User Failed to Create" });

            // then assign a role to user

        }
    }
}
