using System.Net;
using DSS2022.Api.Exceptions;
using DSS2022.DataTransferObjects.User;
using Newtonsoft.Json.Linq;


namespace DSS2022.Business.Helpers
{
    public class AuthenticationHelper
    {
        private CookieCollection collection;
        string strCookietoPass;
        string sessionID;
        private readonly Uri _uri = new Uri(bonitaUrl);
        
        const string bonitaUrl = "http://localhost:38169/bonita/";
        //const string bonitaUrl = "http://localhost:8080/bonita/";

        public async Task<JObject> Login(UserDTO userDto)
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = _uri;

                try {
                    HttpResponseMessage response = await client.PostAsync("loginservice", BuildFormUrlEncodedParams(userDto.Email, userDto.Password));
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Login Error" + (int)response.StatusCode + "," + response.ReasonPhrase);
                        return new JObject();
                    }


                    var responseBodyAsText = await response.Content.ReadAsStringAsync();

                    if (!String.IsNullOrEmpty(responseBodyAsText))
                    {
                        Console.WriteLine("Unsuccessful Login.Bonita bundle may not have been started, or the URL is invalid.");
                        return new JObject();
                    }

                    return GetCredentialsFromCookie(cookies, response);


                } catch (HttpRequestException e)
                {
                    throw new BonitaConnectionException("Error connecting to BonitaBPM");
                }
            }
        }

        private JObject GetCredentialsFromCookie(CookieContainer cookies, HttpResponseMessage response)
        {
            collection = cookies.GetCookies(_uri);
            strCookietoPass = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

            var apitoken = collection["X-Bonita-API-Token"].ToString();
            sessionID = collection["JSESSIONID"].ToString();

            Console.WriteLine("Successful Login Retrieved session ID {0}", sessionID);

            return new JObject(
                new JProperty("apiToken", apitoken.Replace("X-Bonita-API-Token=", "")),
                new JProperty("sessionId", sessionID.Replace("JSESSIONID=", ""))
            );

        }

        private FormUrlEncodedContent BuildFormUrlEncodedParams(String user, string pwd)
        {
            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", user),
                new KeyValuePair<string, string>("password", pwd),
                new KeyValuePair<string, string>("redirect", "false"),
                new KeyValuePair<string, string>("redirectUrl", ""),
            });
        }

        public async void Logout(string token, string sessionId)
        {
            var cookies = new CookieContainer();

            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            handler.CookieContainer = cookies;
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = _uri;
                
                cookieContainer.Add(_uri, new Cookie("X-Bonita-API-Token", token));
                cookieContainer.Add(_uri, new Cookie("JSESSIONID", sessionId));
                
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
