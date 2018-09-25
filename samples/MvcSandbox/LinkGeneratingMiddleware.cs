using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace MvcSandbox
{
    public class LinkGeneratingMiddleware
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly RequestDelegate _next;

        public LinkGeneratingMiddleware(LinkGenerator linkGenerator, RequestDelegate next)
        {
            if (linkGenerator == null)
            {
                throw new ArgumentNullException(nameof(linkGenerator));
            }

            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _linkGenerator = linkGenerator;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path == "/link")
            {
                var path = _linkGenerator.GetPathByRouteValues(
                    routeName: null,
                    values: new { controller = "SubscriptionManagement", action = "GetAll" });

                //var path = _linkGenerator.GetUriByAction(httpContext, "GetAll", "SubscriptionManagement");

                if (bool.TryParse(httpContext.Request.Query["redirect"], out var redirect) && redirect)
                {
                    httpContext.Response.Redirect(path);
                }
                else
                {
                    httpContext.Response.StatusCode = 200;
                    await httpContext.Response.WriteAsync(path);
                }
                return;
            }

            await _next(httpContext);
        }
    }
}
