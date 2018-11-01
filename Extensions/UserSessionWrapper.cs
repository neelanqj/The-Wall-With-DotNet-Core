using Microsoft.AspNetCore.Http;

namespace The_Wall_With_DotNet_Core.Extensions
{
    public class UserSessionWrapper
    {
        private HttpContext _httpContext;
        public UserSessionWrapper(HttpContext httpContext){
            _httpContext = httpContext;
        }
        public int GetSessionUser(){
            return (int)_httpContext.Session.GetInt32("User");
        }

        public void SetSessionUser(int user) {
                _httpContext.Session.SetInt32("User", user);
        }

    }
}