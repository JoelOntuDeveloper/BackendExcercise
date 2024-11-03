using Core.Entities;
using Core.Interfaces.IServices;
using Core.Interfaces;
using AutoMapper;
using Util.DTO;
using Service.Exceptions;

namespace Service.CRUDServices
{
    public class ClienteService : IClienteService
    {

        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        #region CRUD
        public async Task<ClienteDTO> GetClienteByIdAsync(int id)
        {

            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);

                if (cliente == null)
                {
                    throw new NotFoundException($"Cliente con ID {id} no encontrado.");
                }

                ClienteDTO clienteDTO = _mapper.Map<ClienteDTO>(cliente.Persona);
                clienteDTO.ClienteId = cliente.ClienteId;
                clienteDTO.Estado = cliente.Estado;

                return clienteDTO;

            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Error al buscar al cliente. " + ex.Message);
            }
        }

        public async Task<IEnumerable<ClienteDTO>> GetAllClientesAsync()
        {

            var clientes = await _clienteRepository.GetAllAsync();

            IList<ClienteDTO> clientesDTO = new List<ClienteDTO>();

            foreach (var cliente in clientes)
            {
                ClienteDTO clienteDTO = _mapper.Map<ClienteDTO>(cliente.Persona);
                clienteDTO.ClienteId = cliente.ClienteId;
                clienteDTO.Estado = cliente.Estado;
                clientesDTO.Add(clienteDTO);
            }

            return clientesDTO;
        }

        public async Task CreateClienteAsync(ClienteDTO clienteDto)
        {

            var cliente = _mapper.Map<Cliente>(clienteDto);
            var persona = _mapper.Map<Persona>(clienteDto);

            cliente.Persona = persona;

            await _clienteRepository.AddAsync(cliente);
        }

        public async Task UpdateClienteAsync(ClienteDTO clienteDto)
        {

            try
            {

                var cliente = await _clienteRepository.GetByIdAsync(clienteDto.ClienteId);

                if (cliente == null)
                {
                    throw new NotFoundException($"Cliente con ID {clienteDto.ClienteId} no encontrado.");
                }

                cliente.Estado = clienteDto.Estado;

                cliente.Persona.Edad = clienteDto.Edad;
                cliente.Persona.Identificacion = clienteDto.Identificacion;
                cliente.Persona.Direccion = clienteDto.Direccion;
                cliente.Persona.Nombre = clienteDto.Nombre;
                cliente.Persona.Genero = clienteDto.Genero;
                cliente.Persona.Telefono = clienteDto.Telefono;

                await _clienteRepository.UpdateAsync(cliente);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Error al actualizar al cliente. " + ex.Message);
            }
        }

        //Logic Delete
        public async Task DeleteClienteAsync(int id)
        {

            try
            {

                var cliente = await _clienteRepository.GetByIdAsync(id);

                if (cliente == null)
                {
                    throw new NotFoundException($"Cliente con ID {id} no encontrado.");
                }

                cliente.Estado = false;
                await _clienteRepository.UpdateAsync(cliente);

            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Error al eliminar al cliente. " + ex.Message);
            }
        }
        #endregion

        public async Task<ClienteDTO> GetClienteByIdentificacionAsync(string identificacion)
        {

            try
            {
                var cliente = await _clienteRepository.GetByIdentificacionAsync(identificacion);

                if (cliente == null)
                {
                    throw new NotFoundException($"Cliente con Identificación {identificacion} no encontrado.");
                }

                return _mapper.Map<ClienteDTO>(cliente);

            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Error al eliminar al cliente. " + ex.Message);
            }
        }
    }
}
