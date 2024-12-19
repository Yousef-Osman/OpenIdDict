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
            return Ok("This api is open for anyone to access");
        }

        [HttpGet("GetProtectedResource")]
        [Authorize]
        public IActionResult GetProtectedResource()
        {
            return Ok("OpenIdDict Auth Works");
        }

        [HttpGet("GetAdminResource")]
        [Authorize(Policy = "adminRole")]
        public IActionResult GetAdminResource()
        {
            return Ok("you will see this message if you have admin role");
        }

        [HttpGet("GetContentManagerResource")]
        [Authorize(Policy = "contentManagerRole")]
        public IActionResult GetContentManagerResource()
        {
            return Ok("you will see this message if you have admin or content manager role");
        }
        
        [HttpGet("GetEditorResource")]
        [Authorize(Policy = "editorRole")]
        public IActionResult GetEditorResource()
        {
            return Ok("you will see this message if you have admin, content manager, or editor role");
        }
    }
}
