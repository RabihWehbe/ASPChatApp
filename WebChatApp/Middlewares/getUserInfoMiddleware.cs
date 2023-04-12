using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WebChatApp.Model;
using WebChatApp.Services;

namespace WebChatApp.Middlewares
{
    //The RequestDelegate represents the next middleware in the pipeline
    public class GetUserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public GetUserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /*
         Invoke or InvokeAsync that takes an HttpContext as the first parameter and returns a Task.
        This method should include the code for processing the request,
        optionally short-circuiting the pipeline and returning a response,
         or passing control on to the next middleware.
         */
        public Task Invoke(HttpContext httpContext)
        {

           // Console.WriteLine("get user info middleware is running...");

            var ctxtUser = httpContext.User;

            if (ctxtUser.Identity.IsAuthenticated)
            {
             //   Console.WriteLine("authenticated user");
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ContactsMiddlewareExtensions
    {
        public static IApplicationBuilder UseContactsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GetUserInfoMiddleware>();
        }
    }
}
