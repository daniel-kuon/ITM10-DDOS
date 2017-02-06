using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DDoSDetector
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class RequestCostsAttribute:ResultFilterAttribute
    {

        public int Costs { get; set; }

        public RequestCostsAttribute(int costs)
        {
            Costs = costs;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.HttpContext.Items.ContainsKey(Detector.RequestCosts))
                context.HttpContext.Items[Detector.RequestCosts] = Costs;
            else
                context.HttpContext.Items.Add(Detector.RequestCosts, Costs);
        }
    }
}