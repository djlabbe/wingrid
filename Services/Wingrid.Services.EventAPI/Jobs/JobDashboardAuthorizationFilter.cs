using System.Diagnostics.CodeAnalysis;
using System.Net;
using Hangfire.Dashboard;
using Microsoft.Extensions.Primitives;

namespace Wingrid.Services.EventAPI.Jobs
{
    public class JobDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _claimType;
        private readonly StringValues _permittedValues;

        public JobDashboardAuthorizationFilter(string claimType, StringValues permittedValues)
        {
            _claimType = claimType ?? throw new ArgumentNullException(nameof(claimType));
            _permittedValues = permittedValues;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext != null && IsLocal(httpContext.Connection))
                return true;

            if (StringValues.IsNullOrEmpty(_permittedValues))
                return true;

            if (httpContext?.User == null)
                return false;

            return _permittedValues.Any(v => httpContext.User.HasClaim(c =>
                string.Equals(c.Type, _claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value != null &&
                string.Equals(c.Value, v, StringComparison.OrdinalIgnoreCase)
            ));
        }

        private bool IsLocal(ConnectionInfo connection)
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