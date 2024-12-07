using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> AllPeople()
        {
            var people = await Task.FromResult(new string[] { "Jack", "Joe", "Jill" });
            return Ok(people);
        }
    }
}
