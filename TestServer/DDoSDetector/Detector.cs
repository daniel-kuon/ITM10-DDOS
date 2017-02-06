using System;
using System.Linq;
using System.Threading.Tasks;
using DDoSDetector.Models;
using DDoSDetector.Multipliers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DDoSDetector
{
    public class Detector
    {
        public const string RequestCosts = "RequestCosts";

        private readonly RequestDelegate _next;
        private readonly ILogger<Detector> _logger;

        public Detector(RequestDelegate next, ILogger<Detector> logger)
        {
            _next = next;
            _logger = logger;
            Repository.Configuration = Configuration;
        }

        public Configuration Configuration { get; set; } = Configuration.GetDefaultConfiguration();

        public async Task Invoke(HttpContext context)
        {
            Request request = Repository.AddRequest(context.Request);
            Task.Delay(Configuration.CostKeepingTime * 60 * 100).ContinueWith((t) => Repository.RemoveRequest(request));
            ClientIp clientIp = request.ClientIp;
            if (clientIp.IsWhiteListed)
            {
                await _next.Invoke(context);
            }
            else
            {
                if (clientIp.IsBlackListed || clientIp.CurrentRequestCosts > Configuration.BlockingLimitPerIp)
                {
                    _logger.LogError(0, $"Request blocked from: {clientIp.Ip}");
                    request.IsBlocked = true;
                    request.Costs = Configuration.BlockingLimitPerIp;
                    context.Abort();
                    return;
                }
                Task requestTask = _next.Invoke(context);
                request.Costs = CalculateStaticCosts(request);
                await requestTask;
                CalculateDynamicRequestCosts(context, request);
                foreach (ICostMultiplier multiplier in Configuration.CostMultipliers)
                {
                    request.Costs = request.Costs * (int)multiplier.Calculate(request);
                }
                //Task.Run(() => CheckBlocking(request));
                _logger.LogInformation(0,
                    $"Request from: {clientIp.Ip}, Costs: {request.Costs}, Costs for IP: {clientIp.CurrentRequestCosts}, Duration: {DateTime.Now.Subtract(request.Time).Milliseconds} ms");
            }
        }

        private void CalculateDynamicRequestCosts(HttpContext context, Request request)
        {
            if (Configuration.HttpErrorCosts.ContainsKey(context.Response.StatusCode))
                request.Costs = Math.Max(request.Costs, Configuration.HttpErrorCosts[context.Response.StatusCode]);
            if (context.Items.ContainsKey(RequestCosts) && context.Items[RequestCosts] is int)
                request.Costs = Math.Max(request.Costs, (int)context.Items[RequestCosts]);
        }

        private void CheckBlocking(Request request)
        {
            ClientIp clientIp = request.ClientIp;
            if (DateTime.Now.Subtract(clientIp.BlockingTime).TotalMinutes <= Configuration.CostKeepingTime ||
            !clientIp.IsWhiteListed && clientIp.CurrentRequestCosts > Configuration.BlockingLimitPerIp)
            {
                clientIp.BlockingTime = DateTime.Now;
                request.BlockingTime = clientIp.BlockingTime;
            }
        }

        private int CalculateStaticCosts(Request request)
        {
            int costs =
                Configuration.UrlPathCosts.Where(uPC => uPC.Key.IsMatch(request.Address))
                    .Select(uPC => uPC.Value)
                    .DefaultIfEmpty(Configuration.DefaultRequestCosts)
                    .Max();
            return costs;
        }
    }
}