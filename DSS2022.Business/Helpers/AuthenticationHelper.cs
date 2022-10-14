using System.Net;
using DSS2022.DataTransferObjects.User;
using Newtonsoft.Json.Linq;

namespace DSS2022.Business.Helpers
{
    public class AuthenticationHelper
    {
        private CookieCollection collection;
        string strCookietoPass;
        string sessionID;
        
        const string bonitaUrl = "http://localhost:38169/bonita/";
        //const string bonitaUrl = "http://localhost:8080/bonita/";

        public async Task<JObject> Login(UserDTO userDto)
        {

            var cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(bonitaUrl);
                client.BaseAddress = uri;
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", userDto.Email),
                    new KeyValuePair<string, string>("password", userDto.Password),
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
                        return  new JObject();
                    }

                    collection = cookies.GetCookies(uri);
                    strCookietoPass = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

                    var apitoken = collection["X-Bonita-API-Token"].ToString();
                    sessionID = collection["JSESSIONID"].ToString();

                    Console.WriteLine(string.Format("Successful Login Retrieved session ID {0}", sessionID));
                    
                    return new JObject(new JProperty("apiToken", apitoken.Replace("X-Bonita-API-Token=","")), new JProperty("sessionId", sessionID.Replace("JSESSIONID=","")));
                    
                   // return sessionID.Replace("X-Bonita-API-Token=","");
                    // Do useful work 
                }
                else
                {
                    Console.WriteLine("Login Error" + (int)response.StatusCode + "," + response.ReasonPhrase);
                    return new JObject();
                }
            }
        }
        
         public async void Logout(string token, string sessionId)
        {
            var cookies = new CookieContainer();

            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            handler.CookieContainer = cookies;
            using (var client = new HttpClient(handler))
            {
                var uri = new Uri(bonitaUrl);
                client.BaseAddress = uri;
                
                cookieContainer.Add(uri, new Cookie("X-Bonita-API-Token", token));
                cookieContainer.Add(uri, new Cookie("JSESSIONID", sessionId));
                
                client.DefaultRequestHeaders.Add("X-Bonita-API-Token", token);

                HttpResponseMessage response = await client.GetAsync("logoutservice");

                if (response.IsSuccessStatusCode)
                {
                    var responseBodyAsText = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("Login Error" + (int)response.StatusCode + "," + response.ReasonPhrase);
                }
            }
        }
    }
}
