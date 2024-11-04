using Util.DTO;

namespace Core.Interfaces.IServices {
    public interface ICuentaService {

        #region CRUD
        Task<CuentaDTO> GetCuentaByIdAsync(int id);
        Task<IEnumerable<CuentaDTO>> GetAllCuentasAsync();
        Task CreateCuentaAsync(CuentaDTO cuenta);
        Task UpdateCuentaAsync(CuentaDTO cuenta);
        Task DeleteCuentaAsync(int id);
        #endregion
    }
}
