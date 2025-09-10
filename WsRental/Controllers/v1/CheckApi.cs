using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WsRental.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class CheckApi : ControllerBase
    {
        public CheckApi()
        {
        }

        [HttpGet("check")]
        public ActionResult<dynamic> Check()
        {
            return Ok();
        }
    }
}
