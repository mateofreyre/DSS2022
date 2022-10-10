using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web;
using DSS2022.Business.Helpers;

namespace DSS2022.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
            private CookieCollection collection;
            string strCookietoPass;
            string sessionID;

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            AuthenticationHelper authenticationHelper = new AuthenticationHelper();
            var loginStr = authenticationHelper.Login();
            return Ok(loginStr);
        }

        [HttpPost("logout")]
        public async void Logout()
            {
                const string url = "http://localhost:8080/bonita/";

                var cookies = new CookieContainer();
                var handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                using (var client = new HttpClient(handler))
                {
                    var uri = new Uri(url);
                    client.BaseAddress = uri;

                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("redirect", "false")
                });

                    HttpResponseMessage response = await client.PostAsync("logoutservice", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyText = await response.Content.ReadAsStringAsync();

                        if (!String.IsNullOrEmpty(responseBodyText))
                        {
                            Console.WriteLine("Unsuccessful Logout.Bonita bundle may not have been started, or the URL is invalid.");
                            return;
                        }

                        Console.WriteLine("Successfully Logged out.");
                    }
                    else
                    {
                        Console.WriteLine("Logout Error" + (int)response.StatusCode + "," + response.ReasonPhrase);
                    }

                }
            }

       
    }
}
