using Core.Entities;

namespace Core.Interfaces {
    public interface IMovimientoRepository: IRepository<Movimiento> 
    {
        Task<Movimiento> GetLastMovimientoByCuentaId(int cuentaId);

        Task<IEnumerable<Movimiento>> GetMovimientosByFechasAndClienteId(DateTime fechaInicio, DateTime fechaFin, int clienteId);

    }
}
