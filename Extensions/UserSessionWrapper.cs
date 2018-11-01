using Microsoft.AspNetCore.Http;

namespace The_Wall_With_DotNet_Core.Extensions
{
    public class UserSessionWrapper
    {
        public int GetSessionUser(HttpContext httpContext){
            return (int)httpContext.Session.GetInt32("User");
        }

        public void SetSessionUser(int user, HttpContext httpContext) {
                httpContext.Session.SetInt32("User", user);
        }

    }
}