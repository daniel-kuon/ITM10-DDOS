using DDoSDetector;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("pi")]
    public class PiController : Controller
    {
        
        [RequestCosts(700)]
        [HttpGet("{iterations}")]
        public double Pi(long iterations)
        {
            return calcPi(iterations);
        }

        private double calcPi(long iterations)
        {
            var pi = 1d;
            for (var i = 1; i < iterations; i++)
            {
                if (i % 2 == 0)
                    pi += 1d / (2d * i + 1);
                else
                    pi -= 1d / (2d * i + 1);
            }
            return pi * 4;
        }
    }
}