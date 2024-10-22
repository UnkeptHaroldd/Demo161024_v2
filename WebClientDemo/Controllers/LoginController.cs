using APIDemo161024.DTOS;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebClientDemo.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;

        public LoginController()
        {
            client = new HttpClient();
            var contentType =
                new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public IActionResult Index()
        {
            string token = HttpContext.Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Books"); 
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var loginJson = JsonSerializer.Serialize(model, options);
            var content = new StringContent(loginJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(APIEndPoint.SignIn, content);
            if (!response.IsSuccessStatusCode)
            {
                string errorData = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("Email", "Email or Password are Incorrect" );
                return View("Index", model);
            }
            string strData = await response.Content.ReadAsStringAsync();
            string Token = JsonSerializer.Deserialize<string>(strData, options);
            if (!string.IsNullOrEmpty(Token))
            {
                // Create a cookie with the token
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Ensures the cookie is accessible only by the server
                    Secure = true,   // Ensures the cookie is sent over HTTPS only
                    Expires = DateTimeOffset.UtcNow.AddDays(1) // Set the expiration as needed
                };

                // Add the cookie to the response
                Response.Cookies.Append("AuthToken", Token, cookieOptions);
                return RedirectToAction("Index", "Home");
            } else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
