using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WT_WebMVCApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WT_WebMVCApp.Helpers;

namespace WT_WebMVCApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        public async Task Logout()
        {
            // get the metadata
            var discoveryClient = new DiscoveryClient(WorkotTrackerHelper.IdentityServerUrl);
            var metaDataResponse = await discoveryClient.GetAsync();

            // create a TokenRevocationClient
            var revocationClient = new TokenRevocationClient(metaDataResponse.RevocationEndpoint, "wtmvcapp", "secret");

            // get the access token to revoke 
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                var revokeAccessTokenResponse = await revocationClient.RevokeAccessTokenAsync(accessToken);

                if (revokeAccessTokenResponse.IsError)
                {
                    throw new Exception("Problem encountered while revoking the access token.", revokeAccessTokenResponse.Exception);
                }
            }

            // revoke the refresh token as well
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var revokeRefreshTokenResponse = await revocationClient.RevokeRefreshTokenAsync(refreshToken);

                if (revokeRefreshTokenResponse.IsError)
                {
                    throw new Exception("Problem encountered while revoking the refresh token.", revokeRefreshTokenResponse.Exception);
                }
            }

            //Log out of client app
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public async Task<string> GetAddress()
        {
            //HOW TO CALL USER INFO ENDPOINT
            var discoveryClient = new DiscoveryClient(WorkotTrackerHelper.IdentityServerUrl);
            var metaDataResponse = await discoveryClient.GetAsync();

            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new Exception("Problem accessing the UserInfo endpoint.", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(c => c.Type == "address")?.Value;

            return address;
        }

        [Authorize(Roles = "Admin")]
        public string Admin()
        {
            //EXAPLE FOR ROLE AUTHORIZATION
            return "HELLO ADMIN !!";
        }

        public IActionResult Error()
        {
            _logger.LogInformation("Error in APP");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            _logger.LogInformation("Access Denied in APP");
            return View();
        }
    }
}
