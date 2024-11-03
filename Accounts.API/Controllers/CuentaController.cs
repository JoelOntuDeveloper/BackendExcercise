using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Util.DTO;

namespace Accounts.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase {

        private readonly ICuentaService _cuentaService; 

        public CuentaController(ICuentaService cuentaService) {
            _cuentaService = cuentaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaDTO>>> GetAllCuentas() {
            var cuentas = await _cuentaService.GetAllCuentasAsync();
            return Ok(cuentas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CuentaDTO>>> GetCuentaById(int id) {
            var cuenta = await _cuentaService.GetCuentaByIdAsync(id);
            if (cuenta == null) {
                return NotFound();
            }
            return Ok(cuenta);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCuenta([FromBody] CuentaDTO cuentaDto) {

            await _cuentaService.CreateCuentaAsync(cuentaDto);
            return Ok(cuentaDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCuenta([FromBody] CuentaDTO cuentaDto) {

            await _cuentaService.UpdateCuentaAsync(cuentaDto);
            return Ok(cuentaDto);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCuenta(int id) {

            await _cuentaService.DeleteCuentaAsync(id);
            return Ok(id);
        }

    }
}
