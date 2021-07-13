using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolution.Utilityes.Constants;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public LoginController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request)

        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Tài Khoản hoặc mật khẩu không đúng!");
                    return View();
            }    
                
            var restul = await _userApiClient.Authenticate(request);
            if (!restul.IsSuccessed)
            {
                ModelState.AddModelError("", restul.Message);
                return View();
                //return BadRequest(restul.Message);
            }

            var userPrincipal = this.ValidateToken(restul.ResultObj);
            var authPropertise = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(360),
                IsPersistent = false
            };
            HttpContext.Session.SetString(SystemConstans.Appsetings.DefaultLanguageId, _configuration[SystemConstans.Appsetings.DefaultLanguageId]);
            HttpContext.Session.SetString(SystemConstans.Appsetings.Token, restul.ResultObj);
            await HttpContext.SignInAsync(
                              CookieAuthenticationDefaults.AuthenticationScheme,
                              userPrincipal,
                              authPropertise);

            return RedirectToAction("Index", "Home");

        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
    }
}
