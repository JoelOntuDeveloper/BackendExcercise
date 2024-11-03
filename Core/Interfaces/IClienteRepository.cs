using Core.Entities;

namespace Core.Interfaces {
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> GetByIdentificacionAsync(string identificacion);
    }


}
