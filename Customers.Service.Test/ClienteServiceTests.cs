using AutoMapper;
using Common.Exceptions;
using Core.Entities;
using Core.Interfaces;
using Moq;
using Service.CRUDServices;
using Util.DTO;

namespace Customers.Service.Test {
    public class ClienteServiceTests {

        private readonly ClienteService _clienteService;
        private readonly Mock<IClienteRepository> _mockClienteRepository;
        private readonly Mock<IMapper> _mockMapper;

        List<Cliente> _clientes;
        List<ClienteDTO> _clientesDTO;

        public ClienteServiceTests() {
            _mockClienteRepository = new Mock<IClienteRepository>();
            _mockMapper = new Mock<IMapper>();

            _clienteService = new ClienteService(_mockClienteRepository.Object, _mockMapper.Object);

            _clientes = createClientes();

            _clientesDTO = _clientes.Select(c => new ClienteDTO {
                ClienteId = c.ClienteId,
                Nombre = c.Persona.Nombre,
                Direccion = c.Persona.Direccion
            }).ToList();
        }


        #region GetAllClientesAsync TESTS
        [Fact]
        public async Task GetAllClientesAsync_DeberiaRetornarListaDeClienteDTO() {
            // Arrange
            _mockClienteRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_clientes);

            _mockMapper.Setup(m => m.Map<ClienteDTO>(It.IsAny<Persona>()))
                .Returns((Persona persona) => new ClienteDTO { Nombre = persona.Nombre, Direccion = persona.Direccion, Estado = true });

            // Act
            var result = await _clienteService.GetAllClientesAsync();

            // Assert: Verificar que el resultado contiene los datos mapeados correctamente
            Assert.NotNull(result);
            Assert.Equal(_clientesDTO.Count, result.Count());

            int i = 0;
            foreach (var clienteDTO in _clientesDTO) {
                Assert.Equal(clienteDTO.ClienteId, result.ToArray()[i].ClienteId);
                Assert.Equal(clienteDTO.Nombre, result.ToArray()[i].Nombre);
                Assert.Equal(clienteDTO.Direccion, result.ToArray()[i].Direccion);
                Assert.Equal(clienteDTO.Estado, result.ToArray()[i].Estado);

                i++;
            }

            // Verificar que el método del repositorio y el mapper fueron llamados
            _mockClienteRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            _mockMapper.Verify(m => m.Map<ClienteDTO>(It.IsAny<Persona>()), Times.Exactly(_clientes.Count));

        }
        #endregion

