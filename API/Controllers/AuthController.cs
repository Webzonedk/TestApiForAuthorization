using API.DAL;
using API.DAL.Entities;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        // Records for requests and responses
        public record SignInRequest(string Email, string Password);
        public record Response(bool IsSuccess, string Message);
        public record UserDto(string Email, string Password, string Role);

        /// <summary>
        /// Constructor that injects AppDbContext and IConfiguration.
        /// </summary>
        /// <param name="dbContext">Injected EF Core DB context.</param>
        /// <param name="configuration">Injected configuration to read token secret.</param>
        public AuthController(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Endpoint for registering a new user.
        /// Uses EF Core to persist the user with hashed password data.
        /// </summary>
        /// <param name="request">UserDto containing email and password.</param>
        /// <returns>A Response indicating registration success.</returns>
        //[Authorize] // Fjern eller juster autorisationen efter behov
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(UserDto request)
        {
            // Create a new user entity and hash the provided password.
            User user = new();
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var role = await _dbContext.UserRoles.FirstOrDefaultAsync(r => r.RoleName == request.Role);
            if (role == null)
                return BadRequest($"{request.Role} role does not exist.");

            user.Role = role;


            // Save the new user using EF Core
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new Response(true, "User registered successfully."));
        }

        /// <summary>
        /// Endpoint for logging in.
        /// Verifies the user via EF Core and returns a JWT token upon successful authentication.
        /// </summary>
        /// <param name="signInRequest">SignInRequest containing email and password.</param>
        /// <returns>A JSON object containing the JWT token.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] SignInRequest signInRequest)
        {
            // Retrieve the user from the database based on email.
            var user = await _dbContext.Users
                            .Include(u => u.Role)
                            .FirstOrDefaultAsync(u => u.Email == signInRequest.Email);

            if (user == null || !VerifyPasswordHash(signInRequest.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                return BadRequest(new Response(false, "Wrong email or password."));
            }

            // Create a JWT token for the authenticated user.
            string token = CreateToken(user);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Endpoint for retrieving information about the authenticated user.
        /// Token-baseret autorisation udtrækker user claims fra tokenet.
        /// </summary>
        /// <returns>User information such as email and role.</returns>
        [Authorize]
        [HttpGet("User")]
        public IActionResult GetUser()
        {
            // Retrieve email and role from the token claims.
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (email is null || role is null)
            {
                return Unauthorized(new Response(false, "Invalid token claims."));
            }

            return Ok(new { Email = email, Role = role });
        }

        // Private method to create a password hash and salt.
        // NOTE: Typically you'd use Encoding.UTF8.GetBytes(password) instead of Convert.FromBase64String
        // unless the password is already base64 encoded.
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); 
        }


        // Private method to verify if the provided password matches the stored hash.
        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new(passwordSalt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }


        // Private method to create a JWT token based on the authenticated user's data.
        private string CreateToken(User user)
        {
            string roleName = user.Role?.RoleName ?? "Unknown";

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName)
            };

            // Retrieve token secret from configuration
            var tokenSecret = _configuration["AppSettings:Token"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            // Return the serialized JWT token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


       

    }
}
