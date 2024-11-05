using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Util.DTO {
    public class CuentaDTO {
        public CuentaDTO() { }

        public int CuentaId { get; set; }
        public int ClienteId { get; set; }
        [Required(ErrorMessage = "El número de cuenta es obligatorio")]
        public string NumeroCuenta { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NombreCliente { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Identificacion { get; set; }

        [Required(ErrorMessage = "El tipo de cuenta es obligatorio")]
        [RegularExpression("^(AHORROS|CORRIENTE)$", ErrorMessage = "El campo Tipo de cuenta solo puede contener los valores 'AHORROS' o 'CORRIENTE'.")]
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? SaldoDisponible { get; set; }
        public bool Estado { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<MovimientoDTO>? Movimientos { get; set; } = null!;
    }
}
