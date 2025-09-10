using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Services.RentalService;

namespace WsRental.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class RentalController : ControllerBase
    {
        readonly private RentalService _rentalService;

        [HttpPost("rental")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Rental>> InsertRentalAsync()
        {
            return Ok();
        }

        [HttpGet("rental/{rentalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Rental>> GetRentalByIdAsync([FromRoute] Guid rentalId)
        {
            return Ok();
        }

        [HttpPut("rental/{rentalId}/return")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ReturnRentalAsync([FromRoute] Guid rentalId)
        {
            return Ok();
        }
    }
}
