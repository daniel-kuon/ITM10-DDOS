using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoadTester
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            Console.Write("Enter number of concurrent requests: ");
            var number = int.Parse(Console.ReadLine());
            Console.Write("Enter number of test rounds: ");
            var count = int.Parse(Console.ReadLine());
            var instances = new List<Program>();
            for (int i = 0; i < number; i++)
            {
                instances.Add(new Program() { Address = "http://localhost:5000", TestCount = count });
            }
            //Task.WaitAll(instances.Select(i => Task.Run(() => i.RunTest())).ToArray());
            Task.WaitAll(instances.Select(i =>  i.RunTest()).ToArray());
            Console.WriteLine("Average Concurrent Requests: " + instances.Select(i => i.ConcurrentRequests).Average());
            Console.WriteLine("Average Response time: " + instances.Select(i => i.Duration).Average());
            Console.WriteLine("Max Response time: " + instances.Select(i => i.Duration).Max());
            Console.WriteLine("Min Response time: " + instances.Select(i => i.Duration).Min());
            Main(args);
        }

        public async Task RunTest()
        {
            var duration = 0L;
            var concurrent = 0d;
            MaxDuration = 0;
            MinDuration = 0;
            for (int i = 0; i < TestCount; i++)
            {
                var sw = new Stopwatch();
                sw.Start();
                //WebRequest.CreateHttp(Address).GetResponse();
                //long currentDuration = sw.ElapsedMilliseconds;
                WebResponse webResponse = await WebRequest.CreateHttp(Address).GetResponseAsync();
                sw.Stop();
                long currentDuration = sw.ElapsedMilliseconds;//long.Parse(webResponse.Headers["execution-time"]);
                concurrent += long.Parse(webResponse.Headers["concurrent-requests"]);
                MaxDuration = Math.Max(MaxDuration, currentDuration);
                MinDuration = Math.Min(MinDuration, currentDuration);
                duration += currentDuration;
            }
            Duration = duration / TestCount;
            ConcurrentRequests = concurrent / TestCount;
        }

        public double ConcurrentRequests { get; set; }
        

        public string Address { get; set; }

        public long Duration { get; private set; }

        public long MaxDuration { get; set; }
        public long MinDuration { get; set; }


        public int TestCount { get; set; }
    }
}
