using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Service.CRUDServices;
using Util.DTO;

namespace Accounts.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoController : ControllerBase {

        private readonly IMovimientoService _movimientoService;

        public MovimientoController(IMovimientoService movimientoService) {
            _movimientoService = movimientoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimientoDTO>>> GetAllMovimientos() {
            var movimientos = await _movimientoService.GetAllMovimientosAsync();
            return Ok(movimientos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoDTO>> GetMovimientoById(int id) {
            var movimiento = await _movimientoService.GetMovimientoByIdAsync(id);
            return Ok(movimiento);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCuenta([FromBody] MovimientoDTO movimientoDto) {
            await _movimientoService.CreateMovimientoAsync(movimientoDto);
            return Ok(movimientoDto);
        }
    }
}
