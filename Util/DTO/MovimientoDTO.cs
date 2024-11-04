using System.Text.Json.Serialization;

namespace Util.DTO {
    public class MovimientoDTO {

        public MovimientoDTO() { }

        public DateTime? Fecha { get; set; }
        public decimal Valor { get; set; }
        public decimal? Saldo { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string NumeroCuenta { get; set; }
        public string? TipoMovimiento { get; set; }
    }
}
