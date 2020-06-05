using IServices;
using System;

namespace Services
{
    public class TestService : ITestService
    {
        public string Test()
        {
            return "test";
        }
    }
}
