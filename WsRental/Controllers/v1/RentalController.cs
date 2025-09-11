using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Services.IRentalService;
using Domain.ViewModel;
using static Domain.Entities.Rental;

namespace WsRental.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class RentalController : ControllerBase
    {
        readonly private IRentalService _rentalService;

        public RentalController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GenericResult<Rental>>> InsertRentalAsync([FromBody] RentalViewModel rentalViewModel )
        {
            var result = await _rentalService.CreateAsync(rentalViewModel, CancellationToken.None);

            if (result.Code > 200)
            {
                return BadRequest(result.Mensagem);
            }

            return Ok(result);
        }

        [HttpGet("by-id/{rentalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GenericResult<Rental>>> GetRentalByIdAsync([FromRoute] Guid rentalId)
        {
            var result = await _rentalService.GetByIdAsync(rentalId, CancellationToken.None);

            if (result.Code > 200)
            {
                return BadRequest(result.Mensagem);
            }

            return Ok(result);
        }

        [HttpPut("by-id/{id:guid}/return")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GenericResult<dynamic>>> ReturnRentalAsync([FromRoute] Guid id)
        {
            var result = await _rentalService.ReturnRentalAsync(id, DateOnly.FromDateTime(DateTime.Now), CancellationToken.None);

            if (result.Code > 200)
            {
                return BadRequest(result.Mensagem);
            }

            return Ok(result);
        }
    }
}
