using IServices;
using System;
using System.Diagnostics;
using System.Threading;

namespace Services
{
    public class TestService : ITestService
    {
        public object Test()
        {
            test test = new test();
            Stopwatch stopwatch = new Stopwatch();
            test.CreateModelByRelection();
            stopwatch.Start();
            test.CreateModelByExpression();
            
            stopwatch.Stop();
            var second = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
            test.CreateModelByRelection();
            stopwatch.Stop();
            var second1= stopwatch.ElapsedMilliseconds;
            return "test";
        }
    }
}
