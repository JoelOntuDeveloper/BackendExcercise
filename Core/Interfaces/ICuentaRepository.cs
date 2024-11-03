using Core.Entities;

namespace Core.Interfaces
{
    public interface ICuentaRepository : IRepository<Cuenta> 
    {
        Task<Cuenta> GetByNumeroCuenta(string numeroCuenta);
    }
}
