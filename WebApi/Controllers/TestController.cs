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
        private IUserService userService;
        public TestController(ITestService testService, IUserService userService)
        {
            this.testService = testService;
            this.userService = userService;
        }
        [HttpGet]
        public string Test()
        {
            var list = userService.GetUserList();
            return testService.Test();
        }
    }
}
