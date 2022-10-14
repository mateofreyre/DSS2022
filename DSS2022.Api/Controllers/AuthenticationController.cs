using Microsoft.AspNetCore.Mvc;
using System.Net;
using DSS2022.Business.Helpers;
using DSS2022.DataTransferObjects.User;
using Newtonsoft.Json.Linq;

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
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            AuthenticationHelper authenticationHelper = new AuthenticationHelper();
            var loginStr = await authenticationHelper.Login(userDTO);
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Domain = "localhost:3000";
            if(loginStr.HasValues) {
                HttpContext.Response.Cookies.Append("session-id", loginStr.GetValue("sessionId").ToString(), cookieOptions);
                HttpContext.Response.Cookies.Append("api-token", loginStr.GetValue("apiToken").ToString(), cookieOptions);
                JObject res = new JObject( new JProperty("credentials", loginStr));
                return Ok(loginStr);
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public void Logout()
            {
                var bonitaSessionId = this.HttpContext.Request.Cookies["session-id"];
                var bonitaApiKey = this.HttpContext.Request.Cookies["api-token"];
                
                AuthenticationHelper authenticationHelper = new AuthenticationHelper();
                authenticationHelper.Logout(bonitaApiKey,bonitaSessionId);
            }

       
    }
}
