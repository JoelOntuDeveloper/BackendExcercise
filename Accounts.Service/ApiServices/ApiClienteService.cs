using System.Net.Http.Json;
using Util.DTO;

namespace Accounts.Service.ApiServices {
    public class ApiClienteService {
        
        private readonly HttpClient _httpClient;

        public ApiClienteService(IHttpClientFactory httpClientFactory) {
            _httpClient = httpClientFactory.CreateClient("ClienteService");
        }

        public async Task<ClienteDTO?> GetClienteByIdentificacion(string identificacion) {
            return await _httpClient.GetFromJsonAsync<ClienteDTO>($"api/cliente/searchByIdentificacion/{identificacion}");
        }

    }
}
