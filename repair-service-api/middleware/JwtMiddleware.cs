using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using repair.service.shared.model;
using repair.service.shared.constants;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace repair.service.api.middleware
{
    /// <summary>
    /// Middleware to extract the Jwt Token Information
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            var token = context.Request.Headers[AppConstants.AuthorizationJwt].FirstOrDefault()?.Split(" ").Last();
            var xcredentialJwt = context.Request.Headers[AppConstants.XCredentialJwt].FirstOrDefault();

            if (token != null)
                AttachJwtInfoToContext(context, xcredentialJwt, token);

            await _next(context);
        }

        /// <summary>
        /// Method to parse the token and x-credential from the request header
        /// </summary>
        /// <param name="context">Current http Context</param>
        /// <param name="token">bearer token</param>
        /// <param name="xcredentialJwt">jwt token in the request header</param>
        private void AttachJwtInfoToContext(HttpContext context, string xcredentialJwt, string token)
        {
            //x-credential-jwt is provided by the api gateway after validating the TID
            //account information will not be available for the admin account, that is why x-credential-jwt token used
            //Authorization will have account information when using key id and key secret.  Authorization token from TID does not
            //account information.  The Authorization token is sent by ISA workflow, x-credential-jwt is when using public
            // api calls from the User interface
            var jwtToken = new JwtSecurityToken(xcredentialJwt ?? token);

            JwtInfo jwtInfo = new()
            {
                AccountId = jwtToken.Claims.First(c => c.Type == AppConstants.AccountId).Value,
                AccountName = jwtToken.Claims.First(c => c.Type == AppConstants.AccountName).Value,
                Token = token
            };

            context.Items[AppConstants.JwtInfo] = jwtInfo;
        }
    }
}
