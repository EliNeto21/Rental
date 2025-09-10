using Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Services.MotorcycleService;
using Services.IMotorcycleService;

namespace WsRental.Controllers.v1
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMotorcycleService _motorcycleService;

        public MotorcyclesController(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        [HttpPost()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MotorcycleViewModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<GenericResult<Motorcycle>>> InsertMotorcycleAsync([FromBody] MotorcycleViewModel motorcycle)
        {
            try
            {
                var result = await _motorcycleService.CreateAsync(motorcycle, CancellationToken.None);

                if (result.Code > 200)
                {
                    return BadRequest(result.Mensagem);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("by-plate/{plate}")]
        [AllowAnonymous]
        public async Task<ActionResult<GenericResult<Motorcycle>>> GetMotorcycleByPlateAsync(string plate)
        {
            var result = await _motorcycleService.GetByPlateAsync(plate, CancellationToken.None);

            if (result.Code > 200)
            {
                return BadRequest(result.Mensagem);
            }

            return Ok(result);
        }

        [HttpGet("by-id/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<GenericResult<Motorcycle>>> GetMotorcycleByIdAsync(Guid id)
        {
            var result = await _motorcycleService.GetByIdAsync(id, CancellationToken.None);

            if (result.Code > 200)
            {
                return BadRequest(result.Mensagem);
            }

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Motorcycle>> UpdateMotorcycleAsync([FromHeader] string plate, Guid id)
        {
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteMotorcycleAsync(Guid id)
        {
            return Ok();
        }
    }
}
