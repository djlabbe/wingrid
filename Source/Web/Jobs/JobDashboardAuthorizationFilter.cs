using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Hangfire.Dashboard;
using Microsoft.Extensions.Primitives;

namespace Wingrid.Jobs
{
    public class JobDashboardAuthorizationFilter(string claimType, StringValues permittedValues) : IDashboardAuthorizationFilter
    {
        private readonly string _claimType = claimType ?? throw new ArgumentNullException(nameof(claimType));
        private readonly StringValues _permittedValues = permittedValues;

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext != null && IsLocal(httpContext.Connection))
                return true;

            if (httpContext == null) return false;

            var jwtToken = "";
            if (httpContext.Request.Query.ContainsKey("jwt_token"))
            {
                jwtToken = httpContext.Request.Query["jwt_token"].FirstOrDefault();
                if (jwtToken != null) SetCookie(httpContext, jwtToken);
            }
            else
            {
                jwtToken = httpContext.Request.Cookies["_hangfireCookie"];
            }

            if (string.IsNullOrEmpty(jwtToken))
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);

            var isAuthorized = jwtSecurityToken.Claims.Any(t => t.Type.Equals(_claimType) && _permittedValues.Any(pv => t.Value.Equals(pv)));

            return isAuthorized;
        }

        private static void SetCookie(HttpContext? httpContext, string jwtToken)
        {
            httpContext?.Response.Cookies.Append("_hangfireCookie",
                    jwtToken,
                    new CookieOptions()
                    {
                        Expires = DateTime.Now.AddMinutes(30)
                    });
        }

        private static bool IsLocal(ConnectionInfo connection)
        {
            if (connection == null) return true;
            if (HasValue(connection.RemoteIpAddress))
            {
                return HasValue(connection.LocalIpAddress) ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress) : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }
            return !HasValue(connection.LocalIpAddress) || IPAddress.IsLoopback(connection.LocalIpAddress);
        }

        public static bool HasValue([NotNullWhen(true)] IPAddress? address)
        {
            return address != null && address.ToString() != "::1";
        }
    }
}