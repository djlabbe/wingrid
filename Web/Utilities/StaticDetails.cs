namespace Wingrid.Web.Utility
{
    public class StaticDetails
    {
        public static string EventAPIBase { get; set; } = "";
        public static string FixturesAPIBase { get; set; } = "";
        public static string AuthAPIBase { get; set; } = "";

        public const string RoleAdmin = "ADMIN";
        public const string RoleUser = "USER";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }

}