using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Util.DTO;

namespace Accounts.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase {

        private readonly IReportService _reportService;

        public ReportesController(IReportService reportService) { 
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<EstadoCuentasDTO>> getEstadoCuentas(string fechaInicio, string fechaFin, string identificacion) {

            var estadoCuentas = await _reportService.GetEstadoCuentaAsync(fechaInicio, fechaFin, identificacion);

            return Ok(estadoCuentas);
        }
    }
}
