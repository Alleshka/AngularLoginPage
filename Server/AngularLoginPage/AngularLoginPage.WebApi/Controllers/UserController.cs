using AngularLoginPage.Common;
using AngularLoginPage.Context;
using AngularLoginPage.Domain;
using AngularLoginPage.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularLoginPage.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AngularLoginPageDbContext _context;

        public UserController(AngularLoginPageDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister user)
        {
            if (user == null)
            {
                return BadRequest("Incorrect data");
            }

            if (!ModelState.IsValid)
            {
                return Conflict(ModelState);
            }

            var foundUser = await _context.Users.FirstOrDefaultAsync(x=>x.Login == user.Login);
            if (foundUser != null)
            {
                return Conflict($"User with login {foundUser.Login} was already registered");
            }

            Province province = await _context.Provinces.FindAsync(user.ProvinceId);
            if (province == null)
            {
                return NotFound($"Province with id = {user.ProvinceId} not found");
            }

            PasswordParts passwordParts = SecurePasswordHasher.ComputeHash(user.Password);

            User newUser = new User()
            {
                Id = Guid.NewGuid(),
                Login = user.Login,
                PasswordHash = passwordParts.Password,
                PasswordSalt = passwordParts.Salt,
                Province = province
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(newUser);
        }
    }
}
