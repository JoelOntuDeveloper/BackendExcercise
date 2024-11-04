using AutoMapper;
using Common.Exceptions;
using Core.Entities;
using Core.Interfaces.IServices;
using Core.Interfaces;
using Util.Constants;
using Util.DTO;

namespace Accounts.Service.CRUDServices {
    public class MovimientoService : IMovimientoService {

        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IMapper _mapper;

        public MovimientoService(IMovimientoRepository movimientoRepository, ICuentaRepository cuentaRepository, IMapper mapper) {
            _movimientoRepository = movimientoRepository;
            _cuentaRepository = cuentaRepository;
            _mapper = mapper;
        }

        #region CRUD
        public async Task<IEnumerable<MovimientoDTO>> GetAllMovimientosAsync() {

            var movimientos = await _movimientoRepository.GetAllAsync();

            IList<MovimientoDTO> movimientosDTO = new List<MovimientoDTO>();

            foreach (var movimiento in movimientos) {
                MovimientoDTO movimientoDTO = _mapper.Map<MovimientoDTO>(movimiento);
                movimientoDTO.NumeroCuenta = movimiento.Cuenta.NumeroCuenta;

                movimientosDTO.Add(movimientoDTO);
            }

            return movimientosDTO;
        }

        public async Task<MovimientoDTO> GetMovimientoByIdAsync(int id) {

            var movimiento = await _movimientoRepository.GetByIdAsync(id);

            MovimientoDTO movimientoDTO = _mapper.Map<MovimientoDTO>(movimiento);
            movimientoDTO.NumeroCuenta = movimiento.Cuenta.NumeroCuenta;

            return movimientoDTO;
        }

        public async Task CreateMovimientoAsync(MovimientoDTO movimientoDTO) {
            try {
                var cuenta = await _cuentaRepository.GetByNumeroCuenta(movimientoDTO.NumeroCuenta);

                if (cuenta == null) { throw new NotFoundException($"No existe la cuenta: '{movimientoDTO.NumeroCuenta}'"); }

                decimal saldoRestante = 0;
                var lastMovimiento = await _movimientoRepository.GetLastMovimientoByCuentaId(cuenta.CuentaId);

                if (lastMovimiento != null) {
                    saldoRestante = lastMovimiento.Saldo + movimientoDTO.Valor;
                } else {
                    saldoRestante = cuenta.SaldoInicial + movimientoDTO.Valor;
                }

                if (saldoRestante < 0) { throw new ValidationException($"Saldo no disponible"); }

                Movimiento movimiento = new Movimiento();
                movimiento.CuentaId = cuenta.CuentaId;
                movimiento.TipoMovimiento = movimientoDTO.Valor >= 0 ? TipoMovimiento.DEPOSITO : TipoMovimiento.RETIRO;
                movimiento.Valor = Math.Abs(movimientoDTO.Valor);
                movimiento.Saldo = saldoRestante;
                movimiento.Fecha = DateTime.Now;

                await _movimientoRepository.AddAsync(movimiento);

            } catch (NotFoundException) {
                throw;
            } catch (ValidationException) {
                throw;
            } catch (Exception ex) {
                throw new ValidationException("Error al actualizar al cliente. " + ex.Message);
            }
        }
        #endregion
    }
}
