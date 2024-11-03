using Core.Entities;

namespace Core.Interfaces {
    public interface IMovimientoRepository: IRepository<Movimiento> 
    {
        Task<Movimiento> GetLastMovimientoByCuentaId(int cuentaId);

    }
}
