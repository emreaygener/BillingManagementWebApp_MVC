using BillingManagementWebApp.Auth.Model;
using BillingManagementWebApp.Models.ViewModels;
using BillingManagementWebApp.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using BillingManagementWebApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using BillingManagementWebApp.Services;

namespace BillingManagementWebApp.Controllers
{

    public class LoginController:Controller
    {
        private readonly UserRepository _userRepository;
        private readonly SmsLoggerService _smsLoggerService;

        public LoginController(UserRepository userRepository, SmsLoggerService smsLoggerService)
        {
            _userRepository = userRepository;
            _smsLoggerService = smsLoggerService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateToken([FromBody] LoginViewModel loginModel)
        {
            var user = await _userRepository.GetByCredentials(loginModel.Username, loginModel.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = await _userRepository.CreateToken(user);

            HttpContext.Session.SetString("token", token.AccessToken);

            return Ok();
        }

        [HttpGet("refreshToken")]
        public async Task<ActionResult<Token>> RefreshToken()
        {
            User? user = await GetUserFromSessionedAccessToken();

            if (user is not null && user.RefreshTokenExpireDate > DateTime.Now)
            {
                var newToken = await _userRepository.CreateToken(user);

                HttpContext.Session.SetString("token", newToken.AccessToken);

                return Ok();
            }
            HttpContext.Session.Clear();
            return Unauthorized();
        }

        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Clear();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<User?> GetUserFromSessionedAccessToken()
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var user = await _userRepository.GetByEmail(jwtHandler
                                              .ReadJwtToken(HttpContext.Session.GetString("token"))
                                                 .Claims
                                                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)
                                                        .Value);
            return user;
        }
        public IActionResult Reset()
        {
            return View();
        }
        [HttpPut]
        public async Task<IActionResult>ResetPassword([FromBody]LoginViewModel credential)
        { 
            var user = await _userRepository.GetByCredentials(credential.Username);
            if (user == null)
            {
                return BadRequest();
            }
            var newPassword =  Guid.NewGuid().ToString().Substring(0,8);
            user.Password = newPassword;
            await _userRepository.Update(user);
            _smsLoggerService.Write("Here is your new password. You can change it anytime you log in. Password: " + newPassword);
            return Ok();
        }
    }
}
