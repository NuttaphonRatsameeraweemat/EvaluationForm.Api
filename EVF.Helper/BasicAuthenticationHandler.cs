using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EVF.Helper
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly IConfigSetting _config;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfigSetting config)
            : base(options, logger, encoder, clock)
        {
            _config = config;
        }

        /// <summary>
        /// Handle Basic Authentication.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            ApiUser user = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                user = this.Authenticate(username, password);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (user == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            this.InitialRoles(identity, user.Roles);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        /// <summary>
        /// Handle AuthenticateResult Fail return message.
        /// </summary>
        /// <param name="properties">The AuthenticationProperties.</param>
        /// <returns></returns>
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var model = new ResultViewModel
            {
                IsError = true,
                Message = $"Invalid Username or Password"
            };
            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            Response.OnStarting(async () =>
            {
                Response.ContentType = "application/json";
                await Response.WriteAsync(json);
            });
            return base.HandleChallengeAsync(properties);
        }

        /// <summary>
        /// Handle forbidden return message.
        /// </summary>
        /// <param name="properties">The AuthenticationProperties.</param>
        /// <returns></returns>
        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            var model = new ResultViewModel
            {
                IsError = true,
                Message = $"You don't have permission to access."
            };
            string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            Response.OnStarting(async () =>
            {
                Response.ContentType = "application/json";
                await Response.WriteAsync(json);
            });
            return base.HandleForbiddenAsync(properties);
        }

        /// <summary>
        /// Authorization username and passowrd in config setting.
        /// </summary>
        /// <param name="username">The username from basic authen.</param>
        /// <param name="password">The password from basic authen.</param>
        /// <returns></returns>
        private ApiUser Authenticate(string username, string password)
        {
            ApiUser apiUser = null;

            var apiUsersParts = _config.BasicAuthUsers;
            string[] apiUsers = apiUsersParts.Split('|');
            int index = Array.FindIndex(apiUsers, u => u == username);
            if (index > -1)
            {
                string[] apiPasswords = _config.BasicAuthPasswords.Split('|');
                string[] apiRoles = _config.BasicAuthRoles.Split('|');

                apiUser = new ApiUser
                {
                    Username = apiUsers[index],
                    Password = apiPasswords[index],
                    Roles = apiRoles[index]
                };
            }

            if (apiUser != null && (apiUser.Username != username || apiUser.Password != Convert.ToBase64String(Encoding.UTF8.GetBytes(password))))
            {
                // Sets to be null if it is unauthorized.
                apiUser = null;
            }

            return apiUser;
        }

        /// <summary>
        /// Initial Roles to ClaimsIdentity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <param name="roles">The user roles.</param>
        private void InitialRoles(ClaimsIdentity identity, string roles)
        {
            var roleArray = roles.Split(',');
            foreach (var item in roleArray)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, item));
            }
        }

    }
}
