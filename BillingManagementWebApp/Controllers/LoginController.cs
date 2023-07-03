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

namespace BillingManagementWebApp.Controllers
{

    public class LoginController:Controller
    {
        private readonly UserRepository _userRepository;

        public LoginController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            var auth = HttpContext.Request.Headers["Authorization"];
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
            ViewBag.Token = token.AccessToken;
            ViewBag.RefreshToken = token.RefreshToken;

            Response.Headers.Add("Authorization", token.AccessToken);
            HttpContext.Session.SetString("token", token.AccessToken);

            return Ok(new { token = token.AccessToken, refreshToken = token.RefreshToken });
        }

        [HttpGet("refreshToken")]
        public async Task<ActionResult<Token>> RefreshToken([FromHeader] string refreshToken)
        {
            var user = await _userRepository.GetByValidRefreshToken(refreshToken);
            if (user != null)
            {
                var newToken = await _userRepository.CreateToken(user);
                ViewBag.Token = newToken.AccessToken;
                ViewBag.RefreshToken = newToken.RefreshToken;

                Response.Headers.Add("Authorization", newToken.AccessToken);

                return Ok(new { token = newToken.AccessToken, refreshToken = newToken.RefreshToken });
            }
            return Unauthorized();
        }
    }
}
