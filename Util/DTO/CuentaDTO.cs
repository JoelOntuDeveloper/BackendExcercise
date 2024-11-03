using System.Text.Json.Serialization;

namespace Util.DTO {
    public class CuentaDTO {
        public CuentaDTO() { }

        public int CuentaId { get; set; }
        public int ClienteId { get; set; }
        public string NumeroCuenta { get; set; }
        public string? NombreCliente { get; set; }
        public string? Identificacion { get; set; }
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal SaldoDisponible { get; set; }
        public bool Estado { get; set; }

        [JsonIgnore]
        public IList<MovimientoDTO>? Movimientos { get; set; } = null!;
    }
}
