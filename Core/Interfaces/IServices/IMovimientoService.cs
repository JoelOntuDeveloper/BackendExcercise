using Util.DTO;

namespace Core.Interfaces.IServices {
    public interface IMovimientoService 
    {
        #region CRUD
        Task<MovimientoDTO> GetMovimientoByIdAsync(int id);
        Task<IEnumerable<MovimientoDTO>> GetAllMovimientosAsync();
        Task CreateMovimientoAsync(MovimientoDTO Movimiento);
        #endregion

    }
}
