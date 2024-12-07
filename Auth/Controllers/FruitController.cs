using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/fruits")]
    [ApiController]
    [Authorize]
    public class FruitController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var fruits = await Task.FromResult(new string[] { "apple", "banana", "kiwi" });
            return Ok(fruits);
        }
    }
}
