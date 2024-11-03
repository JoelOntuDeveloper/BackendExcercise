
using Util.DTO;

namespace Core.Interfaces.IServices {
    public interface IClienteService {

        #region CRUD
        Task<ClienteDTO> GetClienteByIdAsync(int id);
        Task<IEnumerable<ClienteDTO>> GetAllClientesAsync();
        Task CreateClienteAsync(ClienteDTO cliente);
        Task UpdateClienteAsync(ClienteDTO cliente);
        Task DeleteClienteAsync(int id); 
        #endregion

        Task<ClienteDTO> GetClienteByIdentificacionAsync(string identificacion);
    }
}
