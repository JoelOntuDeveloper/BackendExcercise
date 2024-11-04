using Util.DTO;

namespace Core.Interfaces.IServices {
    public interface IReportService
    {
        Task<EstadoCuentasDTO> GetEstadoCuentaAsync(string fechaInicio, string fechaFin, string identificacion);
    }
}
