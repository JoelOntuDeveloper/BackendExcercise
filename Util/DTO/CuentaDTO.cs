using System.Text.Json.Serialization;

namespace Util.DTO {
    public class CuentaDTO {
        public CuentaDTO() { }

        public int CuentaId { get; set; }
        public int ClienteId { get; set; }
        public string NumeroCuenta { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NombreCliente { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Identificacion { get; set; }
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal SaldoDisponible { get; set; }
        public bool Estado { get; set; }

        public IList<MovimientoDTO>? Movimientos { get; set; } = null!;
    }
}
