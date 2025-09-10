using Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Services.ICourierService;

namespace WsRental.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService _courierService;

        public CourierController(ICourierService courierService)
        {
            _courierService = courierService;
        }

        [HttpPost("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CourierViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<GenericResult<Courier>>> InsertCourierAsync([FromBody] Courier courier)
        {
            var result = await _courierService.RegisterAsync(courier, CancellationToken.None);

            return Ok();
        }

        [HttpPost("{courierId}/cnh-image")]
        [AllowAnonymous]
        public async Task<ActionResult<GenericResult<dynamic>>> InsertCnhImageAsync([FromRoute] Guid courierId, IFormFile file)
        {
            var result = await _courierService.UpdateCnhImageAsync(courierId, file.OpenReadStream(), file.ContentType, CancellationToken.None);

            return Ok();
        }
    }
}