        #region GetClienteByIdAsync TESTS
        [Fact]
        public async Task GetClienteByIdAsync_ClienteExiste_DeberiaRetornarClienteDTO() {
            // Arrange
            int clienteId = 1;

            var clienteDtoEsperado = _clientesDTO.First();

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId))
                .ReturnsAsync(_clientes.First());

            _mockMapper.Setup(m => m.Map<ClienteDTO>(_clientes.First().Persona))
                .Returns(_clientesDTO.First());

            // Act
            var result = await _clienteService.GetClienteByIdAsync(clienteId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clienteDtoEsperado.ClienteId, result.ClienteId);
            Assert.Equal(clienteDtoEsperado.Nombre, result.Nombre);
            Assert.Equal(clienteDtoEsperado.Direccion, result.Direccion);
            Assert.Equal(clienteDtoEsperado.Estado, result.Estado);

            _mockClienteRepository.Verify(repo => repo.GetByIdAsync(clienteId), Times.Once);
            _mockMapper.Verify(m => m.Map<ClienteDTO>(_clientes.First().Persona), Times.Once);
        }

        [Fact]
        public async Task GetClienteByIdAsync_ClienteNoExiste_DeberiaLanzarNotFoundException() {
            // Arrange
            int clienteId = 1;

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId))
                .ReturnsAsync((Cliente)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _clienteService.GetClienteByIdAsync(clienteId));

            _mockClienteRepository.Verify(repo => repo.GetByIdAsync(clienteId), Times.Once);
        }

        [Fact]
        public async Task GetClienteByIdAsync_ExcepcionGeneral_DeberiaLanzarValidationException() {
            // Arrange
            int clienteId = 1;

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId))
                .ThrowsAsync(new Exception("Error simulado"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _clienteService.GetClienteByIdAsync(clienteId));

            Assert.Equal("Error al buscar al cliente. Error simulado", exception.Message);
            _mockClienteRepository.Verify(repo => repo.GetByIdAsync(clienteId), Times.Once);
        }
        #endregion

        #region CreateClienteAsync TESTS
        [Fact]
        public async Task CreateClienteAsync_ClienteDTOValido_DeberiaAgregarCliente() {
            // Arrange
            var clienteDto = _clientesDTO.First();

            var clienteEsperado = _clientes.First();
            var personaEsperada = _clientes.First().Persona;

            _mockMapper.Setup(m => m.Map<Cliente>(clienteDto)).Returns(clienteEsperado);
            _mockMapper.Setup(m => m.Map<Persona>(clienteDto)).Returns(personaEsperada);

            _mockClienteRepository.Setup(repo => repo.AddAsync(clienteEsperado)).Returns(Task.CompletedTask);

            // Act
            await _clienteService.CreateClienteAsync(clienteDto);

            // Assert
            _mockMapper.Verify(m => m.Map<Cliente>(clienteDto), Times.Once);
            _mockMapper.Verify(m => m.Map<Persona>(clienteDto), Times.Once);
            _mockClienteRepository.Verify(repo => repo.AddAsync(It.Is<Cliente>(c => c.Persona == personaEsperada)), Times.Once);
        }

        [Fact]
        public async Task CreateClienteAsync_ExcepcionAlAgregar_DeberiaLanzarValidationException() {
            // Arrange
            var clienteDto = _clientesDTO.First();

            var clienteEsperado = _clientes.First();
            var personaEsperada = _clientes.First().Persona;

            _mockMapper.Setup(m => m.Map<Cliente>(clienteDto)).Returns(clienteEsperado);
            _mockMapper.Setup(m => m.Map<Persona>(clienteDto)).Returns(personaEsperada);

            _mockClienteRepository.Setup(repo => repo.AddAsync(clienteEsperado))
                .ThrowsAsync(new Exception("Error simulado"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _clienteService.CreateClienteAsync(clienteDto));
            Assert.Equal("Error al crear al cliente. Error simulado", exception.Message);

            _mockMapper.Verify(m => m.Map<Cliente>(clienteDto), Times.Once);
            _mockMapper.Verify(m => m.Map<Persona>(clienteDto), Times.Once);
            _mockClienteRepository.Verify(repo => repo.AddAsync(clienteEsperado), Times.Once);
        }
        #endregion

        #region UpdateClienteAsync TESTS
        [Fact]
        public async Task UpdateClienteAsync_ClienteExiste_DeberiaActualizarCliente() {
            // Arrange
            var clienteDto = _clientesDTO.First();
            var clienteExistente = _clientes.Last();

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteDto.ClienteId))
                .ReturnsAsync(clienteExistente);

            _mockClienteRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Cliente>()))
                .Returns(Task.CompletedTask);

            // Act
            await _clienteService.UpdateClienteAsync(clienteDto);

            // Assert
            Assert.Equal(clienteDto.Estado, clienteExistente.Estado);
            Assert.Equal(clienteDto.Nombre, clienteExistente.Persona.Nombre);
            Assert.Equal(clienteDto.Edad, clienteExistente.Persona.Edad);
            Assert.Equal(clienteDto.Identificacion, clienteExistente.Persona.Identificacion);
            Assert.Equal(clienteDto.Direccion, clienteExistente.Persona.Direccion);
            Assert.Equal(clienteDto.Genero, clienteExistente.Persona.Genero);
            Assert.Equal(clienteDto.Telefono, clienteExistente.Persona.Telefono);

            _mockClienteRepository.Verify(repo => repo.UpdateAsync(clienteExistente), Times.Once);
        }

        [Fact]
        public async Task UpdateClienteAsync_ClienteNoExiste_DeberiaLanzarNotFoundException() {
            // Arrange
            var clienteDto = new ClienteDTO { ClienteId = 1 };

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteDto.ClienteId))
                .ReturnsAsync((Cliente)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _clienteService.UpdateClienteAsync(clienteDto));

            _mockClienteRepository.Verify(repo => repo.GetByIdAsync(clienteDto.ClienteId), Times.Once);
            _mockClienteRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Cliente>()), Times.Never);
        }

        [Fact]
        public async Task UpdateClienteAsync_ExcepcionGeneral_DeberiaLanzarValidationException() {
            // Arrange
            var clienteDto = _clientesDTO.First();
            var clienteExistente = _clientes.Last();

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteDto.ClienteId))
                .ReturnsAsync(clienteExistente);

            _mockClienteRepository.Setup(repo => repo.UpdateAsync(clienteExistente))
                .ThrowsAsync(new Exception("Error simulado"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _clienteService.UpdateClienteAsync(clienteDto));

            Assert.Equal("Error al actualizar al cliente. Error simulado", exception.Message);
            _mockClienteRepository.Verify(repo => repo.UpdateAsync(clienteExistente), Times.Once);
        }
        #endregion

        #region DeleteClienteAsync TESTS
        [Fact]
        public async Task DeleteClienteAsync_ClienteExiste_DeberiaDesactivarCliente() {
            // Arrange
            int clienteId = 1;
            var clienteExistente = _clientes.First();

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId))
                .ReturnsAsync(clienteExistente);

            _mockClienteRepository.Setup(repo => repo.UpdateAsync(clienteExistente))
                .Returns(Task.CompletedTask);

            // Act
            await _clienteService.DeleteClienteAsync(clienteId);

            // Assert
            Assert.False(clienteExistente.Estado); // Verifica que el estado sea false
            _mockClienteRepository.Verify(repo => repo.UpdateAsync(clienteExistente), Times.Once);
        }

        [Fact]
        public async Task DeleteClienteAsync_ClienteNoExiste_DeberiaLanzarNotFoundException() {
            // Arrange
            int clienteId = 1;

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId))
                .ReturnsAsync((Cliente)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _clienteService.DeleteClienteAsync(clienteId));

            _mockClienteRepository.Verify(repo => repo.GetByIdAsync(clienteId), Times.Once);
            _mockClienteRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Cliente>()), Times.Never);
        }

        [Fact]
        public async Task DeleteClienteAsync_ExcepcionGeneral_DeberiaLanzarValidationException() {
            // Arrange
            int clienteId = 1;
            var clienteExistente = _clientes.First();

            _mockClienteRepository.Setup(repo => repo.GetByIdAsync(clienteId))
                .ReturnsAsync(clienteExistente);

            _mockClienteRepository.Setup(repo => repo.UpdateAsync(clienteExistente))
                .ThrowsAsync(new Exception("Error simulado"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _clienteService.DeleteClienteAsync(clienteId));

            Assert.Equal("Error al eliminar al cliente. Error simulado", exception.Message);
            _mockClienteRepository.Verify(repo => repo.UpdateAsync(clienteExistente), Times.Once);
        }
        #endregion

        #region GetClienteByIdentificacionAsync TESTS
        [Fact]
        public async Task GetClienteByIdentificacionAsync_ClienteExiste_DeberiaRetornarClienteDTO() {
            // Arrange
            string identificacion = "1742538690";
            var clienteDtoEsperado = _clientesDTO.First();

            _mockClienteRepository.Setup(repo => repo.GetByIdentificacionAsync(identificacion))
                .ReturnsAsync(_clientes.First());

            _mockMapper.Setup(m => m.Map<ClienteDTO>(_clientes.First().Persona))
                .Returns(_clientesDTO.First());

            // Act
            var result = await _clienteService.GetClienteByIdentificacionAsync(identificacion);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clienteDtoEsperado.ClienteId, result.ClienteId);
            Assert.Equal(clienteDtoEsperado.Nombre, result.Nombre);
            Assert.Equal(clienteDtoEsperado.Direccion, result.Direccion);
            Assert.Equal(clienteDtoEsperado.Estado, result.Estado);

            _mockClienteRepository.Verify(repo => repo.GetByIdentificacionAsync(identificacion), Times.Once);
            _mockMapper.Verify(m => m.Map<ClienteDTO>(_clientes.First().Persona), Times.Once);
        }

        [Fact]
        public async Task GetClienteByIdentificacionAsync_ClienteNoExiste_DeberiaLanzarNotFoundException() {
            // Arrange
            string identificacion = "1742538690";

            _mockClienteRepository.Setup(repo => repo.GetByIdentificacionAsync(identificacion))
                .ReturnsAsync((Cliente)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _clienteService.GetClienteByIdentificacionAsync(identificacion));

            _mockClienteRepository.Verify(repo => repo.GetByIdentificacionAsync(identificacion), Times.Once);
        }

        [Fact]
        public async Task GetClienteByIdentificacionAsync_ExcepcionGeneral_DeberiaLanzarValidationException() {
            // Arrange
            string identificacion = "1742538690";

            _mockClienteRepository.Setup(repo => repo.GetByIdentificacionAsync(identificacion))
                .ThrowsAsync(new Exception("Error simulado"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _clienteService.GetClienteByIdentificacionAsync(identificacion));

            Assert.Equal("Error al buscar al cliente. Error simulado", exception.Message);
            _mockClienteRepository.Verify(repo => repo.GetByIdentificacionAsync(identificacion), Times.Once);
        } 
        #endregion

        #region Private Methods
        private List<Cliente> createClientes() {

            var persona1 = new Persona {
                Id = 1,
                Nombre = "Jose Lema",
                Genero = "M",
                Edad = 25,
                Identificacion = "1742538690",
                Direccion = "Otavalo sn y principal",
                Telefono = "098254785"
            };

            var persona2 = new Persona {
                Id = 2,
                Nombre = "Marianela Montalvo",
                Genero = "F",
                Edad = 25,
                Identificacion = "1742948690",
                Direccion = "Amazonas y NNUU",
                Telefono = "097548965"
            };

            var persona3 = new Persona {
                Id = 3,
                Nombre = "Juan Osorio",
                Genero = "M",
                Edad = 25,
                Identificacion = "1742558690",
                Direccion = "13 junio y Equinoccial",
                Telefono = "098874587"
            };

            return new List<Cliente>
            {
                new Cliente { ClienteId = 1, Contrasenia="1234", Estado = true, Persona = persona1 },
                new Cliente { ClienteId = 2, Contrasenia="5678", Estado = true, Persona = persona2 },
                new Cliente { ClienteId = 3, Contrasenia="1245", Estado = true, Persona = persona3 },
            };
        } 
        #endregion
    }
}