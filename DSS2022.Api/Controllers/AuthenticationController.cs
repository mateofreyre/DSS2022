﻿using Microsoft.AspNetCore.Mvc;
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
        private IProviderService _providerService;

        private const String ProviderUser = "luismiguel@gmail.com";
        private const String ProviderPwd = "123abc";
        
        public AuthenticationController(IBonitaBpmService bonitaBpmService, IProviderService providerService)
        {
            _bonitaBpmService = bonitaBpmService;
            _providerService = providerService;
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
                String providerToken = await _providerService.Auth(ProviderUser, ProviderPwd);

                HttpContext.Response.Cookies.Append("session-id", sessionId, cookieOptions);
                HttpContext.Response.Cookies.Append("api-token", apiToken, cookieOptions);
                HttpContext.Response.Cookies.Append("provider-token", providerToken, cookieOptions);
                JObject res = new JObject(
                    new JProperty("name", userDTO.Email),
                    new JProperty("sessionId", sessionId),
                    new JProperty("apiToken", apiToken),
                    new JProperty("groupId", groupId),
                    new JProperty("providerToken", providerToken)
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