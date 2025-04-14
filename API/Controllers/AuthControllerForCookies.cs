using API.DAL.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace API.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthControllerForCookies : ControllerBase
    {


        //private readonly DBManager _dbContext;
        //public record SignInRequest(string Email, string Password);
        //public record Response(bool IsSuccess, string Message);
        //public record UserClaim(string Type, string Value);
        //public record UserDto(string Email, string Password);

        //private readonly IHttpContextAccessor _contextAccessor;
        //public AuthControllerForCookies(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        //{
        //    _dbContext = new DBManager(configuration);
        //    _contextAccessor = contextAccessor;
        //}




        ////Endpoint for registering new users. only to be usen in emergency if neww user needs to be created through Swagger. Then Authorize
        ////Needs to be out commneted temporary. Should only be used by developer on localhost, to avoid security breach
        //[Authorize]
        //[HttpPost("Register")]
        //public ActionResult<User> Register(UserDto request)
        //{
        //    try
        //    {
        //        User user = new();
        //        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        //        user.Email = request.Email;
        //        user.PasswordHash = passwordHash;
        //        user.PasswordSalt = passwordSalt;
        //        user.Role = "Admin";
        //        _dbContext.AddAdmin(user);
        //        return Ok(user);
        //    }
        //    finally
        //    {

        //    }
        //}




        ////Endpoint for logging in and setting the cookie in client browser
        ////The cookie is not set until this method is completely done
        //[HttpPost("Login")]
        //public async Task<IActionResult> SignInAsync([FromBody] SignInRequest signInRequest)
        //{
        //    try
        //    {
        //        User user = _dbContext.GetAdmin(signInRequest.Email);

        //        if (user.Email != signInRequest.Email)
        //        {
        //            // Shoud be changed to wrong email or password. Only put this specific for debugging
        //            return BadRequest(new Response(false, "Wrong email or password."));
        //        }

        //        if (!VerifyPasswordHash(signInRequest.Password, user.PasswordHash!, user.PasswordSalt!))
        //        {
        //            // Shoud be changed to wrong email or password. Only put this specific for debugging
        //            return BadRequest(new Response(false, "Wrong email or password."));
        //        }

        //        //Adding claims to the cookie
        //        List<Claim> claims = new()
        //    {
        //         new Claim(type: ClaimTypes.Email, value: signInRequest.Email),
        //         new Claim(type: ClaimTypes.Role, value: user.Role!)
        //    };

        //        ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //        // Set the cookie
        //        await HttpContext.SignInAsync(
        //             CookieAuthenticationDefaults.AuthenticationScheme,
        //             new ClaimsPrincipal(identity),
        //             new AuthenticationProperties
        //             {
        //                 IsPersistent = true
        //             });
        //    }
        //    catch (Exception)
        //    { }
        //    return Ok(new Response(true, "Signed in successfully"));
        //}







        //// Endpoint for getting userclaims
        //[Authorize]
        //[HttpGet("User")]
        //public async Task<IActionResult> GetUser()
        //{
        //    try
        //    {
        //        List<UserClaim>? userClaims = User.Claims.Select(x => new UserClaim(x.Type, x.Value)).ToList();
        //        //Debug.WriteLine("Userclaims: " + userClaims[0].Value); //DEBUG
        //        User user = _dbContext.AuthAdmin(userClaims[0].Value, userClaims[1].Value);
        //        if (user.Email != userClaims[0].Value || user.Role != userClaims[1].Value)
        //        {
        //            return BadRequest(new Response(false, "User is not authorized!"));
        //        }

        //        // userClaim[0].value should be email, and userClaim[1].value should be role
        //        if (userClaims != null && userClaims[0].Value == user.Email && userClaims[1].Value == user.Role)
        //        {

        //            //Adding claims to the cookie to 
        //            List<Claim> claims = new()
        //    {
        //         new Claim(type: ClaimTypes.Email, value: userClaims[0].Value),
        //         new Claim(type: ClaimTypes.Role, value: userClaims[1].Value)
        //    };
        //            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //            await HttpContext.SignInAsync(
        //                CookieAuthenticationDefaults.AuthenticationScheme,
        //                new ClaimsPrincipal(identity),
        //                new AuthenticationProperties
        //                {
        //                    IsPersistent = true,
        //                });
        //            return Ok(userClaims);
        //        }

        //    }
        //    catch (Exception)
        //    { }
        //    return Ok();
        //}





        //// Endpoint for logging out
        //[Authorize]
        //[HttpGet("Logout")]
        //public async Task SignOutAsync()
        //{
        //    try
        //    {
        //        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    }
        //    finally
        //    {

        //    }
        //}





        ////Method to create password hash and salt. (Used in the Register endpoint)
        //private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        //{
        //    try
        //    {
        //        using HMACSHA512 hmac = new();
        //        passwordSalt = hmac.Key;
        //        passwordHash = hmac.ComputeHash(Convert.FromBase64String(password));
        //    }
        //    finally
        //    {

        //    }
        //}




        ////Method to verify if the password is correct. (Used in the Login endpoint)
        //private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        //{
        //    try
        //    {
        //        using HMACSHA512 hmac = new(passwordSalt);
        //        byte[] computedHash = hmac.ComputeHash(Convert.FromBase64String(password));
        //        return computedHash.SequenceEqual(passwordHash);
        //    }
        //    finally
        //    {

        //    }
        //}

    }
}
