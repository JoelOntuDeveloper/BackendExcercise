using Accounts.Service.ApiServices;
using Common.Exceptions;
using Core.Interfaces;
using Core.Interfaces.IServices;
using Util.DTO;
using Util.General;

namespace Accounts.Service.BussinesServices {
    public class ReportService : IReportService {

        private readonly ApiClienteService _apiClienteService;
        private readonly IMovimientoRepository _movimientoRepository;
        public ReportService(ApiClienteService clienteService, IMovimientoRepository movimientoService) {
            _apiClienteService = clienteService;
            _movimientoRepository = movimientoService;
        }

        public async Task<EstadoCuentasDTO> GetEstadoCuentaAsync(string fechaInicio, string fechaFin, string identificacion) {
            try {
                DateTime fechaInicioDate = FormatDates.ConvertStringToDate(fechaInicio);
                DateTime fechaFinDate = FormatDates.ConvertStringToDate(fechaFin);

                ClienteDTO? clienteDTO = await _apiClienteService.GetClienteByIdentificacion(identificacion);

                if (clienteDTO == null) {
                    throw new NotFoundException($"Cliente con Identificación {identificacion} no encontrado.");
                }

                EstadoCuentasDTO estadoCuenta = new EstadoCuentasDTO();
                estadoCuenta.NombreCliente = clienteDTO.Nombre;
                estadoCuenta.Identificacion = identificacion;
                estadoCuenta.Cuentas = await getCuentasByClienteIdAndMovimientosByFechas(fechaInicioDate, fechaFinDate, clienteDTO);

                return estadoCuenta;

            } catch (NotFoundException) {
                throw;
            } catch (FormatException) {
                throw;
            } catch (Exception ex) {
                throw new ValidationException("Error al obtener el estado de cuenta");
            }
        }

        #region Private Methods
        private async Task<List<CuentaDTO>> getCuentasByClienteIdAndMovimientosByFechas(DateTime fechaInicio, DateTime fechaFin, ClienteDTO clienteDTO) {

            List<CuentaDTO> result = new List<CuentaDTO>();

            var movimientos = await _movimientoRepository.GetMovimientosByFechasAndClienteId(fechaInicio, fechaFin, clienteDTO.ClienteId);

            result = movimientos.GroupBy(mov => mov.Cuenta,
                (key, group) => new CuentaDTO {
                    ClienteId = clienteDTO.ClienteId,
                    CuentaId = key.CuentaId,
                    NumeroCuenta = key.NumeroCuenta,
                    Estado = key.Estado,
                    TipoCuenta = key.TipoCuenta,
                    SaldoInicial = key.SaldoInicial,
                    SaldoDisponible = group.OrderByDescending(mov => mov.Fecha).First().Saldo,
                    Movimientos = group.Select(mov => new MovimientoDTO {
                        Fecha = mov.Fecha,
                        Saldo = mov.Saldo,
                        TipoMovimiento = mov.TipoMovimiento,
                        Valor = mov.Valor,
                    }).ToList(),
                }).ToList();

            return result; 
            #endregion
        }
    }
}
