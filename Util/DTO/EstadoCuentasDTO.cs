namespace Util.DTO {
    public class EstadoCuentasDTO {
        public string NombreCliente { get; set; }
        public string Identificacion { get; set; }
        public List<CuentaDTO> Cuentas { get; set; }
    }
}
