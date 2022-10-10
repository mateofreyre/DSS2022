using System.Net;
using DSS2022.DataTransferObjects.User;

namespace DSS2022.Business.Helpers
{
    public class AuthenticationHelper
    {
        private CookieCollection collection;
        string strCookietoPass;
        string sessionID;

        public async Task<string> Login(UserDTO userDto)
        {

            const string bonitaUrl = "http://localhost:38169/bonita/";
            //const string bonitaUrl = "http://localhost:8080/bonita/";

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
                        return "";
                    }

                    collection = cookies.GetCookies(uri);
                    strCookietoPass = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

                    sessionID = collection["X-Bonita-API-Token"].ToString();

                    Console.WriteLine(string.Format("Successful Login Retrieved session ID {0}", sessionID));
                    return sessionID.Replace("X-Bonita-API-Token=","");
                    // Do useful work 
                }
                else
                {
                    Console.WriteLine("Login Error" + (int)response.StatusCode + "," + response.ReasonPhrase);
                    return "";
                }
            }
        }
    }
}
