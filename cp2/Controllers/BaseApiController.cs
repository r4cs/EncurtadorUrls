using Microsoft.AspNetCore.Mvc;

namespace cp2.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/")]
    [ApiVersion("1.0")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}