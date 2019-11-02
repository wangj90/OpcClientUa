using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace OpcClientUa.Middleware
{
    public static class OpcApplicationBuilderExtensions
    {
        public static void UseOpcNotification(this IApplicationBuilder app, OpcPointOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            app.UseMiddleware<OpcMiddleware>(Options.Create(options));
        }
    }
}
