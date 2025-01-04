using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourcesController : ControllerBase
    {
        [HttpGet("GetOpenResource")]
        public IActionResult GetOpenResource()
        {
            return Ok("Resources Server success result, This api is accessible for all user");
        }

        [HttpGet("GetProtectedResource")]
        [Authorize]
        public IActionResult GetProtectedResource()
        {
            return Ok("Resources Server success result, This api is only accessible for authenticated user.");
        }

        [HttpGet("GetAdminResource")]
        [Authorize(Policy = "adminRole")]
        public IActionResult GetAdminResource()
        {
            return Ok("Resources Server success result, This api is only accessible for admins.");
        }

        [HttpGet("GetContentManagerResource")]
        [Authorize(Policy = "contentManagerRole")]
        public IActionResult GetContentManagerResource()
        {
            return Ok("Resources Server success result, This api is only accessible for content managers.");
        }
        
        [HttpGet("GetEditorResource")]
        [Authorize(Policy = "editorRole")]
        public IActionResult GetEditorResource()
        {
            return Ok("Resources Server success result, This api is only accessible for content editors.");
        }
    }
}
