using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private ITestService testService;
        public TestController(ITestService testService)
        {
            this.testService = testService;
        }
        [HttpGet]
        public string Test()
        {
            return testService.Test();
        }
    }
}
