using ContactManagementSystem.API.Validators;
using ContactManagementSystem.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RegisterValidator _registerValidator;
        private readonly LoginValidator _loginValidator;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            RegisterValidator registerValidator,
            LoginValidator loginValidator)
        {
            _userManager = userManager;
            _configuration = configuration;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            try
            {
                // VALIDATE REQUEST
                var validation = await _registerValidator.ValidateAsync(request);
                if (!validation.IsValid)
                    return BadRequest(new
                    {
                        Errors = validation.Errors.Select(x => x.ErrorMessage)
                    });

                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if (userExists != null)
                    return BadRequest("User already exists");

                var user = new IdentityUser
                {
                    UserName = request.Email,
                    Email = request.Email
                };

                // Identity requires password internally
                var result = await _userManager.CreateAsync(user, "Temp@123456");

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok("User registration completed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during registration.");
            }
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            try
            {
                // VALIDATE REQUEST
                var validation = await _loginValidator.ValidateAsync(request);
                if (!validation.IsValid)
                    return BadRequest(new
                    {
                        Errors = validation.Errors.Select(x => x.ErrorMessage)
                    });

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Unauthorized("Invalid login credentials.");

                var token = GenerateToken(user);

                return Ok(new
                {
                    Message = "Login successful.",
                    Token = token
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred during login.");
            }
        }

        // JWT TOKEN
        private string GenerateToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(jwtSettings["DurationInMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}