using Microsoft.AspNetCore.Mvc;
using System.Net;
using DSS2022.Business;
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
        private IBonitaBpmService _bonitaBpmService;

        string strCookietoPass;
        string sessionID;

        public AuthenticationController(IBonitaBpmService bonitaBpmService)
        {
            _bonitaBpmService = bonitaBpmService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDTO)
        {
            AuthenticationHelper authenticationHelper = new AuthenticationHelper();
            var loginStr = await authenticationHelper.Login(userDTO);
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Domain = "localhost:3000";
            if (loginStr.HasValues)
            {
                string sessionId = loginStr.GetValue("sessionId").ToString();
                string apiToken = loginStr.GetValue("apiToken").ToString();

                string userId = await _bonitaBpmService.GetUserId(apiToken, sessionId, userDTO.Email);
                string groupId = await _bonitaBpmService.GetGroupId(apiToken, sessionId, userId);

                HttpContext.Response.Cookies.Append("session-id", sessionId, cookieOptions);
                HttpContext.Response.Cookies.Append("api-token", apiToken, cookieOptions);
                JObject res = new JObject(
                    new JProperty("sessionId", sessionId),
                    new JProperty("apiToken", apiToken),
                    new JProperty("groupId", groupId)
                );
                return Ok(res);
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public void Logout()
        {
            var bonitaSessionId = this.HttpContext.Request.Cookies["session-id"];
            var bonitaApiKey = this.HttpContext.Request.Cookies["api-token"];

            AuthenticationHelper authenticationHelper = new AuthenticationHelper();
            authenticationHelper.Logout(bonitaApiKey, bonitaSessionId);
        }
    }
}