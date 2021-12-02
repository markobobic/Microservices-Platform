using Microsoft.AspNetCore.Mvc;
using System;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController() { }
        
        [HttpPost]
        public IActionResult Test123()
        {
            Console.WriteLine("Test");
            return Ok("Test");
        }
        
    }
}
