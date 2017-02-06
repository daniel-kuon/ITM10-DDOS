using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestServer
{
    public class Startup
    {
        public static int ConcurrentRequests { get; set; }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //            app.Run(async (context) =>
            //            {
            //                var watch=new Stopwatch();
            //                watch.Start();
            //                ConcurrentRequests++;
            //var text=0.0;
            //                await Task.Run(()=>text = calcPi(100000000));
            //                context.Response.Headers.Add("concurrent-requests", ConcurrentRequests.ToString());
            //                ConcurrentRequests--;
            //                watch.Stop();
            //                context.Response.Headers.Add("execution-time",watch.ElapsedMilliseconds.ToString());
            //                await context.Response.WriteAsync((text*watch.ElapsedTicks).ToString());
            //            });
            app.UseMyMiddleware();
            app.UseMvc();

        }

        private double calcPi(int iterations)
        {
            var pi = 1d;
            for (var i = 1; i < iterations; i++)
            {
                if (i % 2 == 0)
                    pi += 1d / (2d * i + 1);
                else
                    pi -= 1d / (2d * i + 1);
            }
            return pi*4;
        }
    }

    public class MyMiddleware
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Do something with context near the beginning of request processing.

            await _next.Invoke(context);

            // Clean up.
        }
    }

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }
}
