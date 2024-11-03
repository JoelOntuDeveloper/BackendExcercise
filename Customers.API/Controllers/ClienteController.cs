using Core.Entities;
using Core.Interfaces.IServices;
using Util.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase {

        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService) { 
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes() {
            var clientes = await _clienteService.GetAllClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id) {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null) {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCliente([FromBody] ClienteDTO clienteDto) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _clienteService.CreateClienteAsync(clienteDto);
            return Ok(clienteDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente([FromBody] ClienteDTO clienteDto) {

            await _clienteService.UpdateClienteAsync(clienteDto);
            return Ok(clienteDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id) {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null) {
                return NotFound();
            }

            await _clienteService.DeleteClienteAsync(id);
            return NoContent();
        }

        [HttpGet("identificacion/{identificacion}")]
        public async Task<ActionResult<Cliente>> GetClienteByIdentificacion(string identificacion) {
            var cliente = await _clienteService.GetClienteByIdentificacionAsync(identificacion);
            if (cliente == null) {
                return NotFound();
            }
            return Ok(cliente);
        }
    }
}
