namespace Util.DTO {
    public class MovimientoDTO {

        public MovimientoDTO() { }

        public DateTime Fecha { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public string NumeroCuenta { get; set; }
    }
}
