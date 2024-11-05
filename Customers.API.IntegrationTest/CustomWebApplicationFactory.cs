using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.API.IntegrationTest {
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class {
        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            builder.ConfigureServices(services => {
                // Eliminar la configuración existente del DbContext si la hay
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TestDbContext>));
                if (descriptor != null) {
                    services.Remove(descriptor);
                }

                // Agregar DbContext usando base de datos en memoria
                services.AddDbContext<TestDbContext>(options => {
                    options
                    .UseInMemoryDatabase("InMemoryDbForTesting")
                    .EnableSensitiveDataLogging();

                });

                // Construir el proveedor de servicios
                var sp = services.BuildServiceProvider();

                // Crear un alcance de servicios
                using (var scope = sp.CreateScope()) {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<TestDbContext>();
                    db.Database.EnsureCreated(); // Crear la base de datos en memoria
                }
            });
        }
    }
}