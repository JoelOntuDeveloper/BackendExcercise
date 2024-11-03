using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.IServices;
using Service.Exceptions;
using Util.DTO;

namespace Service.CRUDServices
{
    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IMapper _mapper;

        public CuentaService(ICuentaRepository cuentaRepository, IMapper mapper) {
            _cuentaRepository = cuentaRepository;
            _mapper = mapper;
        }

        #region CRUD
        public async Task<IEnumerable<CuentaDTO>> GetAllCuentasAsync() {

            try { 
                var cuentas = await _cuentaRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CuentaDTO>>(cuentas);
            } catch (NotFoundException) {
                throw;
            } catch (Exception ex) {
                throw new ValidationException("Error al obtener la cuenta. " + ex.Message);
            }
        }

        public async Task<CuentaDTO> GetCuentaByIdAsync(int id) {

            try {
                var cuenta = await _cuentaRepository.GetByIdAsync(id);
                if (cuenta == null) { throw new NotFoundException($"Cuenta con ID {id} no encontrado."); }

                return _mapper.Map<CuentaDTO>(cuenta);

            } catch (NotFoundException) {
                throw;
            } catch (Exception ex) {
                throw new ValidationException("Error al obtener la cuenta. " + ex.Message);
            }
        }

        public async Task CreateCuentaAsync(CuentaDTO cuentaDTO)
        {
            var cuenta = _mapper.Map<Cuenta>(cuentaDTO);
            await _cuentaRepository.AddAsync(cuenta);
        }

        public async Task UpdateCuentaAsync(CuentaDTO cuentaDTO) {

            try {
                var cuenta = await _cuentaRepository.GetByIdAsync(cuentaDTO.CuentaId);

                if (cuenta == null) { throw new NotFoundException($"Cuenta con ID {cuentaDTO.CuentaId} no encontrado."); }

                cuenta.TipoCuenta = cuentaDTO.TipoCuenta;
                cuenta.SaldoInicial = cuentaDTO.SaldoInicial;

                await _cuentaRepository.UpdateAsync(cuenta);

            } catch (NotFoundException) {
                throw;
            } catch (Exception ex) {
                throw new ValidationException("Error al actualizar la cuenta. " + ex.Message);
            }
        }

        public async Task DeleteCuentaAsync(int id)
        {
            try
            {
                var cuenta = await _cuentaRepository.GetByIdAsync(id);

                if (cuenta == null) { throw new NotFoundException($"Cuenta con ID {id} no encontrado."); }

                cuenta.Estado = false;

                await _cuentaRepository.UpdateAsync(cuenta);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Error al eliminar la cuenta. " + ex.Message);
            }
        }
        #endregion
    }
}
