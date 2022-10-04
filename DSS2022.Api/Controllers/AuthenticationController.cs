using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web;

namespace DSS2022.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
            private CookieCollection collection;
            string strCookietoPass;
            string sessionID;

        /*static void Main(string[] args)
        {
            BonitaApi obj = new BonitaApi();
            Task login = new Task(obj.Login);
            login.Start();
            login.Wait();
            Console.ReadLine();

            Task GetProcesses = new Task(obj.GetProcesses);
            GetProcesses.Start();
            GetProcesses.Wait();
            Console.ReadLine();

            Task logout = new Task(obj.Logout);
            logout.Start();
            logout.Wait();
            Console.ReadLine();

        }*/

        [HttpPost("login")]
        public async Task<IActionResult> Login()
            {
                const string url = "http://localhost:8080/bonita/";

                var cookies = new CookieContainer();
                var handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                using (var client = new HttpClient(handler))
                {
                    var uri = new Uri(url);
                    client.BaseAddress = uri;
                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("username", "helen.kelly"),
                    new KeyValuePair<string, string>("password", "bpm"),
                    new KeyValuePair<string, string>("redirect", "false"),
                    new KeyValuePair<string, string>("redirectUrl", ""),
                });

                    HttpResponseMessage response = await client.PostAsync("loginservice", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyAsText = await response.Content.ReadAsStringAsync();

                        if (!String.IsNullOrEmpty(responseBodyAsText))
                        {
                            Console.WriteLine("Unsuccessful Login.Bonita bundle may not have been started, or the URL is invalid.");
                            return Ok();
                        }

                        collection = cookies.GetCookies(uri);
                        strCookietoPass = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

                        sessionID = collection["JSESSIONID"].ToString();

                        Console.WriteLine(string.Format("Successful Login Retrieved session ID {0}", sessionID));
                    return Ok(sessionID);
                        // Do useful work 
                    }
                    else
                    {
                        Console.WriteLine("Login Error" + (int)response.StatusCode + "," + response.ReasonPhrase);
                    }
                return Ok();


            }
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

        [HttpPost("getProcesses")]
        public async void GetProcesses()
            {

                var handler = new HttpClientHandler();

                Cookie ok = new Cookie("Set-Cookie:", strCookietoPass);

                handler.CookieContainer.Add(collection);

                using (var client = new HttpClient(handler))
                {

                    var builder = new UriBuilder("http://localhost/bonita/API/bpm/process");
                    builder.Port = 8080;

                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["p"] = "0";
                    query["c"] = "10";
                    builder.Query = query.ToString();

                    Uri uri = new Uri(builder.ToString());
                    client.BaseAddress = uri;

                    HttpResponseMessage response = await client.GetAsync(uri.ToString());

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBodyText = await response.Content.ReadAsStringAsync();

                        if (String.IsNullOrEmpty(responseBodyText))
                        {
                            Console.WriteLine("Unsuccessful GetProcesses.Bonita bundle may not have been started, or the URL is invalid.");
                            return;
                        }

                        Console.WriteLine("Successfully GetProcesses:" + responseBodyText);

                    }
                    else
                    {
                        Console.WriteLine("GetProcesses Error" + (int)response.StatusCode + "," + response.ReasonPhrase);
                    }

                }
        
        }
    }
}
