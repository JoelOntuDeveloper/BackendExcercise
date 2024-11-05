using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Util.DTO;

namespace Customers.API.IntegrationTest.Tests {
    public class ClienteControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>> {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public ClienteControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetClientes_DeberiaRetornarListaDeClientes() {
            var response = await _client.GetAsync("/api/cliente");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var clientes = JsonSerializer.Deserialize<List<ClienteDTO>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(clientes);
            Assert.IsType<List<ClienteDTO>>(clientes);
            Assert.True(clientes.Count > 0);
        }

        [Fact]
        public async Task GetClienteByIdentificacion_ClienteExiste_DeberiaRetornarCliente() {
            // Arrange
            var identificacion = "1742538690";

            // Seeding data into the in-memory database
            using (var scope = _factory.Services.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<TestDbContext>();
                context.Clientes.AddRange(createClientes());
                context.SaveChanges();
            }

            // Act
            var response = await _client.GetAsync($"/api/cliente/searchByIdentificacion/{identificacion}");

            // Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var cliente = JsonSerializer.Deserialize<ClienteDTO>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(cliente);
            Assert.Equal(identificacion, cliente.Identificacion);
            Assert.Equal("Jose Lema", cliente.Nombre);
        }

        [Fact]
        public async Task GetClienteByIdentificacion_ClienteNoExiste_DeberiaRetornarNotFound() {
            // Arrange
            var identificacion = "9999999999";

            // Act
            var response = await _client.GetAsync($"/api/cliente/searchByIdentificacion/{identificacion}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        #region private Methods
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